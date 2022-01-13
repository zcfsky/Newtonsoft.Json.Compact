using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200003F RID: 63
	public class ValidationEventArgs : EventArgs
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0000E88B File Offset: 0x0000CA8B
		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			this._ex = ex;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000E8A5 File Offset: 0x0000CAA5
		public JsonSchemaException Exception
		{
			get
			{
				return this._ex;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000E8AD File Offset: 0x0000CAAD
		public string Message
		{
			get
			{
				return this._ex.Message;
			}
		}

		// Token: 0x040000FB RID: 251
		private readonly JsonSchemaException _ex;
	}
}
