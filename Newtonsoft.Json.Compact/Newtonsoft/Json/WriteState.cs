using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000071 RID: 113
	public enum WriteState
	{
		// Token: 0x040001CE RID: 462
		Error,
		// Token: 0x040001CF RID: 463
		Closed,
		// Token: 0x040001D0 RID: 464
		Object,
		// Token: 0x040001D1 RID: 465
		Array,
		// Token: 0x040001D2 RID: 466
		Constructor,
		// Token: 0x040001D3 RID: 467
		Property,
		// Token: 0x040001D4 RID: 468
		Start
	}
}
