using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200003D RID: 61
	internal class JsonSchemaWriter
	{
		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E186 File Offset: 0x0000C386
		public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = writer;
			this._resolver = resolver;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E1A8 File Offset: 0x0000C3A8
		private void ReferenceOrWriteSchema(JsonSchema schema)
		{
			if (schema.Id != null && this._resolver.GetSchema(schema.Id) != null)
			{
				this._writer.WriteStartObject();
				this._writer.WritePropertyName("$ref");
				this._writer.WriteValue(schema.Id);
				this._writer.WriteEndObject();
				return;
			}
			this.WriteSchema(schema);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E210 File Offset: 0x0000C410
		public void WriteSchema(JsonSchema schema)
		{
			ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!this._resolver.LoadedSchemas.Contains(schema))
			{
				this._resolver.LoadedSchemas.Add(schema);
			}
			this._writer.WriteStartObject();
			this.WritePropertyIfNotNull(this._writer, "id", schema.Id);
			this.WritePropertyIfNotNull(this._writer, "title", schema.Title);
			this.WritePropertyIfNotNull(this._writer, "description", schema.Description);
			this.WritePropertyIfNotNull(this._writer, "optional", schema.Optional);
			this.WritePropertyIfNotNull(this._writer, "readonly", schema.ReadOnly);
			this.WritePropertyIfNotNull(this._writer, "hidden", schema.Hidden);
			this.WritePropertyIfNotNull(this._writer, "transient", schema.Transient);
			if (schema.Type != null)
			{
				this.WriteType("type", this._writer, schema.Type.Value);
			}
			if (!schema.AllowAdditionalProperties)
			{
				this._writer.WritePropertyName("additionalProperties");
				this._writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				this._writer.WritePropertyName("additionalProperties");
				this.ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			if (schema.Properties != null)
			{
				this._writer.WritePropertyName("properties");
				this._writer.WriteStartObject();
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in schema.Properties)
				{
					this._writer.WritePropertyName(keyValuePair.Key);
					this.ReferenceOrWriteSchema(keyValuePair.Value);
				}
				this._writer.WriteEndObject();
			}
			this.WriteItems(schema);
			this.WritePropertyIfNotNull(this._writer, "minimum", schema.Minimum);
			this.WritePropertyIfNotNull(this._writer, "maximum", schema.Maximum);
			this.WritePropertyIfNotNull(this._writer, "minLength", schema.MinimumLength);
			this.WritePropertyIfNotNull(this._writer, "maxLength", schema.MaximumLength);
			this.WritePropertyIfNotNull(this._writer, "minItems", schema.MinimumItems);
			this.WritePropertyIfNotNull(this._writer, "maxItems", schema.MaximumItems);
			this.WritePropertyIfNotNull(this._writer, "maxDecimal", schema.MaximumDecimals);
			this.WritePropertyIfNotNull(this._writer, "format", schema.Format);
			this.WritePropertyIfNotNull(this._writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				this._writer.WritePropertyName("enum");
				this._writer.WriteStartArray();
				foreach (JToken jtoken in schema.Enum)
				{
					jtoken.WriteTo(this._writer, new JsonConverter[0]);
				}
				this._writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				this._writer.WritePropertyName("default");
				schema.Default.WriteTo(this._writer, new JsonConverter[0]);
			}
			if (schema.Options != null)
			{
				this._writer.WritePropertyName("options");
				this._writer.WriteStartArray();
				foreach (KeyValuePair<JToken, string> keyValuePair2 in schema.Options)
				{
					this._writer.WriteStartObject();
					this._writer.WritePropertyName("value");
					keyValuePair2.Key.WriteTo(this._writer, new JsonConverter[0]);
					if (keyValuePair2.Value != null)
					{
						this._writer.WritePropertyName("value");
						this._writer.WriteValue(keyValuePair2.Value);
					}
					this._writer.WriteEndObject();
				}
				this._writer.WriteEndArray();
			}
			if (schema.Disallow != null)
			{
				this.WriteType("disallow", this._writer, schema.Disallow.Value);
			}
			if (schema.Extends != null)
			{
				this._writer.WritePropertyName("extends");
				this.ReferenceOrWriteSchema(schema.Extends);
			}
			this._writer.WriteEndObject();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E6EC File Offset: 0x0000C8EC
		private void WriteItems(JsonSchema schema)
		{
			if (CollectionUtils.IsNullOrEmpty<JsonSchema>(schema.Items))
			{
				return;
			}
			this._writer.WritePropertyName("items");
			if (schema.Items.Count == 1)
			{
				this.ReferenceOrWriteSchema(schema.Items[0]);
				return;
			}
			this._writer.WriteStartArray();
			foreach (JsonSchema schema2 in schema.Items)
			{
				this.ReferenceOrWriteSchema(schema2);
			}
			this._writer.WriteEndArray();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E79C File Offset: 0x0000C99C
		private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
		{
			IList<JsonSchemaType> list2;
			if (Enum.IsDefined(typeof(JsonSchemaType), type))
			{
				List<JsonSchemaType> list = new List<JsonSchemaType>();
				list.Add(type);
				list2 = list;
			}
			else
			{
				list2 = Enumerable.ToList<JsonSchemaType>(Enumerable.Where<JsonSchemaType>(EnumUtils.GetFlagsValues<JsonSchemaType>(type), (JsonSchemaType v) => v != JsonSchemaType.None));
			}
			if (list2.Count == 0)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			if (list2.Count == 1)
			{
				writer.WriteValue(JsonSchemaBuilder.MapType(list2[0]));
				return;
			}
			writer.WriteStartArray();
			foreach (JsonSchemaType type2 in list2)
			{
				writer.WriteValue(JsonSchemaBuilder.MapType(type2));
			}
			writer.WriteEndArray();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E878 File Offset: 0x0000CA78
		private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}

		// Token: 0x040000F4 RID: 244
		private readonly JsonWriter _writer;

		// Token: 0x040000F5 RID: 245
		private readonly JsonSchemaResolver _resolver;
	}
}
