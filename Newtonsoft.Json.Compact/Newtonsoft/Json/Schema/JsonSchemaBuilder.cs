using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000052 RID: 82
	internal class JsonSchemaBuilder
	{
        private class PrivateImplementationDetails{
            internal static Dictionary<string, int> method1;
        }
		// Token: 0x06000497 RID: 1175 RVA: 0x0000F8E7 File Offset: 0x0000DAE7
		private void Push(JsonSchema value)
		{
			this._currentSchema = value;
			this._stack.Add(value);
			this._resolver.LoadedSchemas.Add(value);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000F910 File Offset: 0x0000DB10
		private JsonSchema Pop()
		{
			JsonSchema currentSchema = this._currentSchema;
			this._stack.RemoveAt(this._stack.Count - 1);
			this._currentSchema = Enumerable.LastOrDefault<JsonSchema>(this._stack);
			return currentSchema;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000F94E File Offset: 0x0000DB4E
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000F956 File Offset: 0x0000DB56
		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			this._stack = new List<JsonSchema>();
			this._resolver = resolver;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000F970 File Offset: 0x0000DB70
		internal JsonSchema Parse(JsonReader reader)
		{
			this._reader = reader;
			if (reader.TokenType == JsonToken.None)
			{
				this._reader.Read();
			}
			return this.BuildSchema();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000F994 File Offset: 0x0000DB94
		private JsonSchema BuildSchema()
		{
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			this._reader.Read();
			if (this._reader.TokenType == JsonToken.EndObject)
			{
				this.Push(new JsonSchema());
				return this.Pop();
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			this._reader.Read();
			if (!(text == "$ref"))
			{
				this.Push(new JsonSchema());
				this.ProcessSchemaProperty(text);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
					this._reader.Read();
					this.ProcessSchemaProperty(text);
				}
				return this.Pop();
			}
			string text2 = (string)this._reader.Value;
			this._reader.Read();
			JsonSchema schema = this._resolver.GetSchema(text2);
			if (schema == null)
			{
				throw new Exception("Could not resolve schema reference for Id '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text2
				}));
			}
			return schema;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		private void ProcessSchemaProperty(string propertyName)
		{
			if (propertyName != null)
			{
				if (PrivateImplementationDetails.method1 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(26);
					dictionary.Add("type", 0);
					dictionary.Add("id", 1);
					dictionary.Add("title", 2);
					dictionary.Add("description", 3);
					dictionary.Add("properties", 4);
					dictionary.Add("items", 5);
					dictionary.Add("additionalProperties", 6);
					dictionary.Add("optional", 7);
					dictionary.Add("requires", 8);
					dictionary.Add("identity", 9);
					dictionary.Add("minimum", 10);
					dictionary.Add("maximum", 11);
					dictionary.Add("maxLength", 12);
					dictionary.Add("minLength", 13);
					dictionary.Add("maxItems", 14);
					dictionary.Add("minItems", 15);
					dictionary.Add("maxDecimal", 16);
					dictionary.Add("disallow", 17);
					dictionary.Add("default", 18);
					dictionary.Add("hidden", 19);
					dictionary.Add("readonly", 20);
					dictionary.Add("format", 21);
					dictionary.Add("pattern", 22);
					dictionary.Add("options", 23);
					dictionary.Add("enum", 24);
					dictionary.Add("extends", 25);
					PrivateImplementationDetails.method1 = dictionary;
				}
				int num;
                if (PrivateImplementationDetails.method1.TryGetValue(propertyName, out num))
				{
					switch (num)
					{
					case 0:
						this.CurrentSchema.Type = this.ProcessType();
						return;
					case 1:
						this.CurrentSchema.Id = (string)this._reader.Value;
						return;
					case 2:
						this.CurrentSchema.Title = (string)this._reader.Value;
						return;
					case 3:
						this.CurrentSchema.Description = (string)this._reader.Value;
						return;
					case 4:
						this.ProcessProperties();
						return;
					case 5:
						this.ProcessItems();
						return;
					case 6:
						this.ProcessAdditionalProperties();
						return;
					case 7:
						this.CurrentSchema.Optional = new bool?((bool)this._reader.Value);
						return;
					case 8:
						this.CurrentSchema.Requires = (string)this._reader.Value;
						return;
					case 9:
						this.ProcessIdentity();
						return;
					case 10:
						this.CurrentSchema.Minimum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 11:
						this.CurrentSchema.Maximum = new double?(Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 12:
						this.CurrentSchema.MaximumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 13:
						this.CurrentSchema.MinimumLength = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 14:
						this.CurrentSchema.MaximumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 15:
						this.CurrentSchema.MinimumItems = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 16:
						this.CurrentSchema.MaximumDecimals = new int?(Convert.ToInt32(this._reader.Value, CultureInfo.InvariantCulture));
						return;
					case 17:
						this.CurrentSchema.Disallow = this.ProcessType();
						return;
					case 18:
						this.ProcessDefault();
						return;
					case 19:
						this.CurrentSchema.Hidden = new bool?((bool)this._reader.Value);
						return;
					case 20:
						this.CurrentSchema.ReadOnly = new bool?((bool)this._reader.Value);
						return;
					case 21:
						this.CurrentSchema.Format = (string)this._reader.Value;
						return;
					case 22:
						this.CurrentSchema.Pattern = (string)this._reader.Value;
						return;
					case 23:
						this.ProcessOptions();
						return;
					case 24:
						this.ProcessEnum();
						return;
					case 25:
						this.ProcessExtends();
						return;
					}
				}
			}
			this._reader.Skip();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000FF6B File Offset: 0x0000E16B
		private void ProcessExtends()
		{
			this.CurrentSchema.Extends = this.BuildSchema();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000FF80 File Offset: 0x0000E180
		private void ProcessEnum()
		{
			if (this._reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected StartArray token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			this.CurrentSchema.Enum = new List<JToken>();
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
			{
				JToken jtoken = JToken.ReadFrom(this._reader);
				this.CurrentSchema.Enum.Add(jtoken);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00010018 File Offset: 0x0000E218
		private void ProcessOptions()
		{
			this.CurrentSchema.Options = new Dictionary<JToken, string>(new JTokenEqualityComparer());
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType != JsonToken.StartArray)
			{
				throw new Exception("Expected array token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read())
			{
				if (this._reader.TokenType == JsonToken.EndArray)
				{
					return;
				}
				if (this._reader.TokenType != JsonToken.StartObject)
				{
					throw new Exception("Expect object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._reader.TokenType
					}));
				}
				string text = null;
				JToken jtoken = null;
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
				{
					string text2 = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
					this._reader.Read();
					string text3;
					if ((text3 = text2) != null)
					{
						if (text3 == "value")
						{
							jtoken = JToken.ReadFrom(this._reader);
							continue;
						}
						if (text3 == "label")
						{
							text = (string)this._reader.Value;
							continue;
						}
					}
					throw new Exception("Unexpected property in JSON schema option: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text2
					}));
				}
				if (jtoken == null)
				{
					throw new Exception("No value specified for JSON schema option.");
				}
				if (this.CurrentSchema.Options.ContainsKey(jtoken))
				{
					throw new Exception("Duplicate value in JSON schema option collection: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						jtoken
					}));
				}
				this.CurrentSchema.Options.Add(jtoken, text);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000101F0 File Offset: 0x0000E3F0
		private void ProcessDefault()
		{
			this.CurrentSchema.Default = JToken.ReadFrom(this._reader);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00010208 File Offset: 0x0000E408
		private void ProcessIdentity()
		{
			this.CurrentSchema.Identity = new List<string>();
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType == JsonToken.StartArray)
			{
				while (this._reader.Read())
				{
					if (this._reader.TokenType == JsonToken.EndArray)
					{
						return;
					}
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._reader.TokenType
						}));
					}
					this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
				}
				return;
			}
			if (tokenType == JsonToken.String)
			{
				this.CurrentSchema.Identity.Add(this._reader.Value.ToString());
				return;
			}
			throw new Exception("Expected array or JSON property name string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this._reader.TokenType
			}));
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001030D File Offset: 0x0000E50D
		private void ProcessAdditionalProperties()
		{
			if (this._reader.TokenType == JsonToken.Boolean)
			{
				this.CurrentSchema.AllowAdditionalProperties = (bool)this._reader.Value;
				return;
			}
			this.CurrentSchema.AdditionalProperties = this.BuildSchema();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001034C File Offset: 0x0000E54C
		private void ProcessItems()
		{
			this.CurrentSchema.Items = new List<JsonSchema>();
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				this.CurrentSchema.Items.Add(this.BuildSchema());
				return;
			case JsonToken.StartArray:
				while (this._reader.Read())
				{
					if (this._reader.TokenType == JsonToken.EndArray)
					{
						return;
					}
					this.CurrentSchema.Items.Add(this.BuildSchema());
				}
				return;
			default:
				throw new Exception("Expected array or JSON schema object token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00010404 File Offset: 0x0000E604
		private void ProcessProperties()
		{
			IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (this._reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Expected StartObject token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					this._reader.TokenType
				}));
			}
			while (this._reader.Read() && this._reader.TokenType != JsonToken.EndObject)
			{
				string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
				this._reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new Exception("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}));
				}
				dictionary.Add(text, this.BuildSchema());
			}
			this.CurrentSchema.Properties = dictionary;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000104DC File Offset: 0x0000E6DC
		private JsonSchemaType? ProcessType()
		{
			JsonToken tokenType = this._reader.TokenType;
			if (tokenType == JsonToken.StartArray)
			{
				JsonSchemaType? jsonSchemaType = new JsonSchemaType?(JsonSchemaType.None);
				while (this._reader.Read() && this._reader.TokenType != JsonToken.EndArray)
				{
					if (this._reader.TokenType != JsonToken.String)
					{
						throw new Exception("Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							this._reader.TokenType
						}));
					}
					jsonSchemaType |= JsonSchemaBuilder.MapType(this._reader.Value.ToString());
				}
				return jsonSchemaType;
			}
			if (tokenType == JsonToken.String)
			{
				return new JsonSchemaType?(JsonSchemaBuilder.MapType(this._reader.Value.ToString()));
			}
			throw new Exception("Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this._reader.TokenType
			}));
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000105F8 File Offset: 0x0000E7F8
		internal static JsonSchemaType MapType(string type)
		{
			JsonSchemaType result;
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out result))
			{
				throw new Exception("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
			return result;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00010650 File Offset: 0x0000E850
		internal static string MapType(JsonSchemaType type)
		{
			return Enumerable.Single<KeyValuePair<string, JsonSchemaType>>(JsonSchemaConstants.JsonSchemaTypeMapping, (KeyValuePair<string, JsonSchemaType> kv) => kv.Value == type).Key;
		}

		// Token: 0x04000141 RID: 321
		private JsonReader _reader;

		// Token: 0x04000142 RID: 322
		private readonly IList<JsonSchema> _stack;

		// Token: 0x04000143 RID: 323
		private readonly JsonSchemaResolver _resolver;

		// Token: 0x04000144 RID: 324
		private JsonSchema _currentSchema;
	}
}
