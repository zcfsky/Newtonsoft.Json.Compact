using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200003C RID: 60
	internal class JsonSchemaNode
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000DFE6 File Offset: 0x0000C1E6
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		public string Id { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000DFF7 File Offset: 0x0000C1F7
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000DFFF File Offset: 0x0000C1FF
		public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000E008 File Offset: 0x0000C208
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000E010 File Offset: 0x0000C210
		public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000E019 File Offset: 0x0000C219
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000E021 File Offset: 0x0000C221
		public List<JsonSchemaNode> Items { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000E02A File Offset: 0x0000C22A
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000E032 File Offset: 0x0000C232
		public JsonSchemaNode AdditionalProperties { get; set; }

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E03C File Offset: 0x0000C23C
		public JsonSchemaNode(JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[]
			{
				schema
			});
			this.Properties = new Dictionary<string, JsonSchemaNode>();
			this.Items = new List<JsonSchemaNode>();
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000E090 File Offset: 0x0000C290
		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			this.Schemas = new ReadOnlyCollection<JsonSchema>(Enumerable.ToList<JsonSchema>(Enumerable.Union<JsonSchema>(source.Schemas, new JsonSchema[]
			{
				schema
			})));
			this.Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			this.Items = new List<JsonSchemaNode>(source.Items);
			this.AdditionalProperties = source.AdditionalProperties;
			this.Id = JsonSchemaNode.GetId(this.Schemas);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E109 File Offset: 0x0000C309
		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E120 File Offset: 0x0000C320
		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(Enumerable.Select<JsonSchema, string>(schemata, (JsonSchema s) => s.InternalId), (string id) => id, StringComparer.Ordinal)));
		}
	}
}
