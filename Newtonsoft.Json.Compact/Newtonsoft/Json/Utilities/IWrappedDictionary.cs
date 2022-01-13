using System;
using System.Collections;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000069 RID: 105
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000585 RID: 1413
		object UnderlyingDictionary { get; }
	}
}
