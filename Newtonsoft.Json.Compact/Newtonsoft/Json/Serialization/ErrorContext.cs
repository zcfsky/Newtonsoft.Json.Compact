using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000049 RID: 73
	public class ErrorContext
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0000F127 File Offset: 0x0000D327
		internal ErrorContext(object originalObject, object member, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000F144 File Offset: 0x0000D344
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000F14C File Offset: 0x0000D34C
		public Exception Error { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0000F155 File Offset: 0x0000D355
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x0000F15D File Offset: 0x0000D35D
		public object OriginalObject { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000F166 File Offset: 0x0000D366
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0000F16E File Offset: 0x0000D36E
		public object Member { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000F177 File Offset: 0x0000D377
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000F17F File Offset: 0x0000D37F
		public bool Handled { get; set; }
	}
}
