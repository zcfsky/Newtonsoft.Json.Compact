using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007D RID: 125
	internal class ThreadSafeStore<TKey, TValue>
	{
		// Token: 0x06000668 RID: 1640 RVA: 0x00017291 File Offset: 0x00015491
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			this._creator = creator;
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x000172BC File Offset: 0x000154BC
		public TValue Get(TKey key)
		{
			if (this._store == null)
			{
				return this.AddValue(key);
			}
			TValue result;
			if (!this._store.TryGetValue(key, out result))
			{
				return this.AddValue(key);
			}
			return result;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000172F4 File Offset: 0x000154F4
		private TValue AddValue(TKey key)
		{
			TValue tvalue = this._creator.Invoke(key);
			TValue result2;
			lock (this._lock)
			{
				if (this._store == null)
				{
					this._store = new Dictionary<TKey, TValue>();
					this._store[key] = tvalue;
				}
				else
				{
					TValue result;
					if (this._store.TryGetValue(key, out result))
					{
						return result;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._store);
					dictionary[key] = tvalue;
					this._store = dictionary;
				}
				result2 = tvalue;
			}
			return result2;
		}

		// Token: 0x040001E6 RID: 486
		private readonly object _lock = new object();

		// Token: 0x040001E7 RID: 487
		private Dictionary<TKey, TValue> _store;

		// Token: 0x040001E8 RID: 488
		private readonly Func<TKey, TValue> _creator;
	}
}
