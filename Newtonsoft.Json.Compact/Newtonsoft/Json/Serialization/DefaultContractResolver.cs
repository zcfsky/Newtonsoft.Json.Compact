using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000044 RID: 68
	public class DefaultContractResolver : IContractResolver
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000E8E7 File Offset: 0x0000CAE7
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0000E8EF File Offset: 0x0000CAEF
		public BindingFlags DefaultMembersSearchFlags { get; set; }

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
		public DefaultContractResolver()
		{
			this.DefaultMembersSearchFlags = (BindingFlags)20;
			this._typeContractCache = new ThreadSafeStore<Type, JsonContract>(new Func<Type, JsonContract>(this.CreateContract));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000E91F File Offset: 0x0000CB1F
		public virtual JsonContract ResolveContract(Type type)
		{
			return this._typeContractCache.Get(type);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000E930 File Offset: 0x0000CB30
		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			List<MemberInfo> fieldsAndProperties = ReflectionUtils.GetFieldsAndProperties(objectType, this.DefaultMembersSearchFlags);
			List<MemberInfo> fieldsAndProperties2 = ReflectionUtils.GetFieldsAndProperties(objectType, (BindingFlags)60);
			List<MemberInfo> list = new List<MemberInfo>();
			foreach (MemberInfo memberInfo in fieldsAndProperties2)
			{
				if (fieldsAndProperties.Contains(memberInfo))
				{
					list.Add(memberInfo);
				}
				else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(memberInfo) != null)
				{
					list.Add(memberInfo);
				}
			}
			return list;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			this.InitializeContract(jsonObjectContract);
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType);
			jsonObjectContract.Properties.AddRange(this.CreateProperties(jsonObjectContract));
			if (jsonObjectContract.DefaultContstructor == null || jsonObjectContract.DefaultContstructor.IsPrivate)
			{
				jsonObjectContract.ParametrizedConstructor = this.GetParametrizedConstructor(objectType);
			}
			return jsonObjectContract;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000EA14 File Offset: 0x0000CC14
		private ConstructorInfo GetParametrizedConstructor(Type objectType)
		{
			ConstructorInfo[] constructors = objectType.GetConstructors((BindingFlags)20);
			if (constructors.Length == 1)
			{
				return constructors[0];
			}
			return null;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000EA38 File Offset: 0x0000CC38
		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(contract.UnderlyingType);
			if (jsonContainerAttribute != null)
			{
				contract.IsReference = jsonContainerAttribute._isReference;
			}
			contract.DefaultContstructor = (ReflectionUtils.GetDefaultConstructor(contract.CreatedType, false) ?? ReflectionUtils.GetDefaultConstructor(contract.CreatedType, true));
			foreach (MethodInfo methodInfo in contract.UnderlyingType.GetMethods((BindingFlags)54))
			{
				Type type = null;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (DefaultContractResolver.IsValidCallback(methodInfo, parameters, typeof(OnErrorAttribute), contract.OnError, ref type))
				{
					contract.OnError = methodInfo;
				}
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000EAD8 File Offset: 0x0000CCD8
		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			this.InitializeContract(jsonDictionaryContract);
			return jsonDictionaryContract;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			this.InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000EB10 File Offset: 0x0000CD10
		private JsonContract CreateContract(Type objectType)
		{
			if (JsonTypeReflector.GetJsonObjectAttribute(objectType) != null)
			{
				return this.CreateObjectContract(objectType);
			}
			if (CollectionUtils.IsDictionaryType(objectType))
			{
				return this.CreateDictionaryContract(objectType);
			}
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return this.CreateArrayContract(objectType);
			}
			return this.CreateObjectContract(objectType);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000EB60 File Offset: 0x0000CD60
		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new Exception("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					method,
					currentCallback,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					attributeType
				}));
			}
			if (prevAttributeType != null)
			{
				throw new Exception("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					prevAttributeType,
					attributeType,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method
				}));
			}
			if (method.IsVirtual)
			{
				throw new Exception("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					method,
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					attributeType
				}));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method
				}));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(StreamingContext) || parameters[1].ParameterType != typeof(ErrorContext))
				{
					throw new Exception("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
						method,
						typeof(StreamingContext),
						typeof(ErrorContext)
					}));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext))
			{
				throw new Exception("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					DefaultContractResolver.GetClrTypeFullName(method.DeclaringType),
					method,
					typeof(StreamingContext)
				}));
			}
			prevAttributeType = attributeType;
			return true;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000ED64 File Offset: 0x0000CF64
		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
			{
				return type.FullName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
			{
				type.Namespace,
				type.Name
			});
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		protected virtual IList<JsonProperty> CreateProperties(JsonObjectContract contract)
		{
			List<MemberInfo> serializableMembers = this.GetSerializableMembers(contract.UnderlyingType);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			}
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection();
			foreach (MemberInfo member in serializableMembers)
			{
				JsonProperty jsonProperty = this.CreateProperty(contract, member);
				if (jsonProperty != null)
				{
					jsonPropertyCollection.AddProperty(jsonProperty);
				}
			}
			return jsonPropertyCollection;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000EE34 File Offset: 0x0000D034
		protected virtual JsonProperty CreateProperty(JsonObjectContract contract, MemberInfo member)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.Member = member;
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(member);
			bool flag = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(member) != null;
			string propertyName;
			if (attribute != null && attribute.PropertyName != null)
			{
				propertyName = attribute.PropertyName;
			}
			else
			{
				propertyName = member.Name;
			}
			jsonProperty.PropertyName = this.ResolvePropertyName(propertyName);
			if (attribute != null)
			{
				jsonProperty.Required = attribute.IsRequired;
			}
			else
			{
				jsonProperty.Required = false;
			}
			jsonProperty.Ignored = (flag || (contract.MemberSerialization == MemberSerialization.OptIn && attribute == null));
			jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member);
			jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member);
			jsonProperty.MemberConverter = JsonTypeReflector.GetConverter(member, ReflectionUtils.GetMemberUnderlyingType(member));
			DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(member);
			jsonProperty.DefaultValue = ((attribute2 != null) ? attribute2.Value : null);
			jsonProperty.NullValueHandling = ((attribute != null) ? attribute._nullValueHandling : default(NullValueHandling?));
			jsonProperty.DefaultValueHandling = ((attribute != null) ? attribute._defaultValueHandling : default(DefaultValueHandling?));
			jsonProperty.ReferenceLoopHandling = ((attribute != null) ? attribute._referenceLoopHandling : default(ReferenceLoopHandling?));
			jsonProperty.IsReference = ((attribute != null) ? attribute._isReference : default(bool?));
			return jsonProperty;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000EF71 File Offset: 0x0000D171
		protected virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}

		// Token: 0x040000FD RID: 253
		internal static readonly IContractResolver Instance = new DefaultContractResolver();

		// Token: 0x040000FE RID: 254
		private readonly ThreadSafeStore<Type, JsonContract> _typeContractCache;
	}
}
