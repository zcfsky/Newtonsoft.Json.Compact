using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006A RID: 106
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x00013CE2 File Offset: 0x00011EE2
		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._dictionary = dictionary;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00013CFC File Offset: 0x00011EFC
		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			this._genericDictionary = dictionary;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00013D16 File Offset: 0x00011F16
		public void Add(TKey key, TValue value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(key, value);
				return;
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00013D45 File Offset: 0x00011F45
		public bool ContainsKey(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey(key);
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00013D6D File Offset: 0x00011F6D
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Keys;
				}
				return Enumerable.ToList<TKey>(Enumerable.Cast<TKey>(this._dictionary.Keys));
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00013D98 File Offset: 0x00011F98
		public bool Remove(TKey key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(key);
			}
			if (this._dictionary.Contains(key))
			{
				this._dictionary.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00013DD8 File Offset: 0x00011FD8
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.TryGetValue(key, out value);
			}
			if (!this._dictionary.Contains(key))
			{
				value = default(TValue);
				return false;
			}
			value = (TValue)((object)this._dictionary[key]);
			return true;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00013E34 File Offset: 0x00012034
		public ICollection<TValue> Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Values;
				}
				return Enumerable.ToList<TValue>(Enumerable.Cast<TValue>(this._dictionary.Values));
			}
		}

		// Token: 0x1700011D RID: 285
		public TValue this[TKey key]
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary[key];
				}
				return (TValue)((object)this._dictionary[key]);
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary[key] = value;
					return;
				}
				this._dictionary[key] = value;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00013EBB File Offset: 0x000120BB
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add(item);
				return;
			}
			((IList)this._dictionary).Add(item);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00013EE9 File Offset: 0x000120E9
		public void Clear()
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Clear();
				return;
			}
			this._dictionary.Clear();
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00013F0A File Offset: 0x0001210A
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Contains(item);
			}
			return ((IList)this._dictionary).Contains(item);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00013F38 File Offset: 0x00012138
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo(array, arrayIndex);
				return;
			}
			foreach (object obj in this._dictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				array[arrayIndex++] = new KeyValuePair<TKey, TValue>((TKey)((object)dictionaryEntry.Key), (TValue)((object)dictionaryEntry.Value));
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00013FD0 File Offset: 0x000121D0
		public int Count
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.Count;
				}
				return this._dictionary.Count;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00013FF1 File Offset: 0x000121F1
		public bool IsReadOnly
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary.IsReadOnly;
				}
				return this._dictionary.IsReadOnly;
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00014014 File Offset: 0x00012214
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.Remove(item);
			}
			if (!this._dictionary.Contains(item.Key))
			{
				return true;
			}
			object obj = this._dictionary[item.Key];
			if (object.Equals(obj, item.Value))
			{
				this._dictionary.Remove(item.Key);
				return true;
			}
			return false;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000140B8 File Offset: 0x000122B8
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.GetEnumerator();
			}
			return Enumerable.Select<DictionaryEntry, KeyValuePair<TKey, TValue>>(Enumerable.Cast<DictionaryEntry>(this._dictionary), (DictionaryEntry de) => new KeyValuePair<TKey, TValue>((TKey)((object)de.Key), (TValue)((object)de.Value))).GetEnumerator();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001410B File Offset: 0x0001230B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00014113 File Offset: 0x00012313
		void IDictionary.Add(object key, object value)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Add((TKey)((object)key), (TValue)((object)value));
				return;
			}
			this._dictionary.Add(key, value);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014142 File Offset: 0x00012342
		bool IDictionary.Contains(object key)
		{
			if (this._genericDictionary != null)
			{
				return this._genericDictionary.ContainsKey((TKey)((object)key));
			}
			return this._dictionary.Contains(key);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001416A File Offset: 0x0001236A
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (this._genericDictionary != null)
			{
				return new DictionaryWrapper<TKey, TValue>.DictionaryEnumerator(this._genericDictionary.GetEnumerator());
			}
			return this._dictionary.GetEnumerator();
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00014195 File Offset: 0x00012395
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsFixedSize;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x000141AC File Offset: 0x000123AC
		ICollection IDictionary.Keys
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TKey>(this._genericDictionary.Keys);
				}
				return this._dictionary.Keys;
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000141D2 File Offset: 0x000123D2
		public void Remove(object key)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.Remove((TKey)((object)key));
				return;
			}
			this._dictionary.Remove(key);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x000141FB File Offset: 0x000123FB
		ICollection IDictionary.Values
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return Enumerable.ToList<TValue>(this._genericDictionary.Values);
				}
				return this._dictionary.Values;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00014221 File Offset: 0x00012421
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x0001424E File Offset: 0x0001244E
        object IDictionary.this[object key]
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary[(TKey)(key)];
				}
				return this._dictionary[key];
			}
			set
			{
				if (this._genericDictionary != null)
				{
					this._genericDictionary[(TKey)((object)key)] = (TValue)((object)value);
					return;
				}
				this._dictionary[key] = value;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001427D File Offset: 0x0001247D
		void ICollection.CopyTo(Array array, int index)
		{
			if (this._genericDictionary != null)
			{
				this._genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
				return;
			}
			this._dictionary.CopyTo(array, index);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000142A7 File Offset: 0x000124A7
		bool ICollection.IsSynchronized
		{
			get
			{
				return this._genericDictionary == null && this._dictionary.IsSynchronized;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x000142BE File Offset: 0x000124BE
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000142E0 File Offset: 0x000124E0
		public object UnderlyingDictionary
		{
			get
			{
				if (this._genericDictionary != null)
				{
					return this._genericDictionary;
				}
				return this._dictionary;
			}
		}

		// Token: 0x040001B2 RID: 434
		private readonly IDictionary _dictionary;

		// Token: 0x040001B3 RID: 435
		private readonly IDictionary<TKey, TValue> _genericDictionary;

		// Token: 0x040001B4 RID: 436
		private object _syncRoot;

		// Token: 0x0200006B RID: 107
		private struct DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060005A7 RID: 1447 RVA: 0x000142F7 File Offset: 0x000124F7
			public DictionaryEnumerator(IEnumerator e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				this._e = e;
			}

			// Token: 0x17000127 RID: 295
			// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001430B File Offset: 0x0001250B
			public DictionaryEntry Entry
			{
				get
				{
					return (DictionaryEntry)this.Current;
				}
			}

			// Token: 0x17000128 RID: 296
			// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00014318 File Offset: 0x00012518
			public object Key
			{
				get
				{
					return this.Entry.Key;
				}
			}

			// Token: 0x17000129 RID: 297
			// (get) Token: 0x060005AA RID: 1450 RVA: 0x00014334 File Offset: 0x00012534
			public object Value
			{
				get
				{
					return this.Entry.Value;
				}
			}

			// Token: 0x1700012A RID: 298
			// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001434F File Offset: 0x0001254F
			public object Current
			{
				get
				{
					return this._e.Current;
				}
			}

			// Token: 0x060005AC RID: 1452 RVA: 0x0001435C File Offset: 0x0001255C
			public bool MoveNext()
			{
				return this._e.MoveNext();
			}

			// Token: 0x060005AD RID: 1453 RVA: 0x00014369 File Offset: 0x00012569
			public void Reset()
			{
				this._e.Reset();
			}

			// Token: 0x040001B6 RID: 438
			private IEnumerator _e;
		}
	}
}
