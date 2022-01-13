using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000061 RID: 97
	public enum StreamingContextStates
	{
		// Token: 0x0400019D RID: 413
		All = 255,
		// Token: 0x0400019E RID: 414
		Clone = 64,
		// Token: 0x0400019F RID: 415
		CrossAppDomain = 128,
		// Token: 0x040001A0 RID: 416
		CrossMachine = 2,
		// Token: 0x040001A1 RID: 417
		CrossProcess = 1,
		// Token: 0x040001A2 RID: 418
		File = 4,
		// Token: 0x040001A3 RID: 419
		Other = 32,
		// Token: 0x040001A4 RID: 420
		Persistence = 8,
		// Token: 0x040001A5 RID: 421
		Remoting = 16
	}
}
