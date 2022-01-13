using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000075 RID: 117
	internal class ListWrapper<T> : IList<T>, ICollection<T>, IEnumerable<T>, IWrappedList, IList, ICollection, IEnumerable
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x0001583C File Offset: 0x00013A3C
		public ListWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is IList<T>)
			{
				this._genericList = (IList<T>)list;
				return;
			}
			this._list = list;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001586B File Offset: 0x00013A6B
		public ListWrapper(IList<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericList = list;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00015885 File Offset: 0x00013A85
		public int IndexOf(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.IndexOf(item);
			}
			return this._list.IndexOf(item);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000158AD File Offset: 0x00013AAD
		public void Insert(int index, T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Insert(index, item);
				return;
			}
			this._list.Insert(index, item);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000158D7 File Offset: 0x00013AD7
		public void RemoveAt(int index)
		{
			if (this._genericList != null)
			{
				this._genericList.RemoveAt(index);
				return;
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x1700012E RID: 302
		public T this[int index]
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList[index];
				}
				return (T)((object)this._list[index]);
			}
			set
			{
				if (this._genericList != null)
				{
					this._genericList[index] = value;
					return;
				}
				this._list[index] = value;
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001594C File Offset: 0x00013B4C
		public void Add(T item)
		{
			if (this._genericList != null)
			{
				this._genericList.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00015975 File Offset: 0x00013B75
		public void Clear()
		{
			if (this._genericList != null)
			{
				this._genericList.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00015996 File Offset: 0x00013B96
		public bool Contains(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000159BE File Offset: 0x00013BBE
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericList != null)
			{
				this._genericList.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x000159E3 File Offset: 0x00013BE3
		public int Count
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00015A04 File Offset: 0x00013C04
		public bool IsReadOnly
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00015A28 File Offset: 0x00013C28
		public bool Remove(T item)
		{
			if (this._genericList != null)
			{
				return this._genericList.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00015A71 File Offset: 0x00013C71
		public IEnumerator<T> GetEnumerator()
		{
			if (this._genericList != null)
			{
				return this._genericList.GetEnumerator();
			}
			return Enumerable.Cast<T>(this._list).GetEnumerator();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00015A97 File Offset: 0x00013C97
		IEnumerator IEnumerable.GetEnumerator()
		{
			if (this._genericList != null)
			{
				return this._genericList.GetEnumerator();
			}
			return this._list.GetEnumerator();
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00015AB8 File Offset: 0x00013CB8
		int IList.Add(object value)
		{
			ListWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00015AD4 File Offset: 0x00013CD4
		bool IList.Contains(object value)
		{
			return ListWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00015AEC File Offset: 0x00013CEC
		int IList.IndexOf(object value)
		{
			if (ListWrapper<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00015B04 File Offset: 0x00013D04
		void IList.Insert(int index, object value)
		{
			ListWrapper<T>.VerifyValueType(value);
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x00015B19 File Offset: 0x00013D19
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00015B1C File Offset: 0x00013D1C
		void IList.Remove(object value)
		{
			if (ListWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00015B33 File Offset: 0x00013D33
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00015B41 File Offset: 0x00013D41
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				ListWrapper<T>.VerifyValueType(value);
				this[index] = (T)value;
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00015B56 File Offset: 0x00013D56
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00015B65 File Offset: 0x00013D65
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00015B68 File Offset: 0x00013D68
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

		// Token: 0x06000606 RID: 1542 RVA: 0x00015B8C File Offset: 0x00013D8C
		private static void VerifyValueType(object value)
		{
			if (!ListWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					value,
					typeof(T)
				}), "value");
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00015BD4 File Offset: 0x00013DD4
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x00015C06 File Offset: 0x00013E06
		public object UnderlyingList
		{
			get
			{
				if (this._genericList != null)
				{
					return this._genericList;
				}
				return this._list;
			}
		}

		// Token: 0x040001D8 RID: 472
		private readonly IList _list;

		// Token: 0x040001D9 RID: 473
		private readonly IList<T> _genericList;

		// Token: 0x040001DA RID: 474
		private object _syncRoot;
	}
}
