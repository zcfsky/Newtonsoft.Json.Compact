using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000064 RID: 100
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		// Token: 0x06000553 RID: 1363 RVA: 0x000131F8 File Offset: 0x000113F8
		public void Add(TFirst first, TSecond second)
		{
			if (this._firstToSecond.ContainsKey(first) || this._secondToFirst.ContainsKey(second))
			{
				throw new ArgumentException("Duplicate first or second");
			}
			this._firstToSecond.Add(first, second);
			this._secondToFirst.Add(second, first);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00013246 File Offset: 0x00011446
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return this._firstToSecond.TryGetValue(first, out second);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00013255 File Offset: 0x00011455
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return this._secondToFirst.TryGetValue(second, out first);
		}

		// Token: 0x040001AD RID: 429
		private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();

		// Token: 0x040001AE RID: 430
		private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();
	}
}
