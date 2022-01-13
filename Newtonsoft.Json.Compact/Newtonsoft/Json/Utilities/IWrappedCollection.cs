using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000066 RID: 102
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000568 RID: 1384
		object UnderlyingCollection { get; }
	}
}
