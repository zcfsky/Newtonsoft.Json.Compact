using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000079 RID: 121
	internal static class ReflectionUtils
	{
		// Token: 0x06000618 RID: 1560 RVA: 0x00015E37 File Offset: 0x00014037
		public static Type GetObjectType(object v)
		{
			if (v == null)
			{
				return null;
			}
			return v.GetType();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00015E44 File Offset: 0x00014044
		public static bool IsInstantiatableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsAbstract && !t.IsInterface && !t.IsArray && !t.IsGenericTypeDefinition && t != typeof(void) && ReflectionUtils.HasDefaultConstructor(t);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00015E96 File Offset: 0x00014096
		public static bool HasDefaultConstructor(Type t)
		{
			return ReflectionUtils.HasDefaultConstructor(t, false);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00015E9F File Offset: 0x0001409F
		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsValueType || ReflectionUtils.GetDefaultConstructor(t, nonPublic) != null;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00015EC3 File Offset: 0x000140C3
		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return ReflectionUtils.GetDefaultConstructor(t, false);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00015ECC File Offset: 0x000140CC
		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
            BindingFlags bindingFlags = (!nonPublic) ? (BindingFlags)16 : (BindingFlags)32;
            return t.GetConstructor(bindingFlags | (BindingFlags)4, null, new Type[0], null);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00015EF4 File Offset: 0x000140F4
		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return !t.IsValueType || ReflectionUtils.IsNullableType(t);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00015F11 File Offset: 0x00014111
		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00015F3C File Offset: 0x0001413C
		public static bool IsUnitializedValue(object value)
		{
			if (value == null)
			{
				return true;
			}
			object obj = ReflectionUtils.CreateUnitializedValue(value.GetType());
			return value.Equals(obj);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00015F64 File Offset: 0x00014164
		public static object CreateUnitializedValue(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type {0} is a generic type definition and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}), "type");
			}
			if (type.IsClass || type.IsInterface || type == typeof(void))
			{
				return null;
			}
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			throw new ArgumentException("Type {0} cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				type
			}), "type");
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00015FFD File Offset: 0x000141FD
		public static bool IsPropertyIndexed(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return !CollectionUtils.IsNullOrEmpty<ParameterInfo>(property.GetIndexParameters());
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016018 File Offset: 0x00014218
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type type2;
			return ReflectionUtils.ImplementsGenericDefinition(type, genericInterfaceDefinition, out type2);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00016030 File Offset: 0x00014230
		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					genericInterfaceDefinition
				}));
			}
			if (type.IsInterface && type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (type2.IsGenericType)
				{
					Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000160E8 File Offset: 0x000142E8
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type type2;
			return ReflectionUtils.InheritsGenericDefinition(type, genericClassDefinition, out type2);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00016100 File Offset: 0x00014300
		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					genericClassDefinition
				}));
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(type, type, genericClassDefinition, out implementingType);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00016160 File Offset: 0x00014360
		private static bool InheritsGenericDefinitionInternal(Type initialType, Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			if (currentType.IsGenericType)
			{
				Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.BaseType == null)
			{
				implementingType = null;
				return false;
			}
			return ReflectionUtils.InheritsGenericDefinitionInternal(initialType, currentType.BaseType, genericClassDefinition, out implementingType);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000161A0 File Offset: 0x000143A0
		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IEnumerable), out type2))
			{
				if (type2.IsGenericTypeDefinition)
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						type
					}));
				}
				return type2.GetGenericArguments()[0];
			}
			else
			{
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					return null;
				}
				throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00016244 File Offset: 0x00014444
		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			Type type;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionaryType, typeof(IDictionary), out type))
			{
				if (type.IsGenericTypeDefinition)
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						dictionaryType
					}));
				}
				Type[] genericArguments = type.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
				return;
			}
			else
			{
				if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					keyType = null;
					valueType = null;
					return;
				}
				throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					dictionaryType
				}));
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000162E4 File Offset: 0x000144E4
		public static Type GetDictionaryValueType(Type dictionaryType)
		{
			Type type;
			Type result;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out type, out result);
			return result;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000162FC File Offset: 0x000144FC
		public static Type GetDictionaryKeyType(Type dictionaryType)
		{
			Type result;
			Type type;
			ReflectionUtils.GetDictionaryKeyValueTypes(dictionaryType, out result, out type);
			return result;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00016314 File Offset: 0x00014514
		public static bool ItemsUnitializedValue<T>(IList<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type collectionItemType = ReflectionUtils.GetCollectionItemType(list.GetType());
			if (collectionItemType.IsValueType)
			{
				object obj = ReflectionUtils.CreateUnitializedValue(collectionItemType);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i];
					if (!t.Equals(obj))
					{
						return false;
					}
				}
			}
			else
			{
				if (!collectionItemType.IsClass)
				{
					throw new Exception("Type {0} is neither a ValueType or a Class.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						collectionItemType
					}));
				}
				for (int j = 0; j < list.Count; j++)
				{
					object obj2 = list[j];
					if (obj2 != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000163CC File Offset: 0x000145CC
		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			MemberTypes memberType = member.MemberType;
			switch ((int)memberType)
			{
			case 2:
				return ((EventInfo)member).EventHandlerType;
			case 3:
				break;
			case 4:
				return ((FieldInfo)member).FieldType;
			default:
				if ((int)memberType == 16)
				{
					return ((PropertyInfo)member).PropertyType;
				}
				break;
			}
			throw new ArgumentException("MemberInfo must be if type FieldInfo, PropertyInfo or EventInfo", "member");
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001643C File Offset: 0x0001463C
		public static bool IsIndexedProperty(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			PropertyInfo propertyInfo = member as PropertyInfo;
			return propertyInfo != null && ReflectionUtils.IsIndexedProperty(propertyInfo);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00016466 File Offset: 0x00014666
		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00016480 File Offset: 0x00014680
		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType;
			if ((int)memberType != 4)
			{
                if ((int)memberType == 16)
				{
					try
					{
						return ((PropertyInfo)member).GetValue(target, null);
					}
					catch (TargetParameterCountException ex)
					{
						throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							member.Name
						}), ex);
					}
				}
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					CultureInfo.InvariantCulture,
					member.Name
				}), "member");
			}
			return ((FieldInfo)member).GetValue(target);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00016544 File Offset: 0x00014744
		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			MemberTypes memberType = member.MemberType;
			if ((int)memberType == 4)
			{
				((FieldInfo)member).SetValue(target, value);
				return;
			}
			if ((int)memberType != 16)
			{
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					member.Name
				}), "member");
			}
			((PropertyInfo)member).SetValue(target, value, null);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000165C0 File Offset: 0x000147C0
		public static bool CanReadMemberValue(MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			return (int)memberType == 4 || ((int)memberType == 16 && ((PropertyInfo)member).CanRead);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000165F0 File Offset: 0x000147F0
		public static bool CanSetMemberValue(MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			return (int)memberType == 4 || ((int)memberType == 16 && ((PropertyInfo)member).CanWrite);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001661E File Offset: 0x0001481E
		public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
		{
			return ReflectionUtils.GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00016780 File Offset: 0x00014980
		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(type.GetFields(bindingAttr));
			list.AddRange(type.GetProperties(bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.Count);
			var enumerable = Enumerable.Select(Enumerable.GroupBy<MemberInfo, string>(list, (MemberInfo m) => m.Name), (IGrouping<string, MemberInfo> g) => new
			{
				Count = Enumerable.Count<MemberInfo>(g),
				Members = Enumerable.Cast<MemberInfo>(g)
			});
			foreach (var m in enumerable)
			{
				if (m.Count == 1)
				{
					list2.Add(Enumerable.First<MemberInfo>(m.Members));
				}
				else
				{
					list2.Add(Enumerable.First<MemberInfo>(Enumerable.Where<MemberInfo>(m.Members, (mm) => !ReflectionUtils.IsOverridenGenericMember(mm, bindingAttr))));
				}
			}
			return list2;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000168A0 File Offset: 0x00014AA0
		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
            if ((int)memberInfo.MemberType != 4 && (int)memberInfo.MemberType != 16)
			{
				throw new ArgumentException("Member must be a field or property.");
			}
			Type declaringType = memberInfo.DeclaringType;
			if (!declaringType.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(memberInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member[0]);
			return memberUnderlyingType.IsGenericParameter;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00016911 File Offset: 0x00014B11
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001691C File Offset: 0x00014B1C
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = ReflectionUtils.GetAttributes<T>(attributeProvider, inherit);
			return CollectionUtils.GetSingleItem<T>(attributes, true);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00016938 File Offset: 0x00014B38
		public static T[] GetAttributes<T>(ICustomAttributeProvider attributeProvider, bool inherit) where T : Attribute
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001695B File Offset: 0x00014B5B
		public static string GetNameAndAssessmblyName(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			return t.FullName + ", " + t.Assembly.GetName().Name;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00016988 File Offset: 0x00014B88
		public static Type MakeGenericType(Type genericTypeDefinition, params Type[] innerTypes)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentConditionTrue(genericTypeDefinition.IsGenericTypeDefinition, "genericTypeDefinition", "Type {0} is not a generic type definition.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				genericTypeDefinition
			}));
			return genericTypeDefinition.MakeGenericType(innerTypes);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000169E0 File Offset: 0x00014BE0
		public static object CreateGeneric(Type genericTypeDefinition, Type innerType, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, new Type[]
			{
				innerType
			}, args);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00016A0E File Offset: 0x00014C0E
		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, params object[] args)
		{
			return ReflectionUtils.CreateGeneric(genericTypeDefinition, innerTypes, (Type t, IList<object> a) => ReflectionUtils.CreateInstance(t, Enumerable.ToArray<object>(a)), args);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00016A38 File Offset: 0x00014C38
		public static object CreateGeneric(Type genericTypeDefinition, IList<Type> innerTypes, Func<Type, IList<object>, object> instanceCreator, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(genericTypeDefinition, "genericTypeDefinition");
			ValidationUtils.ArgumentNotNullOrEmpty<Type>(innerTypes, "innerTypes");
			ValidationUtils.ArgumentNotNull(instanceCreator, "createInstance");
			Type type = ReflectionUtils.MakeGenericType(genericTypeDefinition, Enumerable.ToArray<Type>(innerTypes));
			return instanceCreator.Invoke(type, args);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00016A7B File Offset: 0x00014C7B
		public static bool IsCompatibleValue(object value, Type type)
		{
			return (value == null && ReflectionUtils.IsNullable(type)) || type.IsAssignableFrom(value.GetType());
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00016AF4 File Offset: 0x00014CF4
		public static object CreateInstance(Type type, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ConstructorInfo[] constructors = type.GetConstructors();
			ConstructorInfo constructorInfo = Enumerable.FirstOrDefault<ConstructorInfo>(Enumerable.Where<ConstructorInfo>(constructors, delegate(ConstructorInfo c)
			{
				ParameterInfo[] parameters = c.GetParameters();
				if (parameters.Length != args.Length)
				{
					return false;
				}
				for (int i = 0; i < parameters.Length; i++)
				{
					ParameterInfo parameterInfo = parameters[i];
					object value = args[i];
					if (!ReflectionUtils.IsCompatibleValue(value, parameterInfo.ParameterType))
					{
						return false;
					}
				}
				return true;
			}));
			if (constructorInfo == null)
			{
				throw new Exception("Could not create '{0}' with given parameters.".FormatWith(CultureInfo.InvariantCulture, args));
			}
			return constructorInfo.Invoke(args);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00016B64 File Offset: 0x00014D64
		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = ReflectionUtils.GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex != null)
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
				return;
			}
			typeName = fullyQualifiedTypeName;
			assemblyName = null;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00016BC4 File Offset: 0x00014DC4
		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				char c = fullyQualifiedTypeName[i];
				char c2 = c;
				if (c2 != ',')
				{
					switch (c2)
					{
					case '[':
						num++;
						break;
					case ']':
						num--;
						break;
					}
				}
				else if (num == 0)
				{
					return new int?(i);
				}
			}
			return default(int?);
		}
	}
}
