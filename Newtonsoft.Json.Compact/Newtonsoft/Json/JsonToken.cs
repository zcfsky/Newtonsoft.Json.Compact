using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000070 RID: 112
	public enum JsonToken
	{
		// Token: 0x040001BC RID: 444
		None,
		// Token: 0x040001BD RID: 445
		StartObject,
		// Token: 0x040001BE RID: 446
		StartArray,
		// Token: 0x040001BF RID: 447
		StartConstructor,
		// Token: 0x040001C0 RID: 448
		PropertyName,
		// Token: 0x040001C1 RID: 449
		Comment,
		// Token: 0x040001C2 RID: 450
		Raw,
		// Token: 0x040001C3 RID: 451
		Integer,
		// Token: 0x040001C4 RID: 452
		Float,
		// Token: 0x040001C5 RID: 453
		String,
		// Token: 0x040001C6 RID: 454
		Boolean,
		// Token: 0x040001C7 RID: 455
		Null,
		// Token: 0x040001C8 RID: 456
		Undefined,
		// Token: 0x040001C9 RID: 457
		EndObject,
		// Token: 0x040001CA RID: 458
		EndArray,
		// Token: 0x040001CB RID: 459
		EndConstructor,
		// Token: 0x040001CC RID: 460
		Date
	}
}
