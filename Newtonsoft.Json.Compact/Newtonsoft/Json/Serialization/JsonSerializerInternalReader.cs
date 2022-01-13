using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005C RID: 92
	internal class JsonSerializerInternalReader : JsonSerializerInternalBase
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x00011274 File Offset: 0x0000F474
		public JsonSerializerInternalReader(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00011280 File Offset: 0x0000F480
		public void Populate(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(target, "target");
			Type type = target.GetType();
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(type);
			if (reader.TokenType == JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				this.PopulateList(CollectionUtils.CreateCollectionWrapper(target), reader, null, this.GetArrayContract(type));
				return;
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				return;
			}
			this.CheckedRead(reader);
			string id = null;
			if (reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", (StringComparison)4))
			{
				this.CheckedRead(reader);
				id = reader.Value.ToString();
				this.CheckedRead(reader);
			}
			if (jsonContract is JsonDictionaryContract)
			{
				this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(target), reader, (JsonDictionaryContract)jsonContract, id);
				return;
			}
			if (jsonContract is JsonObjectContract)
			{
				this.PopulateObject(target, reader, (JsonObjectContract)jsonContract, id);
				return;
			}
			throw new JsonSerializationException("Expected a JsonObjectContract or JsonDictionaryContract for type '{0}', got '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				type,
				jsonContract.GetType()
			}));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00011391 File Offset: 0x0000F591
		public object Deserialize(JsonReader reader, Type objectType)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				return null;
			}
			return this.CreateValue(reader, objectType, null, null);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000113BD File Offset: 0x0000F5BD
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000113DC File Offset: 0x0000F5DC
		private JToken CreateJToken(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteToken(reader);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00011428 File Offset: 0x0000F628
		private JToken CreateJObject(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jtokenWriter.WriteStartObject();
				if (reader.TokenType == JsonToken.PropertyName)
				{
					jtokenWriter.WriteToken(reader, reader.Depth - 1);
				}
				else
				{
					jtokenWriter.WriteEndObject();
				}
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00011490 File Offset: 0x0000F690
		private object CreateValue(JsonReader reader, Type objectType, object existingValue, JsonConverter memberConverter)
		{
			if (memberConverter != null)
			{
				return memberConverter.ReadJson(reader, objectType, this.GetInternalSerializer());
			}
			JsonConverter jsonConverter;
			if (objectType != null && base.Serializer.HasClassConverter(objectType, out jsonConverter))
			{
				return jsonConverter.ReadJson(reader, objectType, this.GetInternalSerializer());
			}
			if (objectType != null && base.Serializer.HasMatchingConverter(objectType, out jsonConverter))
			{
				return jsonConverter.ReadJson(reader, objectType, this.GetInternalSerializer());
			}
			if (objectType == typeof(JsonRaw))
			{
				return JsonRaw.Create(reader);
			}
			for (;;)
			{
				switch (reader.TokenType)
				{
				case JsonToken.StartObject:
					goto IL_BD;
				case JsonToken.StartArray:
					goto IL_C7;
				case JsonToken.StartConstructor:
				case JsonToken.EndConstructor:
					goto IL_10C;
				case JsonToken.Comment:
					if (!reader.Read())
					{
						goto Block_13;
					}
					continue;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
					goto IL_D2;
				case JsonToken.Null:
				case JsonToken.Undefined:
					goto IL_11A;
				}
				break;
			}
			goto IL_12F;
			IL_BD:
			return this.CreateObject(reader, objectType, existingValue);
			IL_C7:
			return this.CreateList(reader, objectType, existingValue, null);
			IL_D2:
			if (reader.Value is string && string.IsNullOrEmpty((string)reader.Value) && objectType != null && ReflectionUtils.IsNullable(objectType))
			{
				return null;
			}
			return this.EnsureType(reader.Value, objectType);
			IL_10C:
			return reader.Value.ToString();
			IL_11A:
			if (objectType == typeof(DBNull))
			{
				return DBNull.Value;
			}
			return null;
			IL_12F:
			throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
			Block_13:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000115FC File Offset: 0x0000F7FC
		private object CreateObject(JsonReader reader, Type objectType, object existingValue)
		{
			this.CheckedRead(reader);
			string text = null;
			if (reader.TokenType == JsonToken.PropertyName)
			{
				string text3;
				Type type;
				for (;;)
				{
					string text2 = reader.Value.ToString();
					if (string.Equals(text2, "$ref", (StringComparison)4))
					{
						break;
					}
					bool flag;
                    if (string.Equals(text2, "$type", (StringComparison)4))
					{
						this.CheckedRead(reader);
						text3 = reader.Value.ToString();
						this.CheckedRead(reader);
						if (base.Serializer.TypeNameHandling != TypeNameHandling.None)
						{
							string typeName;
							string assemblyName;
							ReflectionUtils.SplitFullyQualifiedTypeName(text3, out typeName, out assemblyName);
							try
							{
								type = base.Serializer.Binder.BindToType(assemblyName, typeName);
							}
							catch (Exception innerException)
							{
								throw new JsonSerializationException("Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
								{
									text3
								}), innerException);
							}
							if (type == null)
							{
								goto Block_5;
							}
							if (objectType != null && !objectType.IsAssignableFrom(type))
							{
								goto Block_7;
							}
							objectType = type;
						}
						flag = true;
					}
					else if (string.Equals(text2, "$id", (StringComparison)4))
					{
						this.CheckedRead(reader);
						text = reader.Value.ToString();
						this.CheckedRead(reader);
						flag = true;
					}
					else
					{
						if (string.Equals(text2, "$values", (StringComparison)4))
						{
							goto Block_9;
						}
						flag = false;
					}
					if (!flag || reader.TokenType != JsonToken.PropertyName)
					{
						goto IL_1BD;
					}
				}
				this.CheckedRead(reader);
				string reference = reader.Value.ToString();
				this.CheckedRead(reader);
				return base.Serializer.ReferenceResolver.ResolveReference(reference);
				Block_5:
				throw new JsonSerializationException("Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text3
				}));
				Block_7:
				throw new JsonSerializationException("Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type.AssemblyQualifiedName,
					objectType.AssemblyQualifiedName
				}));
				Block_9:
				this.CheckedRead(reader);
				object result = this.CreateList(reader, objectType, existingValue, text);
				this.CheckedRead(reader);
				return result;
			}
			IL_1BD:
			if (!this.HasDefinedType(objectType))
			{
				return this.CreateJObject(reader);
			}
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(objectType);
			if (jsonContract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			if (jsonContract is JsonDictionaryContract)
			{
				if (existingValue == null)
				{
					return this.CreateAndPopulateDictionary(reader, (JsonDictionaryContract)jsonContract, text);
				}
				return this.PopulateDictionary(CollectionUtils.CreateDictionaryWrapper(existingValue), reader, (JsonDictionaryContract)jsonContract, text);
			}
			else
			{
				if (!(jsonContract is JsonObjectContract))
				{
					throw new JsonSerializationException("Expected a JsonObjectContract or JsonDictionaryContract for type '{0}', got '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						objectType,
						jsonContract.GetType()
					}));
				}
				if (existingValue == null)
				{
					return this.CreateAndPopulateObject(reader, (JsonObjectContract)jsonContract, text);
				}
				return this.PopulateObject(existingValue, reader, (JsonObjectContract)jsonContract, text);
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000118B0 File Offset: 0x0000FAB0
		private JsonArrayContract GetArrayContract(Type objectType)
		{
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(objectType);
			if (jsonContract == null)
			{
				throw new JsonSerializationException("Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
			JsonArrayContract jsonArrayContract = jsonContract as JsonArrayContract;
			if (jsonArrayContract == null)
			{
				throw new JsonSerializationException("Expected a JsonArrayContract for type '{0}', got '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType,
					jsonContract.GetType()
				}));
			}
			return jsonArrayContract;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00011928 File Offset: 0x0000FB28
		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end when deserializing object.");
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00011940 File Offset: 0x0000FB40
		private object CreateList(JsonReader reader, Type objectType, object existingValue, string reference)
		{
			object result;
			if (this.HasDefinedType(objectType))
			{
				JsonArrayContract arrayContract = this.GetArrayContract(objectType);
				if (existingValue == null)
				{
					result = this.CreateAndPopulateList(reader, reference, arrayContract);
				}
				else
				{
					result = this.PopulateList(CollectionUtils.CreateCollectionWrapper(existingValue), reader, reference, arrayContract);
				}
			}
			else
			{
				result = this.CreateJToken(reader);
			}
			return result;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001198A File Offset: 0x0000FB8A
		private bool HasDefinedType(Type type)
		{
			return type != null && type != typeof(object);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000119A4 File Offset: 0x0000FBA4
		private object EnsureType(object value, Type targetType)
		{
			if (value == null)
			{
				return null;
			}
			if (targetType == null)
			{
				return value;
			}
			Type type = value.GetType();
			if (type != targetType)
			{
				return ConvertUtils.ConvertOrCast(value, CultureInfo.InvariantCulture, targetType);
			}
			return value;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000119D4 File Offset: 0x0000FBD4
		private void SetObjectMember(JsonReader reader, object target, JsonObjectContract contract, string memberName)
		{
			JsonProperty property;
			if (contract.Properties.TryGetClosestMatchProperty(memberName, out property))
			{
				this.SetPropertyValue(property, reader, target);
				return;
			}
			if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
			{
				throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					memberName,
					contract.UnderlyingType.Name
				}));
			}
			reader.Skip();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00011A40 File Offset: 0x0000FC40
		private void SetPropertyValue(JsonProperty property, JsonReader reader, object target)
		{
			if (property.Ignored)
			{
				reader.Skip();
				return;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(property.Member);
			object obj = null;
			bool flag = false;
			if ((base.Serializer.ObjectCreationHandling == ObjectCreationHandling.Auto || base.Serializer.ObjectCreationHandling == ObjectCreationHandling.Reuse) && (reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject) && property.Readable)
			{
				obj = ReflectionUtils.GetMemberValue(property.Member, target);
				flag = (obj != null && !memberUnderlyingType.IsArray && !ReflectionUtils.InheritsGenericDefinition(memberUnderlyingType, typeof(ReadOnlyCollection<>)));
			}
			if (!property.Writable && !flag)
			{
				reader.Skip();
				return;
			}
			object value = this.CreateValue(reader, memberUnderlyingType, flag ? obj : null, JsonTypeReflector.GetConverter(property.Member, memberUnderlyingType));
			if (!flag && this.ShouldSetPropertyValue(property, value))
			{
				ReflectionUtils.SetMemberValue(property.Member, target, value);
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00011B18 File Offset: 0x0000FD18
		private bool ShouldSetPropertyValue(JsonProperty property, object value)
		{
			return (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) != NullValueHandling.Ignore || value != null) && (property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling) != DefaultValueHandling.Ignore || !object.Equals(value, property.DefaultValue)) && property.Writable;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00011B80 File Offset: 0x0000FD80
		private object CreateAndPopulateDictionary(JsonReader reader, JsonDictionaryContract contract, string id)
		{
			IWrappedDictionary wrappedDictionary = contract.CreateWrapper(contract.DefaultContstructor.Invoke(null));
			this.PopulateDictionary(wrappedDictionary, reader, contract, id);
			return wrappedDictionary.UnderlyingDictionary;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00011BB4 File Offset: 0x0000FDB4
		private IDictionary PopulateDictionary(IWrappedDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, string id)
		{
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(id, dictionary.UnderlyingDictionary);
			}
			contract.InvokeOnDeserializing(dictionary.UnderlyingDictionary);
			int depth = reader.Depth;
			JsonToken tokenType;
			for (;;)
			{
				tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					break;
				}
				object obj = this.EnsureType(reader.Value, contract.DictionaryKeyType);
				this.CheckedRead(reader);
				try
				{
					dictionary.Add(obj, this.CreateValue(reader, contract.DictionaryValueType, null, null));
					goto IL_B7;
				}
				catch (Exception ex)
				{
					if (base.IsErrorHandled(dictionary, contract, obj, ex))
					{
						this.HandleError(reader, depth);
						goto IL_B7;
					}
					throw;
				}
				goto IL_8E;
				IL_B7:
				if (!reader.Read())
				{
					goto Block_5;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			IL_8E:
			contract.InvokeOnDeserialized(dictionary.UnderlyingDictionary);
			return dictionary;
			Block_5:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00011D50 File Offset: 0x0000FF50
		private object CreateAndPopulateList(JsonReader reader, string reference, JsonArrayContract contract)
		{
			return CollectionUtils.CreateAndPopulateList(contract.CreatedType, delegate(IList l, bool isTemporaryListReference)
			{
				if (reference != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot preserve reference to array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						contract.UnderlyingType
					}));
				}
				if (contract.OnError != null && isTemporaryListReference)
				{
					throw new JsonSerializationException("Cannot call OnError on an array or readonly list: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						contract.UnderlyingType
					}));
				}
				this.PopulateList(contract.CreateWrapper(l), reader, reference, contract);
			});
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00011D9C File Offset: 0x0000FF9C
		private object PopulateList(IWrappedCollection wrappedList, JsonReader reader, string reference, JsonArrayContract contract)
		{
			object underlyingCollection = wrappedList.UnderlyingCollection;
			if (reference != null)
			{
				base.Serializer.ReferenceResolver.AddReference(reference, underlyingCollection);
			}
			contract.InvokeOnDeserializing(underlyingCollection);
			int depth = reader.Depth;
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						contract.InvokeOnDeserialized(underlyingCollection);
						return wrappedList.UnderlyingCollection;
					}
					try
					{
						object obj = this.CreateValue(reader, contract.CollectionItemType, null, null);
						wrappedList.Add(obj);
					}
					catch (Exception ex)
					{
						if (!base.IsErrorHandled(underlyingCollection, contract, wrappedList.Count, ex))
						{
							throw;
						}
						this.HandleError(reader, depth);
					}
				}
			}
			throw new JsonSerializationException("Unexpected end when deserializing array.");
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00011E5C File Offset: 0x0001005C
		private object CreateAndPopulateObject(JsonReader reader, JsonObjectContract contract, string id)
		{
			if (contract.UnderlyingType.IsInterface || contract.UnderlyingType.IsAbstract)
			{
				throw new JsonSerializationException("Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantated.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					contract.UnderlyingType
				}));
			}
			if (contract.DefaultContstructor != null && (contract.DefaultContstructor.IsPublic || base.Serializer.ConstructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object obj = contract.DefaultContstructor.Invoke(null);
				this.PopulateObject(obj, reader, contract, id);
				return obj;
			}
			return this.CreateObjectFromNonDefaultConstructor(reader, contract, id);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00011F14 File Offset: 0x00010114
		private object CreateObjectFromNonDefaultConstructor(JsonReader reader, JsonObjectContract contract, string id)
		{
			Type underlyingType = contract.UnderlyingType;
			if (contract.ParametrizedConstructor == null)
			{
				throw new JsonSerializationException("Unable to find a constructor to use for type {0}. A class should either have a default constructor or only one constructor with arguments.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					underlyingType
				}));
			}
			IDictionary<JsonProperty, object> dictionary = Enumerable.ToDictionary<JsonProperty, JsonProperty, object>(Enumerable.Where<JsonProperty>(contract.Properties, (JsonProperty p) => !p.Ignored), (JsonProperty kv) => kv, (JsonProperty kv) => null);
			bool flag = false;
			string text;
			for (;;)
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.EndObject)
					{
						break;
					}
					flag = true;
				}
				else
				{
					text = reader.Value.ToString();
					if (!reader.Read())
					{
						goto Block_7;
					}
					JsonProperty jsonProperty;
					if (contract.Properties.TryGetClosestMatchProperty(text, out jsonProperty))
					{
						if (!jsonProperty.Ignored)
						{
							Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(jsonProperty.Member);
							dictionary[jsonProperty] = this.CreateValue(reader, memberUnderlyingType, null, jsonProperty.MemberConverter);
						}
					}
					else
					{
						if (base.Serializer.MissingMemberHandling == MissingMemberHandling.Error)
						{
							goto Block_10;
						}
						reader.Skip();
					}
				}
				if (flag || !reader.Read())
				{
					goto IL_1A6;
				}
			}
			throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			Block_7:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text
			}));
			Block_10:
			throw new JsonSerializationException("Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text,
				underlyingType.Name
			}));
			IL_1A6:
			IDictionary<ParameterInfo, object> dictionary2 = Enumerable.ToDictionary<ParameterInfo, ParameterInfo, object>(contract.ParametrizedConstructor.GetParameters(), (ParameterInfo p) => p, (ParameterInfo p) => null);
			IDictionary<JsonProperty, object> dictionary3 = new Dictionary<JsonProperty, object>();
			foreach (KeyValuePair<JsonProperty, object> keyValuePair in dictionary)
			{
				ParameterInfo key = dictionary2.ForgivingCaseSensitiveFind((KeyValuePair<ParameterInfo, object> kv) => kv.Key.Name, keyValuePair.Key.PropertyName).Key;
				if (key != null)
				{
					dictionary2[key] = keyValuePair.Value;
				}
				else
				{
					dictionary3.Add(keyValuePair);
				}
			}
			object obj = contract.ParametrizedConstructor.Invoke(Enumerable.ToArray<object>(dictionary2.Values));
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(id, obj);
			}
			contract.InvokeOnDeserializing(obj);
			foreach (KeyValuePair<JsonProperty, object> keyValuePair2 in dictionary3)
			{
				if (this.ShouldSetPropertyValue(keyValuePair2.Key, keyValuePair2.Value))
				{
					ReflectionUtils.SetMemberValue(keyValuePair2.Key.Member, obj, keyValuePair2.Value);
				}
			}
			contract.InvokeOnDeserialized(obj);
			return obj;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001226C File Offset: 0x0001046C
		private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, string id)
		{
			contract.InvokeOnDeserializing(newObject);
			Dictionary<string, bool> dictionary = Enumerable.ToDictionary<JsonProperty, string, bool>(Enumerable.Where<JsonProperty>(contract.Properties, (JsonProperty m) => m.Required), (JsonProperty m) => m.PropertyName, (JsonProperty m) => false);
			if (id != null)
			{
				base.Serializer.ReferenceResolver.AddReference(id, newObject);
			}
			int depth = reader.Depth;
			JsonToken tokenType;
			string text;
			for (;;)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.PropertyName:
					text = reader.Value.ToString();
					if (!reader.Read())
					{
						goto Block_7;
					}
					this.SetRequiredProperty(text, dictionary);
					try
					{
						this.SetObjectMember(reader, newObject, contract, text);
						goto IL_1A1;
					}
					catch (Exception ex)
					{
						if (base.IsErrorHandled(newObject, contract, text, ex))
						{
							this.HandleError(reader, depth);
							goto IL_1A1;
						}
						throw;
					}
					goto IL_11E;
				case JsonToken.Comment:
					goto IL_1A1;
				}
				break;
				IL_1A1:
				if (!reader.Read())
				{
					goto Block_10;
				}
			}
			if (tokenType != JsonToken.EndObject)
			{
				throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
			}
			goto IL_11E;
			Block_7:
			throw new JsonSerializationException("Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				text
			}));
			IL_11E:
			foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
			{
				if (!keyValuePair.Value)
				{
					throw new JsonSerializationException("Required property '{0}' not found in JSON.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						keyValuePair.Key
					}));
				}
			}
			contract.InvokeOnDeserialized(newObject);
			return newObject;
			Block_10:
			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001244C File Offset: 0x0001064C
		private void SetRequiredProperty(string memberName, Dictionary<string, bool> requiredProperties)
		{
			if (requiredProperties.ContainsKey(memberName))
			{
				requiredProperties[memberName] = true;
				return;
			}
			foreach (KeyValuePair<string, bool> keyValuePair in requiredProperties)
			{
				if (string.Compare(keyValuePair.Key, keyValuePair.Key, (StringComparison)5) == 0)
				{
					requiredProperties[keyValuePair.Key] = true;
					break;
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000124CC File Offset: 0x000106CC
		private void HandleError(JsonReader reader, int initialDepth)
		{
			base.ClearErrorContext();
			reader.Skip();
			while (reader.Depth > initialDepth + 1)
			{
				reader.Read();
			}
		}

		// Token: 0x04000188 RID: 392
		private JsonSerializerProxy _internalSerializer;
	}
}
