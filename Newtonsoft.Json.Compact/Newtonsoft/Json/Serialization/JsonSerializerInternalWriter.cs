using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005D RID: 93
	internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000124EE File Offset: 0x000106EE
		private List<object> SerializeStack
		{
			get
			{
				if (this._serializeStack == null)
				{
					this._serializeStack = new List<object>();
				}
				return this._serializeStack;
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00012509 File Offset: 0x00010709
		public JsonSerializerInternalWriter(JsonSerializer serializer) : base(serializer)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00012512 File Offset: 0x00010712
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			if (jsonWriter == null)
			{
				throw new ArgumentNullException("jsonWriter");
			}
			this.SerializeValue(jsonWriter, value, null);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001252B File Offset: 0x0001072B
		private JsonSerializerProxy GetInternalSerializer()
		{
			if (this._internalSerializer == null)
			{
				this._internalSerializer = new JsonSerializerProxy(this);
			}
			return this._internalSerializer;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00012548 File Offset: 0x00010748
		private void SerializeValue(JsonWriter writer, object value, JsonConverter memberConverter)
		{
			JsonConverter jsonConverter = memberConverter;
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			if (jsonConverter != null || base.Serializer.HasClassConverter(value.GetType(), out jsonConverter) || base.Serializer.HasMatchingConverter(value.GetType(), out jsonConverter))
			{
				this.SerializeConvertable(writer, jsonConverter, value);
				return;
			}
			if (JsonConvert.IsJsonPrimitive(value))
			{
				writer.WriteValue(value);
				return;
			}
			if (value is JToken)
			{
				((JToken)value).WriteTo(writer, (base.Serializer.Converters != null) ? Enumerable.ToArray<JsonConverter>(base.Serializer.Converters) : null);
				return;
			}
			if (value is JsonRaw)
			{
				writer.WriteRawValue(((JsonRaw)value).Content);
				return;
			}
			JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(value.GetType());
			if (jsonContract is JsonObjectContract)
			{
				this.SerializeObject(writer, value, (JsonObjectContract)jsonContract);
				return;
			}
			if (jsonContract is JsonDictionaryContract)
			{
				this.SerializeDictionary(writer, (IDictionary)value, (JsonDictionaryContract)jsonContract);
				return;
			}
			if (!(jsonContract is JsonArrayContract))
			{
				return;
			}
			if (value is IList)
			{
				this.SerializeList(writer, (IList)value, (JsonArrayContract)jsonContract);
				return;
			}
			if (value is IEnumerable)
			{
				this.SerializeEnumerable(writer, (IEnumerable)value, (JsonArrayContract)jsonContract);
				return;
			}
			throw new Exception("Cannot serialize '{0}' into a JSON array. Type does not implement IEnumerable.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000126A8 File Offset: 0x000108A8
		private bool ShouldWriteReference(object value, JsonProperty property)
		{
			if (value == null)
			{
				return false;
			}
			if (JsonConvert.IsJsonPrimitive(value))
			{
				return false;
			}
			bool? flag = default(bool?);
			if (property != null)
			{
				flag = property.IsReference;
			}
			if (flag == null)
			{
				JsonContract jsonContract = base.Serializer.ContractResolver.ResolveContract(value.GetType());
				if (jsonContract != null)
				{
					flag = jsonContract.IsReference;
				}
			}
			if (flag == null)
			{
				if (value is IList)
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
				}
				else if (value is IDictionary)
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects));
				}
				else if (value is IEnumerable)
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays));
				}
				else
				{
					flag = new bool?(this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects));
				}
			}
			return flag.Value && base.Serializer.ReferenceResolver.IsReferenced(value);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000127AC File Offset: 0x000109AC
		private void WriteMemberInfoProperty(JsonWriter writer, object value, JsonProperty property)
		{
			MemberInfo member = property.Member;
			string propertyName = property.PropertyName;
			JsonConverter memberConverter = property.MemberConverter;
			object defaultValue = property.DefaultValue;
			if (!ReflectionUtils.IsIndexedProperty(member))
			{
				object memberValue = ReflectionUtils.GetMemberValue(member, value);
				if (property.NullValueHandling.GetValueOrDefault(base.Serializer.NullValueHandling) == NullValueHandling.Ignore && memberValue == null)
				{
					return;
				}
				if (property.DefaultValueHandling.GetValueOrDefault(base.Serializer.DefaultValueHandling) == DefaultValueHandling.Ignore && object.Equals(memberValue, defaultValue))
				{
					return;
				}
				if (this.ShouldWriteReference(memberValue, property))
				{
					writer.WritePropertyName(propertyName ?? member.Name);
					this.WriteReference(writer, memberValue);
					return;
				}
				if (!this.CheckForCircularReference(memberValue, property.ReferenceLoopHandling))
				{
					return;
				}
				writer.WritePropertyName(propertyName ?? member.Name);
				this.SerializeValue(writer, memberValue, memberConverter);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00012888 File Offset: 0x00010A88
		private bool CheckForCircularReference(object value, ReferenceLoopHandling? referenceLoopHandling)
		{
			if (this.SerializeStack.IndexOf(value) == -1)
			{
				return true;
			}
			switch (referenceLoopHandling.GetValueOrDefault(base.Serializer.ReferenceLoopHandling))
			{
			case ReferenceLoopHandling.Error:
				throw new JsonSerializationException("Self referencing loop");
			case ReferenceLoopHandling.Ignore:
				return false;
			case ReferenceLoopHandling.Serialize:
				return true;
			default:
				throw new InvalidOperationException("Unexpected ReferenceLoopHandling value: '{0}'".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.Serializer.ReferenceLoopHandling
				}));
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001290B File Offset: 0x00010B0B
		private void WriteReference(JsonWriter writer, object value)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("$ref");
			writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(value));
			writer.WriteEndObject();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001293B File Offset: 0x00010B3B
		internal static bool TryConvertToString(object value, Type type, out string s)
		{
			if (value is Guid || value is Type || value is Uri)
			{
				s = value.ToString();
				return true;
			}
			s = null;
			return false;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00012964 File Offset: 0x00010B64
		private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract)
		{
			contract.InvokeOnSerializing(value);
			string value2;
			if (JsonSerializerInternalWriter.TryConvertToString(value, contract.UnderlyingType, out value2))
			{
				writer.WriteValue(value2);
				return;
			}
			this.SerializeStack.Add(value);
			writer.WriteStartObject();
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
			if (flag)
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(value));
			}
			if (this.HasFlag(base.Serializer.TypeNameHandling, TypeNameHandling.Objects))
			{
				this.WriteTypeProperty(writer, contract.UnderlyingType);
			}
			int top = writer.Top;
			foreach (JsonProperty jsonProperty in contract.Properties)
			{
				try
				{
					if (!jsonProperty.Ignored && jsonProperty.Readable)
					{
						this.WriteMemberInfoProperty(writer, value, jsonProperty);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(value, contract, jsonProperty.Member.Name, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndObject();
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(value);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00012AD0 File Offset: 0x00010CD0
		private void WriteTypeProperty(JsonWriter writer, Type type)
		{
			writer.WritePropertyName("$type");
			writer.WriteValue(type.AssemblyQualifiedName);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00012AE9 File Offset: 0x00010CE9
		private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00012AF1 File Offset: 0x00010CF1
		private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
		{
			return (value & flag) == flag;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00012AFC File Offset: 0x00010CFC
		private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value)
		{
			if (this.ShouldWriteReference(value, null))
			{
				this.WriteReference(writer, value);
				return;
			}
			if (!this.CheckForCircularReference(value, default(ReferenceLoopHandling?)))
			{
				return;
			}
			this.SerializeStack.Add(value);
			converter.WriteJson(writer, value, this.GetInternalSerializer());
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00012B61 File Offset: 0x00010D61
		private void SerializeEnumerable(JsonWriter writer, IEnumerable values, JsonArrayContract contract)
		{
			this.SerializeList(writer, Enumerable.ToList<object>(Enumerable.Cast<object>(values)), contract);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00012B78 File Offset: 0x00010D78
		private void SerializeList(JsonWriter writer, IList values, JsonArrayContract contract)
		{
			contract.InvokeOnSerializing(values);
			this.SerializeStack.Add(values);
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Arrays);
			bool flag2 = this.HasFlag(base.Serializer.TypeNameHandling, TypeNameHandling.Arrays);
			if (flag || flag2)
			{
				writer.WriteStartObject();
				if (flag)
				{
					writer.WritePropertyName("$id");
					writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(values));
				}
				if (flag2)
				{
					this.WriteTypeProperty(writer, values.GetType());
				}
				writer.WritePropertyName("$values");
			}
			writer.WriteStartArray();
			int top = writer.Top;
			for (int i = 0; i < values.Count; i++)
			{
				try
				{
					object value = values[i];
					if (this.ShouldWriteReference(value, null))
					{
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(value, default(ReferenceLoopHandling?)))
					{
						this.SerializeValue(writer, value, null);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(values, contract, i, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndArray();
			if (flag)
			{
				writer.WriteEndObject();
			}
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00012CE4 File Offset: 0x00010EE4
		private void SerializeDictionary(JsonWriter writer, IDictionary values, JsonDictionaryContract contract)
		{
			contract.InvokeOnSerializing(values);
			this.SerializeStack.Add(values);
			writer.WriteStartObject();
			bool flag = contract.IsReference ?? this.HasFlag(base.Serializer.PreserveReferencesHandling, PreserveReferencesHandling.Objects);
			if (flag)
			{
				writer.WritePropertyName("$id");
				writer.WriteValue(base.Serializer.ReferenceResolver.GetReference(values));
			}
			if (this.HasFlag(base.Serializer.TypeNameHandling, TypeNameHandling.Objects))
			{
				this.WriteTypeProperty(writer, values.GetType());
			}
			int top = writer.Top;
			foreach (object obj in values)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				try
				{
					object value = dictionaryEntry.Value;
					if (this.ShouldWriteReference(value, null))
					{
						writer.WritePropertyName(text);
						this.WriteReference(writer, value);
					}
					else if (this.CheckForCircularReference(value, default(ReferenceLoopHandling?)))
					{
						writer.WritePropertyName(text);
						this.SerializeValue(writer, value, null);
					}
				}
				catch (Exception ex)
				{
					if (!base.IsErrorHandled(values, contract, text, ex))
					{
						throw;
					}
					this.HandleError(writer, top);
				}
			}
			writer.WriteEndObject();
			this.SerializeStack.RemoveAt(this.SerializeStack.Count - 1);
			contract.InvokeOnSerialized(values);
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00012E7C File Offset: 0x0001107C
		private void HandleError(JsonWriter writer, int initialDepth)
		{
			base.ClearErrorContext();
			while (writer.Top > initialDepth)
			{
				writer.WriteEnd();
			}
		}

		// Token: 0x04000192 RID: 402
		private JsonSerializerProxy _internalSerializer;

		// Token: 0x04000193 RID: 403
		private List<object> _serializeStack;
	}
}
