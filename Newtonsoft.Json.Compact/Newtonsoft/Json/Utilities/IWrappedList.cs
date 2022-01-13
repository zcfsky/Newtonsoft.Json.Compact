using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000074 RID: 116
	internal interface IWrappedList : IList, ICollection, IEnumerable
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060005EA RID: 1514
		object UnderlyingList { get; }
	}
}
