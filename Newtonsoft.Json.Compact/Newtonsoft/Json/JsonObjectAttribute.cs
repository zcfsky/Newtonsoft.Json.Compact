using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200000D RID: 13
    [AttributeUsage((AttributeTargets)1028, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000301C File Offset: 0x0000121C
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003024 File Offset: 0x00001224
		public MemberSerialization MemberSerialization
		{
			get
			{
				return this._memberSerialization;
			}
			set
			{
				this._memberSerialization = value;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000302D File Offset: 0x0000122D
		public JsonObjectAttribute()
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003035 File Offset: 0x00001235
		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			this.MemberSerialization = memberSerialization;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003044 File Offset: 0x00001244
		public JsonObjectAttribute(string id) : base(id)
		{
		}

		// Token: 0x04000013 RID: 19
		private MemberSerialization _memberSerialization;
	}
}
