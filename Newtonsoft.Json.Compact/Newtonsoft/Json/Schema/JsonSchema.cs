using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000051 RID: 81
	public class JsonSchema
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0000F5DF File Offset: 0x0000D7DF
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x0000F5E7 File Offset: 0x0000D7E7
		public string Id { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		public string Title { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x0000F601 File Offset: 0x0000D801
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x0000F609 File Offset: 0x0000D809
		public bool? Optional { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x0000F612 File Offset: 0x0000D812
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x0000F61A File Offset: 0x0000D81A
		public bool? ReadOnly { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0000F623 File Offset: 0x0000D823
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x0000F62B File Offset: 0x0000D82B
		public bool? Hidden { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0000F634 File Offset: 0x0000D834
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x0000F63C File Offset: 0x0000D83C
		public bool? Transient { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0000F645 File Offset: 0x0000D845
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x0000F64D File Offset: 0x0000D84D
		public string Description { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0000F656 File Offset: 0x0000D856
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x0000F65E File Offset: 0x0000D85E
		public JsonSchemaType? Type { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0000F667 File Offset: 0x0000D867
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x0000F66F File Offset: 0x0000D86F
		public string Pattern { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0000F678 File Offset: 0x0000D878
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0000F680 File Offset: 0x0000D880
		public int? MinimumLength { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0000F689 File Offset: 0x0000D889
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0000F691 File Offset: 0x0000D891
		public int? MaximumLength { get; set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000F69A File Offset: 0x0000D89A
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0000F6A2 File Offset: 0x0000D8A2
		public int? MaximumDecimals { get; set; }

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000F6AB File Offset: 0x0000D8AB
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0000F6B3 File Offset: 0x0000D8B3
		public double? Minimum { get; set; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000F6BC File Offset: 0x0000D8BC
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		public double? Maximum { get; set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000F6CD File Offset: 0x0000D8CD
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x0000F6D5 File Offset: 0x0000D8D5
		public int? MinimumItems { get; set; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x0000F6E6 File Offset: 0x0000D8E6
		public int? MaximumItems { get; set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0000F6EF File Offset: 0x0000D8EF
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		public IList<JsonSchema> Items { get; set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000F700 File Offset: 0x0000D900
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x0000F708 File Offset: 0x0000D908
		public IDictionary<string, JsonSchema> Properties { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000F711 File Offset: 0x0000D911
		// (set) Token: 0x0600047B RID: 1147 RVA: 0x0000F719 File Offset: 0x0000D919
		public JsonSchema AdditionalProperties { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0000F722 File Offset: 0x0000D922
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0000F72A File Offset: 0x0000D92A
		public bool AllowAdditionalProperties { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0000F733 File Offset: 0x0000D933
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x0000F73B File Offset: 0x0000D93B
		public string Requires { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000F744 File Offset: 0x0000D944
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0000F74C File Offset: 0x0000D94C
		public IList<string> Identity { get; set; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000F755 File Offset: 0x0000D955
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0000F75D File Offset: 0x0000D95D
		public IList<JToken> Enum { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0000F766 File Offset: 0x0000D966
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x0000F76E File Offset: 0x0000D96E
		public IDictionary<JToken, string> Options { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0000F777 File Offset: 0x0000D977
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x0000F77F File Offset: 0x0000D97F
		public JsonSchemaType? Disallow { get; set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0000F788 File Offset: 0x0000D988
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x0000F790 File Offset: 0x0000D990
		public JToken Default { get; set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0000F799 File Offset: 0x0000D999
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0000F7A1 File Offset: 0x0000D9A1
		public JsonSchema Extends { get; set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0000F7AA File Offset: 0x0000D9AA
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x0000F7B2 File Offset: 0x0000D9B2
		public string Format { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000F7BB File Offset: 0x0000D9BB
		internal string InternalId
		{
			get
			{
				return this._internalId;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000F7C4 File Offset: 0x0000D9C4
		public JsonSchema()
		{
			this.AllowAdditionalProperties = true;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000F7F6 File Offset: 0x0000D9F6
		public static JsonSchema Read(JsonReader reader)
		{
			return JsonSchema.Read(reader, new JsonSchemaResolver());
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000F804 File Offset: 0x0000DA04
		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaBuilder jsonSchemaBuilder = new JsonSchemaBuilder(resolver);
			return jsonSchemaBuilder.Parse(reader);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000F835 File Offset: 0x0000DA35
		public static JsonSchema Parse(string json)
		{
			return JsonSchema.Parse(json, new JsonSchemaResolver());
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000F844 File Offset: 0x0000DA44
		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JsonSchema.Read(reader, resolver);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000F86F File Offset: 0x0000DA6F
		public void WriteTo(JsonWriter writer)
		{
			this.WriteTo(writer, new JsonSchemaResolver());
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000F880 File Offset: 0x0000DA80
		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaWriter jsonSchemaWriter = new JsonSchemaWriter(writer, resolver);
			jsonSchemaWriter.WriteSchema(this);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteTo(new JsonTextWriter(stringWriter)
			{
				Formatting = Formatting.Indented
			});
			return stringWriter.ToString();
		}

		// Token: 0x04000124 RID: 292
		private readonly string _internalId = Guid.NewGuid().ToString("N");
	}
}
