using System;
using System.Collections;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000015 RID: 21
	public interface IJEnumerable<T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x17000045 RID: 69
		IJEnumerable<JToken> this[object key]
		{
			get;
		}
	}
}
