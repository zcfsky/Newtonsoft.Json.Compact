using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000054 RID: 84
	public class JsonSchemaGenerator
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00010705 File Offset: 0x0000E905
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0001070D File Offset: 0x0000E90D
		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x00010716 File Offset: 0x0000E916
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x0001072C File Offset: 0x0000E92C
		public IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00010735 File Offset: 0x0000E935
		private JsonSchema CurrentSchema
		{
			get
			{
				return this._currentSchema;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001073D File Offset: 0x0000E93D
		private Type CurrentType
		{
			get
			{
				return this._currentType;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00010745 File Offset: 0x0000E945
		private void Push(JsonSchemaGenerator.TypeSchema typeSchema)
		{
			this._currentType = typeSchema.Type;
			this._currentSchema = typeSchema.Schema;
			this._stack.Add(typeSchema);
			this._resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00010784 File Offset: 0x0000E984
		private JsonSchemaGenerator.TypeSchema Pop()
		{
			JsonSchemaGenerator.TypeSchema result = this._stack[this._stack.Count - 1];
			this._stack.RemoveAt(this._stack.Count - 1);
			JsonSchemaGenerator.TypeSchema typeSchema = Enumerable.LastOrDefault<JsonSchemaGenerator.TypeSchema>(this._stack);
			if (typeSchema != null)
			{
				this._currentType = typeSchema.Type;
				this._currentSchema = typeSchema.Schema;
			}
			else
			{
				this._currentType = null;
				this._currentSchema = null;
			}
			return result;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x000107FA File Offset: 0x0000E9FA
		public JsonSchema Generate(Type type)
		{
			return this.Generate(type, new JsonSchemaResolver(), false);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00010809 File Offset: 0x0000EA09
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return this.Generate(type, resolver, false);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00010814 File Offset: 0x0000EA14
		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return this.Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00010823 File Offset: 0x0000EA23
		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			this._resolver = resolver;
			return this.GenerateInternal(type, !rootSchemaNullable);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00010850 File Offset: 0x0000EA50
		private string GetTitle(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Title))
			{
				return jsonContainerAttribute.Title;
			}
			return null;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001087C File Offset: 0x0000EA7C
		private string GetDescription(Type type)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Description))
			{
				return jsonContainerAttribute.Description;
			}
			return null;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000108A8 File Offset: 0x0000EAA8
		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute jsonContainerAttribute = JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Id))
			{
				return jsonContainerAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			switch (this.UndefinedSchemaIdHandling)
			{
			case UndefinedSchemaIdHandling.UseTypeName:
				return type.FullName;
			case UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
				return type.AssemblyQualifiedName;
			default:
				return null;
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00010918 File Offset: 0x0000EB18
		private JsonSchema GenerateInternal(Type type, bool valueRequired)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = this.GetTypeId(type, false);
			string typeId2 = this.GetTypeId(type, true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = this._resolver.GetSchema(typeId);
				if (schema != null)
				{
					return schema;
				}
			}
			if (Enumerable.Any<JsonSchemaGenerator.TypeSchema>(this._stack, (JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new Exception("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type
				}));
			}
			this.Push(new JsonSchemaGenerator.TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				this.CurrentSchema.Id = typeId2;
			}
			this.CurrentSchema.Title = this.GetTitle(type);
			this.CurrentSchema.Description = this.GetDescription(type);
			if (CollectionUtils.IsDictionaryType(type))
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Object);
				Type type2;
				Type type3;
				ReflectionUtils.GetDictionaryKeyValueTypes(type, out type2, out type3);
				if (type2 != null && typeof(IConvertible).IsAssignableFrom(type2))
				{
					this.CurrentSchema.AdditionalProperties = this.GenerateInternal(type3, false);
				}
			}
			else if (CollectionUtils.IsCollectionType(type))
			{
				this.CurrentSchema.Type = new JsonSchemaType?(JsonSchemaType.Array);
				JsonArrayAttribute jsonArrayAttribute = JsonTypeReflector.GetJsonContainerAttribute(type) as JsonArrayAttribute;
				bool flag = jsonArrayAttribute != null && jsonArrayAttribute.AllowNullItems;
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
				if (collectionItemType != null)
				{
					this.CurrentSchema.Items = new List<JsonSchema>();
					this.CurrentSchema.Items.Add(this.GenerateInternal(collectionItemType, !flag));
				}
			}
			else
			{
				this.CurrentSchema.Type = new JsonSchemaType?(this.GetJsonSchemaType(type, valueRequired));
				if (JsonSchemaGenerator.HasFlag(this.CurrentSchema.Type, JsonSchemaType.Object))
				{
					this.CurrentSchema.Id = this.GetTypeId(type, false);
					JsonObjectContract jsonObjectContract = this.ContractResolver.ResolveContract(type) as JsonObjectContract;
					if (jsonObjectContract == null)
					{
						throw new Exception("Could not resolve contract for '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
						{
							type
						}));
					}
					this.CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
					foreach (JsonProperty jsonProperty in jsonObjectContract.Properties)
					{
						if (!jsonProperty.Ignored)
						{
							Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(jsonProperty.Member);
							JsonSchema jsonSchema = this.GenerateInternal(memberUnderlyingType, jsonProperty.Required);
							if (jsonProperty.DefaultValue != null)
							{
								jsonSchema.Default = JToken.FromObject(jsonProperty.DefaultValue);
							}
							this.CurrentSchema.Properties.Add(jsonProperty.PropertyName, jsonSchema);
						}
					}
					if (type.IsSealed)
					{
						this.CurrentSchema.AllowAdditionalProperties = false;
					}
				}
				else if (this.CurrentSchema.Type == JsonSchemaType.Integer && type.IsEnum && !type.IsDefined(typeof(FlagsAttribute), true))
				{
					this.CurrentSchema.Enum = new List<JToken>();
					this.CurrentSchema.Options = new Dictionary<JToken, string>();
					EnumValues<long> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
					foreach (EnumValue<long> enumValue in namesAndValues)
					{
						JToken jtoken = JToken.FromObject(enumValue.Value);
						this.CurrentSchema.Enum.Add(jtoken);
						this.CurrentSchema.Options.Add(jtoken, enumValue.Name);
					}
				}
			}
			return this.Pop().Schema;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00010D6C File Offset: 0x0000EF6C
		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			return value == null || (value & flag) == flag;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		private JsonSchemaType GetJsonSchemaType(Type type, bool valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (!valueRequired && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			TypeCode typeCode = Type.GetTypeCode(type);
			switch ((int)typeCode)
			{
			case 0:
			case 1:
				if (ConvertUtils.CanConvertType(type, typeof(string), false))
				{
					return jsonSchemaType | JsonSchemaType.String;
				}
				return jsonSchemaType | JsonSchemaType.Object;
			case 2:
				return jsonSchemaType | JsonSchemaType.Null;
			case 3:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case 4:
				return jsonSchemaType | JsonSchemaType.String;
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return jsonSchemaType | JsonSchemaType.Integer;
			case 13:
			case 14:
			case 15:
				return jsonSchemaType | JsonSchemaType.Float;
			case 16:
				return jsonSchemaType | JsonSchemaType.String;
			case 18:
				return jsonSchemaType | JsonSchemaType.String;
			}
			throw new Exception("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeCode,
				type
			}));
		}

		// Token: 0x04000164 RID: 356
		private IContractResolver _contractResolver;

		// Token: 0x04000165 RID: 357
		private JsonSchemaResolver _resolver;

		// Token: 0x04000166 RID: 358
		private IList<JsonSchemaGenerator.TypeSchema> _stack = new List<JsonSchemaGenerator.TypeSchema>();

		// Token: 0x04000167 RID: 359
		private JsonSchema _currentSchema;

		// Token: 0x04000168 RID: 360
		private Type _currentType;

		// Token: 0x02000055 RID: 85
		private class TypeSchema
		{
			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x060004BD RID: 1213 RVA: 0x00010EC3 File Offset: 0x0000F0C3
			// (set) Token: 0x060004BE RID: 1214 RVA: 0x00010ECB File Offset: 0x0000F0CB
			public Type Type { get; private set; }

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x060004BF RID: 1215 RVA: 0x00010ED4 File Offset: 0x0000F0D4
			// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00010EDC File Offset: 0x0000F0DC
			public JsonSchema Schema { get; private set; }

			// Token: 0x060004C1 RID: 1217 RVA: 0x00010EE5 File Offset: 0x0000F0E5
			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				this.Type = type;
				this.Schema = schema;
			}
		}
	}
}
