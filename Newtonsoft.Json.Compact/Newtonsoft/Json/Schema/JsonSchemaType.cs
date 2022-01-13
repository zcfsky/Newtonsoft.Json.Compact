using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000057 RID: 87
	[Flags]
	public enum JsonSchemaType
	{
		// Token: 0x0400016E RID: 366
		None = 0,
		// Token: 0x0400016F RID: 367
		String = 1,
		// Token: 0x04000170 RID: 368
		Float = 2,
		// Token: 0x04000171 RID: 369
		Integer = 4,
		// Token: 0x04000172 RID: 370
		Boolean = 8,
		// Token: 0x04000173 RID: 371
		Object = 16,
		// Token: 0x04000174 RID: 372
		Array = 32,
		// Token: 0x04000175 RID: 373
		Null = 64,
		// Token: 0x04000176 RID: 374
		Any = 127
	}
}
