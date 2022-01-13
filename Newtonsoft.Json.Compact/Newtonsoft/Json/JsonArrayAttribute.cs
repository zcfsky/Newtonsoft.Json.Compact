using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200000A RID: 10
    [AttributeUsage((AttributeTargets)1028, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002F73 File Offset: 0x00001173
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002F7B File Offset: 0x0000117B
		public bool AllowNullItems
		{
			get
			{
				return this._allowNullItems;
			}
			set
			{
				this._allowNullItems = value;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002F84 File Offset: 0x00001184
		public JsonArrayAttribute()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002F8C File Offset: 0x0000118C
		public JsonArrayAttribute(bool allowNullItems)
		{
			this._allowNullItems = allowNullItems;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002F9B File Offset: 0x0000119B
		public JsonArrayAttribute(string id) : base(id)
		{
		}

		// Token: 0x0400000E RID: 14
		private bool _allowNullItems;
	}
}
