using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000011 RID: 17
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000A6 RID: 166 RVA: 0x00003574 File Offset: 0x00001774
		// (remove) Token: 0x060000A7 RID: 167 RVA: 0x0000358D File Offset: 0x0000178D
		public event ValidationEventHandler ValidationEventHandler;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000035A6 File Offset: 0x000017A6
		public override object Value
		{
			get
			{
				return this._reader.Value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000035B3 File Offset: 0x000017B3
		public override int Depth
		{
			get
			{
				return this._reader.Depth;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000035C0 File Offset: 0x000017C0
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000035CD File Offset: 0x000017CD
		public override char QuoteChar
		{
			get
			{
				return this._reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000035CF File Offset: 0x000017CF
		public override JsonToken TokenType
		{
			get
			{
				return this._reader.TokenType;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000035DC File Offset: 0x000017DC
		public override Type ValueType
		{
			get
			{
				return this._reader.ValueType;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000035E9 File Offset: 0x000017E9
		private void Push(JsonValidatingReader.SchemaScope scope)
		{
			this._stack.Push(scope);
			this._currentScope = scope;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003600 File Offset: 0x00001800
		private JsonValidatingReader.SchemaScope Pop()
		{
			JsonValidatingReader.SchemaScope result = this._stack.Pop();
			this._currentScope = ((this._stack.Count != 0) ? this._stack.Peek() : null);
			return result;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x0000363B File Offset: 0x0000183B
		private JsonSchemaModel CurrentSchema
		{
			get
			{
				return this._currentScope.Schema;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003648 File Offset: 0x00001848
		private JsonSchemaModel CurrentMemberSchema
		{
			get
			{
				if (this._currentScope == null)
				{
					return this._model;
				}
				if (this._currentScope.Schema == null)
				{
					return null;
				}
				switch (this._currentScope.TokenType)
				{
				case JTokenType.None:
					return this._currentScope.Schema;
				case JTokenType.Object:
				{
					if (this._currentScope.CurrentPropertyName == null)
					{
						throw new Exception("CurrentPropertyName has not been set on scope.");
					}
					JsonSchemaModel result;
					if (this.CurrentSchema.Properties.TryGetValue(this._currentScope.CurrentPropertyName, out result))
					{
						return result;
					}
					if (!this.CurrentSchema.AllowAdditionalProperties)
					{
						return null;
					}
					return this.CurrentSchema.AdditionalProperties;
				}
				case JTokenType.Array:
					if (!CollectionUtils.IsNullOrEmpty<JsonSchemaModel>(this.CurrentSchema.Items))
					{
						if (this.CurrentSchema.Items.Count == 1)
						{
							return this.CurrentSchema.Items[0];
						}
						if (this.CurrentSchema.Items.Count > this._currentScope.ArrayItemCount - 1)
						{
							return this.CurrentSchema.Items[this._currentScope.ArrayItemCount - 1];
						}
					}
					if (!this.CurrentSchema.AllowAdditionalProperties)
					{
						return null;
					}
					return this.CurrentSchema.AdditionalProperties;
				case JTokenType.Constructor:
					return null;
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._currentScope.TokenType
					}));
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000037C0 File Offset: 0x000019C0
		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string message2 = ((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				((IJsonLineInfo)this).LineNumber,
				((IJsonLineInfo)this).LinePosition
			})) : message;
			this.OnValidationEvent(new JsonSchemaException(message2, null, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003830 File Offset: 0x00001A30
		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003856 File Offset: 0x00001A56
		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this._reader = reader;
			this._stack = new Stack<JsonValidatingReader.SchemaScope>();
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000387B File Offset: 0x00001A7B
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003883 File Offset: 0x00001A83
		public JsonSchema Schema
		{
			get
			{
				return this._schema;
			}
			set
			{
				if (this.TokenType != JsonToken.None)
				{
					throw new Exception("Cannot change schema while validating JSON.");
				}
				this._schema = value;
				this._model = null;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000038A6 File Offset: 0x00001AA6
		public JsonReader Reader
		{
			get
			{
				return this._reader;
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000038B0 File Offset: 0x00001AB0
		private void ValidateInEnumAndNotDisallowed(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			JToken jtoken = new JValue(this._reader.Value);
			if (schema.Enum != null && !schema.Enum.ContainsValue(jtoken, new JTokenEqualityComparer()))
			{
				this.RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtoken
				}), schema);
			}
			JsonSchemaType? currentNodeSchemaType = this.GetCurrentNodeSchemaType();
			if (currentNodeSchemaType != null && JsonSchemaGenerator.HasFlag(new JsonSchemaType?(schema.Disallow), currentNodeSchemaType.Value))
			{
				this.RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					currentNodeSchemaType
				}), schema);
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003960 File Offset: 0x00001B60
		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
				return new JsonSchemaType?(JsonSchemaType.Object);
			case JsonToken.StartArray:
				return new JsonSchemaType?(JsonSchemaType.Array);
			case JsonToken.Integer:
				return new JsonSchemaType?(JsonSchemaType.Integer);
			case JsonToken.Float:
				return new JsonSchemaType?(JsonSchemaType.Float);
			case JsonToken.String:
				return new JsonSchemaType?(JsonSchemaType.String);
			case JsonToken.Boolean:
				return new JsonSchemaType?(JsonSchemaType.Boolean);
			case JsonToken.Null:
				return new JsonSchemaType?(JsonSchemaType.Null);
			}
			return default(JsonSchemaType?);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000039EC File Offset: 0x00001BEC
		public override bool Read()
		{
			if (!this._reader.Read())
			{
				return false;
			}
			if (this._reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			if (this._model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				this._model = jsonSchemaModelBuilder.Build(this._schema);
			}
			switch (this._reader.TokenType)
			{
			case JsonToken.StartObject:
			{
				this.ProcessValue();
				JsonSchemaModel schema = this.ValidateObject(this.CurrentMemberSchema) ? this.CurrentMemberSchema : null;
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Object, schema));
				return true;
			}
			case JsonToken.StartArray:
			{
				this.ProcessValue();
				JsonSchemaModel schema2 = this.ValidateArray(this.CurrentMemberSchema) ? this.CurrentMemberSchema : null;
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Array, schema2));
				return true;
			}
			case JsonToken.StartConstructor:
				this.Push(new JsonValidatingReader.SchemaScope(JTokenType.Constructor, null));
				return true;
			case JsonToken.PropertyName:
				this.ValidatePropertyName(this.CurrentSchema);
				return true;
			case JsonToken.Raw:
			case JsonToken.Undefined:
			case JsonToken.Date:
				return true;
			case JsonToken.Integer:
				this.ProcessValue();
				this.ValidateInteger(this.CurrentMemberSchema);
				return true;
			case JsonToken.Float:
				this.ProcessValue();
				this.ValidateFloat(this.CurrentMemberSchema);
				return true;
			case JsonToken.String:
				this.ProcessValue();
				this.ValidateString(this.CurrentMemberSchema);
				return true;
			case JsonToken.Boolean:
				this.ProcessValue();
				this.ValidateBoolean(this.CurrentMemberSchema);
				return true;
			case JsonToken.Null:
				this.ProcessValue();
				this.ValidateNull(this.CurrentMemberSchema);
				return true;
			case JsonToken.EndObject:
				this.ValidateEndObject(this.CurrentSchema);
				this.Pop();
				return true;
			case JsonToken.EndArray:
				this.ValidateEndArray(this.CurrentSchema);
				this.Pop();
				return true;
			case JsonToken.EndConstructor:
				this.Pop();
				return true;
			}
			throw new ArgumentOutOfRangeException();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003BCC File Offset: 0x00001DCC
		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = this._currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				List<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, bool>, string>(Enumerable.Where<KeyValuePair<string, bool>>(requiredProperties, (KeyValuePair<string, bool> kv) => !kv.Value), (KeyValuePair<string, bool> kv) => kv.Key));
				if (list.Count > 0)
				{
					this.RaiseError("Non-optional properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						string.Join(", ", list.ToArray())
					}), schema);
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003C74 File Offset: 0x00001E74
		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			int arrayItemCount = this._currentScope.ArrayItemCount;
			if (schema.MaximumItems != null && arrayItemCount > schema.MaximumItems)
			{
				this.RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					arrayItemCount,
					schema.MaximumItems
				}), schema);
			}
			if (schema.MinimumItems != null && arrayItemCount < schema.MinimumItems)
			{
				this.RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					arrayItemCount,
					schema.MinimumItems
				}), schema);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003D65 File Offset: 0x00001F65
		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Null))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003D7E File Offset: 0x00001F7E
		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Boolean))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003D98 File Offset: 0x00001F98
		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			string text = this._reader.Value.ToString();
			if (schema.MaximumLength != null && text.Length > schema.MaximumLength)
			{
				this.RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text,
					schema.MaximumLength
				}), schema);
			}
			if (schema.MinimumLength != null && text.Length < schema.MinimumLength)
			{
				this.RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					text,
					schema.MinimumLength
				}), schema);
			}
			if (schema.Patterns != null)
			{
				foreach (string text2 in schema.Patterns)
				{
					if (!Regex.IsMatch(text, text2))
					{
						this.RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							text,
							text2
						}), schema);
					}
				}
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003F1C File Offset: 0x0000211C
		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			long num = Convert.ToInt64(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = (double)num;
				double? maximum = schema.Maximum;
				if (num2 > maximum.GetValueOrDefault() && maximum != null)
				{
					this.RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						num,
						schema.Maximum
					}), schema);
				}
			}
			if (schema.Minimum != null)
			{
				double num3 = (double)num;
				double? minimum = schema.Minimum;
				if (num3 < minimum.GetValueOrDefault() && minimum != null)
				{
					this.RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						num,
						schema.Minimum
					}), schema);
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004030 File Offset: 0x00002230
		private void ProcessValue()
		{
			if (this._currentScope != null && this._currentScope.TokenType == JTokenType.Array)
			{
				this._currentScope.ArrayItemCount++;
				if (this.CurrentSchema != null && this.CurrentSchema.Items != null && this.CurrentSchema.Items.Count > 1 && this._currentScope.ArrayItemCount >= this.CurrentSchema.Items.Count)
				{
					this.RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						this._currentScope.ArrayItemCount
					}), this.CurrentSchema);
				}
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000040E8 File Offset: 0x000022E8
		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			if (!this.TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			this.ValidateInEnumAndNotDisallowed(schema);
			double num = Convert.ToDouble(this._reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum != null)
			{
				double num2 = num;
				double? maximum = schema.Maximum;
				if (num2 > maximum.GetValueOrDefault() && maximum != null)
				{
					this.RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						JsonConvert.ToString(num),
						schema.Maximum
					}), schema);
				}
			}
			if (schema.Minimum != null)
			{
				double num3 = num;
				double? minimum = schema.Minimum;
				if (num3 < minimum.GetValueOrDefault() && minimum != null)
				{
					this.RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						JsonConvert.ToString(num),
						schema.Minimum
					}), schema);
				}
			}
			if (schema.MaximumDecimals != null && MathUtils.GetDecimalPlaces(num) > schema.MaximumDecimals)
			{
				this.RaiseError("Float {0} exceeds the maximum allowed number decimal places of {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JsonConvert.ToString(num),
					schema.MaximumDecimals
				}), schema);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004274 File Offset: 0x00002474
		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			string text = Convert.ToString(this._reader.Value, CultureInfo.InvariantCulture);
			if (this._currentScope.RequiredProperties.ContainsKey(text))
			{
				this._currentScope.RequiredProperties[text] = true;
			}
			if (!schema.Properties.ContainsKey(text))
			{
				IList<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, JsonSchemaModel>, string>(schema.Properties, (KeyValuePair<string, JsonSchemaModel> p) => p.Key));
				if (!schema.AllowAdditionalProperties && !list.Contains(text))
				{
					this.RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						text
					}), schema);
				}
			}
			this._currentScope.CurrentPropertyName = text;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000433B File Offset: 0x0000253B
		private bool ValidateArray(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Array);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000434B File Offset: 0x0000254B
		private bool ValidateObject(JsonSchemaModel schema)
		{
			return schema == null || this.TestType(schema, JsonSchemaType.Object);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000435C File Offset: 0x0000255C
		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(new JsonSchemaType?(currentSchema.Type), currentType))
			{
				this.RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					currentSchema.Type,
					currentType
				}), currentSchema);
				return false;
			}
			return true;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000043B4 File Offset: 0x000025B4
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000043D8 File Offset: 0x000025D8
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000043FC File Offset: 0x000025FC
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._reader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x0400003E RID: 62
		private readonly JsonReader _reader;

		// Token: 0x0400003F RID: 63
		private readonly Stack<JsonValidatingReader.SchemaScope> _stack;

		// Token: 0x04000040 RID: 64
		private JsonSchema _schema;

		// Token: 0x04000041 RID: 65
		private JsonSchemaModel _model;

		// Token: 0x04000042 RID: 66
		private JsonValidatingReader.SchemaScope _currentScope;

		// Token: 0x02000012 RID: 18
		private class SchemaScope
		{
			// Token: 0x1700003C RID: 60
			// (get) Token: 0x060000CD RID: 205 RVA: 0x00004420 File Offset: 0x00002620
			// (set) Token: 0x060000CE RID: 206 RVA: 0x00004428 File Offset: 0x00002628
			public string CurrentPropertyName { get; set; }

			// Token: 0x1700003D RID: 61
			// (get) Token: 0x060000CF RID: 207 RVA: 0x00004431 File Offset: 0x00002631
			// (set) Token: 0x060000D0 RID: 208 RVA: 0x00004439 File Offset: 0x00002639
			public int ArrayItemCount { get; set; }

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004442 File Offset: 0x00002642
			public JsonSchemaModel Schema
			{
				get
				{
					return this._schema;
				}
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000444A File Offset: 0x0000264A
			public Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return this._requiredProperties;
				}
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004452 File Offset: 0x00002652
			public JTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
			}

			// Token: 0x060000D4 RID: 212 RVA: 0x00004460 File Offset: 0x00002660
			public SchemaScope(JTokenType tokenType, JsonSchemaModel schema)
			{
				this._tokenType = tokenType;
				this._schema = schema;
				if (this._schema != null && this._schema.Properties != null)
				{
					this._requiredProperties = Enumerable.ToDictionary<string, string, bool>(Enumerable.Distinct<string>(this.GetRequiredProperties(this._schema)), (string p) => p, (string p) => false);
				}
			}

			// Token: 0x060000D5 RID: 213 RVA: 0x00004508 File Offset: 0x00002708
			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				return Enumerable.Select<KeyValuePair<string, JsonSchemaModel>, string>(Enumerable.Where<KeyValuePair<string, JsonSchemaModel>>(schema.Properties, (KeyValuePair<string, JsonSchemaModel> p) => !p.Value.Optional), (KeyValuePair<string, JsonSchemaModel> p) => p.Key);
			}

			// Token: 0x04000047 RID: 71
			private readonly JTokenType _tokenType;

			// Token: 0x04000048 RID: 72
			private readonly JsonSchemaModel _schema;

			// Token: 0x04000049 RID: 73
			private readonly Dictionary<string, bool> _requiredProperties;
		}
	}
}
