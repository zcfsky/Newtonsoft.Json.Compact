using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200004A RID: 74
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000F188 File Offset: 0x0000D388
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000F190 File Offset: 0x0000D390
		public object CurrentObject { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000F199 File Offset: 0x0000D399
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000F1A1 File Offset: 0x0000D3A1
		public ErrorContext ErrorContext { get; private set; }

		// Token: 0x06000438 RID: 1080 RVA: 0x0000F1AA File Offset: 0x0000D3AA
		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
