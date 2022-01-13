using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000009 RID: 9
    [AttributeUsage((AttributeTargets)1028, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002EF3 File Offset: 0x000010F3
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00002EFB File Offset: 0x000010FB
		public string Id { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002F04 File Offset: 0x00001104
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002F0C File Offset: 0x0000110C
		public string Title { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002F15 File Offset: 0x00001115
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002F1D File Offset: 0x0000111D
		public string Description { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002F28 File Offset: 0x00001128
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002F4E File Offset: 0x0000114E
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002F5C File Offset: 0x0000115C
		protected JsonContainerAttribute()
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002F64 File Offset: 0x00001164
		protected JsonContainerAttribute(string id)
		{
			this.Id = id;
		}

		// Token: 0x0400000A RID: 10
		internal bool? _isReference;
	}
}
