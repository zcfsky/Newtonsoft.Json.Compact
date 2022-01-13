using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000056 RID: 86
	public class JsonSchemaResolver
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00010F11 File Offset: 0x0000F111
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00010F19 File Offset: 0x0000F119
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		// Token: 0x060004C4 RID: 1220 RVA: 0x00010F22 File Offset: 0x0000F122
		public JsonSchemaResolver()
		{
			this.LoadedSchemas = new List<JsonSchema>();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00010F50 File Offset: 0x0000F150
		public virtual JsonSchema GetSchema(string id)
		{
			return Enumerable.SingleOrDefault<JsonSchema>(this.LoadedSchemas, (JsonSchema s) => s.Id == id);
		}
	}
}
