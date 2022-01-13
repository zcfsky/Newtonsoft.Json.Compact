using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000039 RID: 57
	internal class JsonSchemaModel
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000D95E File Offset: 0x0000BB5E
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0000D966 File Offset: 0x0000BB66
		public bool Optional { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000D96F File Offset: 0x0000BB6F
		// (set) Token: 0x060003BF RID: 959 RVA: 0x0000D977 File Offset: 0x0000BB77
		public JsonSchemaType Type { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000D980 File Offset: 0x0000BB80
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0000D988 File Offset: 0x0000BB88
		public int? MinimumLength { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000D991 File Offset: 0x0000BB91
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x0000D999 File Offset: 0x0000BB99
		public int? MaximumLength { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000D9A2 File Offset: 0x0000BBA2
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000D9AA File Offset: 0x0000BBAA
		public int? MaximumDecimals { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000D9B3 File Offset: 0x0000BBB3
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000D9BB File Offset: 0x0000BBBB
		public double? Minimum { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public double? Maximum { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003CA RID: 970 RVA: 0x0000D9D5 File Offset: 0x0000BBD5
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0000D9DD File Offset: 0x0000BBDD
		public int? MinimumItems { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003CC RID: 972 RVA: 0x0000D9E6 File Offset: 0x0000BBE6
		// (set) Token: 0x060003CD RID: 973 RVA: 0x0000D9EE File Offset: 0x0000BBEE
		public int? MaximumItems { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003CE RID: 974 RVA: 0x0000D9F7 File Offset: 0x0000BBF7
		// (set) Token: 0x060003CF RID: 975 RVA: 0x0000D9FF File Offset: 0x0000BBFF
		public IList<string> Patterns { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x0000DA08 File Offset: 0x0000BC08
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x0000DA10 File Offset: 0x0000BC10
		public IList<JsonSchemaModel> Items { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000DA19 File Offset: 0x0000BC19
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0000DA21 File Offset: 0x0000BC21
		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000DA2A File Offset: 0x0000BC2A
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000DA32 File Offset: 0x0000BC32
		public JsonSchemaModel AdditionalProperties { get; set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000DA3B File Offset: 0x0000BC3B
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x0000DA43 File Offset: 0x0000BC43
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000DA54 File Offset: 0x0000BC54
		public IList<JToken> Enum { get; set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000DA5D File Offset: 0x0000BC5D
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000DA65 File Offset: 0x0000BC65
		public JsonSchemaType Disallow { get; set; }

		// Token: 0x060003DC RID: 988 RVA: 0x0000DA6E File Offset: 0x0000BC6E
		public JsonSchemaModel()
		{
			this.Type = JsonSchemaType.Any;
			this.AllowAdditionalProperties = true;
			this.Optional = true;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schema in schemata)
			{
				JsonSchemaModel.Combine(jsonSchemaModel, schema);
			}
			return jsonSchemaModel;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000DADC File Offset: 0x0000BCDC
		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Optional = (model.Optional && (schema.Optional ?? false));
			model.Type &= (schema.Type ?? JsonSchemaType.Any);
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.MaximumDecimals = MathUtils.Min(model.MaximumDecimals, schema.MaximumDecimals);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.AllowAdditionalProperties = (model.AllowAdditionalProperties && schema.AllowAdditionalProperties);
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, new JTokenEqualityComparer());
			}
			model.Disallow |= (schema.Disallow ?? JsonSchemaType.None);
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
}
