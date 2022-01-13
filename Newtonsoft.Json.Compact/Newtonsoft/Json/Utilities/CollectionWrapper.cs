using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000067 RID: 103
	internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IWrappedCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x000138A5 File Offset: 0x00011AA5
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is ICollection<T>)
			{
				this._genericCollection = (ICollection<T>)list;
				return;
			}
			this._list = list;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000138D4 File Offset: 0x00011AD4
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000138EE File Offset: 0x00011AEE
		public void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013917 File Offset: 0x00011B17
		public void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013938 File Offset: 0x00011B38
		public bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00013960 File Offset: 0x00011B60
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00013985 File Offset: 0x00011B85
		public int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x000139A6 File Offset: 0x00011BA6
		public bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x000139C8 File Offset: 0x00011BC8
		public bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013A11 File Offset: 0x00011C11
		public IEnumerator<T> GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return Enumerable.Cast<T>(this._list).GetEnumerator();
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013A37 File Offset: 0x00011C37
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.GetEnumerator();
			}
			return this._list.GetEnumerator();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00013A58 File Offset: 0x00011C58
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013A74 File Offset: 0x00011C74
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00013A8C File Offset: 0x00011C8C
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013AC1 File Offset: 0x00011CC1
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013AE2 File Offset: 0x00011CE2
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new Exception("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00013B14 File Offset: 0x00011D14
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00013B17 File Offset: 0x00011D17
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00013B2E File Offset: 0x00011D2E
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00013B4F File Offset: 0x00011D4F
		object IList.this[int index]
		{
			get
			{
				if (this._genericCollection != null)
				{
					throw new Exception("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			set
			{
				if (this._genericCollection != null)
				{
					throw new Exception("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)value;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00013B81 File Offset: 0x00011D81
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00013B90 File Offset: 0x00011D90
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00013B93 File Offset: 0x00011D93
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

		// Token: 0x06000580 RID: 1408 RVA: 0x00013BB8 File Offset: 0x00011DB8
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					value,
					typeof(T)
				}), "value");
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00013C00 File Offset: 0x00011E00
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00013C32 File Offset: 0x00011E32
		public object UnderlyingCollection
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection;
				}
				return this._list;
			}
		}

		// Token: 0x040001AF RID: 431
		private readonly IList _list;

		// Token: 0x040001B0 RID: 432
		private readonly ICollection<T> _genericCollection;

		// Token: 0x040001B1 RID: 433
		private object _syncRoot;
	}
}
