using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json.Linq.ComponentModel;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200002F RID: 47
	public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, ITypedList, IBindingList, IList, ICollection, IEnumerable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002CD RID: 717 RVA: 0x0000AF3E File Offset: 0x0000913E
		// (remove) Token: 0x060002CE RID: 718 RVA: 0x0000AF57 File Offset: 0x00009157
		public event ListChangedEventHandler ListChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002CF RID: 719 RVA: 0x0000AF70 File Offset: 0x00009170
		// (remove) Token: 0x060002D0 RID: 720 RVA: 0x0000AF89 File Offset: 0x00009189
		public event AddingNewEventHandler AddingNew;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000AFA2 File Offset: 0x000091A2
		// (set) Token: 0x060002D2 RID: 722 RVA: 0x0000AFAA File Offset: 0x000091AA
		internal JToken Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000AFB3 File Offset: 0x000091B3
		internal JContainer()
		{
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000AFBC File Offset: 0x000091BC
		internal JContainer(JContainer other)
		{
			ValidationUtils.ArgumentNotNull(other, "c");
			JToken jtoken = other.Last;
			if (jtoken != null)
			{
				do
				{
					jtoken = jtoken._next;
					this.Add(jtoken.CloneToken());
				}
				while (jtoken != other.Last);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B000 File Offset: 0x00009200
		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("ObservableCollection_CannotChangeObservableCollection");
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000B018 File Offset: 0x00009218
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNew = this.AddingNew;
			if (addingNew != null)
			{
				addingNew.Invoke(this, e);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000B038 File Offset: 0x00009238
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			ListChangedEventHandler listChanged = this.ListChanged;
			if (listChanged != null)
			{
				this._busy = true;
				try
				{
					listChanged.Invoke(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000B078 File Offset: 0x00009278
		public override bool HasValues
		{
			get
			{
				return this._content != null;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B088 File Offset: 0x00009288
		internal bool ContentsEqual(JContainer container)
		{
			JToken jtoken = this.First;
			JToken jtoken2 = container.First;
			if (jtoken == jtoken2)
			{
				return true;
			}
			while (jtoken != null || jtoken2 != null)
			{
				if (jtoken == null || jtoken2 == null || !jtoken.DeepEquals(jtoken2))
				{
					return false;
				}
				jtoken = ((jtoken != this.Last) ? jtoken.Next : null);
				jtoken2 = ((jtoken2 != container.Last) ? jtoken2.Next : null);
			}
			return true;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000B0E9 File Offset: 0x000092E9
		public override JToken First
		{
			get
			{
				if (this.Last == null)
				{
					return null;
				}
				return this.Last._next;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000B100 File Offset: 0x00009300
		public override JToken Last
		{
			[DebuggerStepThrough]
			get
			{
				return this._content;
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B108 File Offset: 0x00009308
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenInternal());
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000B22C File Offset: 0x0000942C
		private IEnumerable<JToken> ChildrenInternal()
		{
			JToken first = this.First;
			JToken current = first;
			if (current != null)
			{
				do
				{
					yield return current;
				}
				while (current != null && (current = current.Next) != null);
			}
			yield break;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000B249 File Offset: 0x00009449
		public override IEnumerable<T> Values<T>()
		{
			return this.Children().Convert<JToken, T>();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B4DC File Offset: 0x000096DC
		public IEnumerable<JToken> Descendants()
		{
			foreach (JToken o in this.Children())
			{
				yield return o;
				JContainer c = o as JContainer;
				if (c != null)
				{
					foreach (JToken d in c.Descendants())
					{
						yield return d;
					}
				}
			}
			yield break;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B4F9 File Offset: 0x000096F9
		internal bool IsMultiContent(object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B51C File Offset: 0x0000971C
		internal virtual void AddItem(bool isLast, JToken previous, JToken item)
		{
			this.CheckReentrancy();
			this.ValidateToken(item, null);
			item = this.EnsureParentToken(item);
			JToken next = (previous != null) ? previous._next : item;
			item.Parent = this;
			item.Next = next;
			if (previous != null)
			{
				previous.Next = item;
			}
			if (isLast || previous == null)
			{
				this._content = item;
			}
			if (this.ListChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs((ListChangedType)1, this.IndexOfItem(item)));
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000B590 File Offset: 0x00009790
		internal JToken EnsureParentToken(JToken item)
		{
			if (item.Parent != null)
			{
				item = item.CloneToken();
			}
			else
			{
				JContainer jcontainer = this;
				while (jcontainer.Parent != null)
				{
					jcontainer = jcontainer.Parent;
				}
				if (item == jcontainer)
				{
					item = item.CloneToken();
				}
			}
			return item;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B5D0 File Offset: 0x000097D0
		internal void AddInternal(bool isLast, JToken previous, object content)
		{
            if (this.IsMultiContent(content))
            {
                IEnumerable enumerable = (IEnumerable)content;
                JToken jtoken = previous;
                IEnumerator enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object content2 = enumerator.Current;
                    this.AddInternal(isLast, jtoken, content2);
                    jtoken = ((jtoken != null) ? jtoken._next : this.Last);
                }
            }
            else
            {
                JToken item = this.CreateFromContent(content);
                this.AddItem(isLast, previous, item);
            }
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000B660 File Offset: 0x00009860
		internal int IndexOfItem(JToken item)
		{
			int num = 0;
			foreach (JToken jtoken in this.Children())
			{
				if (jtoken == item)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000B6C0 File Offset: 0x000098C0
		internal virtual void InsertItem(int index, JToken item)
		{
			if (index == 0)
			{
				this.AddFirst(item);
				return;
			}
			JToken item2 = this.GetItem(index);
			this.AddInternal(false, item2.Previous, item);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B6F0 File Offset: 0x000098F0
		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "index is less than 0.");
			}
			this.CheckReentrancy();
			int num = 0;
			foreach (JToken jtoken in this.Children())
			{
				if (index == num)
				{
					jtoken.Remove();
					this.OnListChanged(new ListChangedEventArgs((ListChangedType)2, index));
					return;
				}
				num++;
			}
			throw new ArgumentOutOfRangeException("index", "index is equal to or greater than Count.");
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B784 File Offset: 0x00009984
		internal virtual bool RemoveItem(JToken item)
		{
			if (item == null || item.Parent != this)
			{
				return false;
			}
			this.CheckReentrancy();
			JToken jtoken = this._content;
			int num = 0;
			while (jtoken._next != item)
			{
				num++;
				jtoken = jtoken._next;
			}
			if (jtoken == item)
			{
				this._content = null;
			}
			else
			{
				if (this._content == item)
				{
					this._content = jtoken;
				}
				jtoken._next = item._next;
			}
			item.Parent = null;
			item.Next = null;
			this.OnListChanged(new ListChangedEventArgs((ListChangedType)2, num));
			return true;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B809 File Offset: 0x00009A09
		internal virtual JToken GetItem(int index)
		{
			return Enumerable.ElementAt<JToken>(this.Children(), index);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B81C File Offset: 0x00009A1C
		internal virtual void SetItem(int index, JToken item)
		{
			this.CheckReentrancy();
			JToken item2 = this.GetItem(index);
			item2.Replace(item);
			this.OnListChanged(new ListChangedEventArgs((ListChangedType)4, index));
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B84C File Offset: 0x00009A4C
		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			while (this._content != null)
			{
				JToken content = this._content;
				JToken next = content._next;
				if (content != this._content || next != content._next)
				{
					throw new InvalidOperationException("This operation was corrupted by external code.");
				}
				if (next != content)
				{
					content._next = next._next;
				}
				else
				{
					this._content = null;
				}
				next.Parent = null;
				next._next = null;
			}
			this.OnListChanged(new ListChangedEventArgs(0, -1));
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing == null || existing.Parent != this)
			{
				return;
			}
			if (JContainer.IsTokenUnchanged(existing, replacement))
			{
				return;
			}
			this.CheckReentrancy();
			replacement = this.EnsureParentToken(replacement);
			this.ValidateToken(replacement, existing);
			JToken jtoken = this._content;
			int num = 0;
			while (jtoken._next != existing)
			{
				num++;
				jtoken = jtoken._next;
			}
			if (jtoken == existing)
			{
				this._content = replacement;
				replacement._next = replacement;
			}
			else
			{
				if (this._content == existing)
				{
					this._content = replacement;
				}
				jtoken._next = replacement;
				replacement._next = existing._next;
			}
			replacement.Parent = this;
			existing.Parent = null;
			existing.Next = null;
			this.OnListChanged(new ListChangedEventArgs((ListChangedType)4, num));
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B97B File Offset: 0x00009B7B
		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B98C File Offset: 0x00009B8C
		internal virtual void CopyItemsTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (this.CountItems() > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this.Children())
			{
				array.SetValue(jtoken, arrayIndex + num);
				num++;
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000BA38 File Offset: 0x00009C38
		internal virtual int CountItems()
		{
			return Enumerable.Count<JToken>(this.Children());
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000BA4C File Offset: 0x00009C4C
		internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
		{
			JValue jvalue = currentValue as JValue;
			return jvalue != null && ((jvalue.Type == JTokenType.Null && newValue == null) || jvalue.Equals(newValue));
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000BA7C File Offset: 0x00009C7C
		internal virtual void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000BACC File Offset: 0x00009CCC
		public void Add(object content)
		{
			this.AddInternal(true, this.Last, content);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000BADC File Offset: 0x00009CDC
		public void AddFirst(object content)
		{
			this.AddInternal(false, this.Last, content);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BAEC File Offset: 0x00009CEC
		internal JToken CreateFromContent(object content)
		{
			if (content is JToken)
			{
				return (JToken)content;
			}
			return new JValue(content);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BB03 File Offset: 0x00009D03
		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BB0B File Offset: 0x00009D0B
		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BB1A File Offset: 0x00009D1A
		public void RemoveAll()
		{
			this.ClearItems();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BB24 File Offset: 0x00009D24
		internal void ReadContentFrom(JsonReader r)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jcontainer = this;
			for (;;)
			{
				if (jcontainer is JProperty && ((JProperty)jcontainer).Value != null)
				{
					if (jcontainer == this)
					{
						break;
					}
					jcontainer = jcontainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_220;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(lineInfo);
					jcontainer.Add(jobject);
					jcontainer = jobject;
					goto IL_220;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(lineInfo);
					jcontainer.Add(jarray);
					jcontainer = jarray;
					goto IL_220;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(r.Value.ToString());
					jconstructor.SetLineInfo(jconstructor);
					jcontainer.Add(jconstructor);
					jcontainer = jconstructor;
					goto IL_220;
				}
				case JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					JProperty jproperty = new JProperty(name);
					jproperty.SetLineInfo(lineInfo);
					JObject jobject2 = (JObject)jcontainer;
					JProperty jproperty2 = jobject2.Property(name);
					if (jproperty2 == null)
					{
						jcontainer.Add(jproperty);
					}
					else
					{
						jproperty2.Replace(jproperty);
					}
					jcontainer = jproperty;
					goto IL_220;
				}
				case JsonToken.Comment:
				{
					JValue jvalue = JValue.CreateComment(r.Value.ToString());
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_220;
				}
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				{
					JValue jvalue = new JValue(r.Value);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_220;
				}
				case JsonToken.Null:
				{
					JValue jvalue = new JValue(null, JTokenType.Null);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_220;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = new JValue(null, JTokenType.Undefined);
					jvalue.SetLineInfo(lineInfo);
					jcontainer.Add(jvalue);
					goto IL_220;
				}
				case JsonToken.EndObject:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_220;
				case JsonToken.EndArray:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_220;
				case JsonToken.EndConstructor:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_220;
				}
				goto Block_4;
				IL_220:
				if (!r.Read())
				{
					return;
				}
			}
			return;
			Block_4:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				r.TokenType
			}));
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BD5C File Offset: 0x00009F5C
		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken jtoken in this.Children())
			{
				num ^= jtoken.GetDeepHashCode();
			}
			return num;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BDBC File Offset: 0x00009FBC
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			JObject jobject = this.First as JObject;
			if (jobject != null)
			{
				JTypeDescriptor jtypeDescriptor = new JTypeDescriptor(jobject);
				return jtypeDescriptor.GetProperties();
			}
			return null;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000BDE7 File Offset: 0x00009FE7
		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000BDFA File Offset: 0x00009FFA
		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000BE03 File Offset: 0x0000A003
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000BE0C File Offset: 0x0000A00C
		JToken IList<JToken>.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000BE16 File Offset: 0x0000A016
		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000BE1F File Offset: 0x0000A01F
		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000BE27 File Offset: 0x0000A027
		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BE30 File Offset: 0x0000A030
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000BE3A File Offset: 0x0000A03A
		int ICollection<JToken>.Count
		{
			get
			{
				return this.CountItems();
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000BE42 File Offset: 0x0000A042
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000BE45 File Offset: 0x0000A045
		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000BE4E File Offset: 0x0000A04E
		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is JToken)
			{
				return (JToken)value;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000BE6E File Offset: 0x0000A06E
		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.CountItems() - 1;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000BE85 File Offset: 0x0000A085
		void IList.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000BE8D File Offset: 0x0000A08D
		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000BE9C File Offset: 0x0000A09C
		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000BEAB File Offset: 0x0000A0AB
		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value));
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000BEBB File Offset: 0x0000A0BB
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000BEC1 File Offset: 0x0000A0C1
		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000BED1 File Offset: 0x0000A0D1
		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000BEDA File Offset: 0x0000A0DA
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000BEE3 File Offset: 0x0000A0E3
		object IList.this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000BEFD File Offset: 0x0000A0FD
		int ICollection.Count
		{
			get
			{
				return this.CountItems();
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000BF05 File Offset: 0x0000A105
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000BF08 File Offset: 0x0000A108
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

		// Token: 0x06000317 RID: 791 RVA: 0x0000BF2A File Offset: 0x0000A12A
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000BF2C File Offset: 0x0000A12C
		object IBindingList.AddNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			this.OnAddingNew(addingNewEventArgs);
			if (addingNewEventArgs.NewObject == null)
			{
				throw new Exception("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
			if (!(addingNewEventArgs.NewObject is JToken))
			{
				throw new Exception("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeof(JToken)
				}));
			}
			JToken jtoken = (JToken)addingNewEventArgs.NewObject;
			this.Add(jtoken);
			return jtoken;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000BFBF File Offset: 0x0000A1BF
		bool IBindingList.AllowEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000BFC2 File Offset: 0x0000A1C2
		bool IBindingList.AllowNew
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000BFC5 File Offset: 0x0000A1C5
		bool IBindingList.AllowRemove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000BFC8 File Offset: 0x0000A1C8
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000BFCF File Offset: 0x0000A1CF
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000BFD6 File Offset: 0x0000A1D6
		bool IBindingList.IsSorted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000BFD9 File Offset: 0x0000A1D9
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000BFDB File Offset: 0x0000A1DB
		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000BFE2 File Offset: 0x0000A1E2
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000BFE5 File Offset: 0x0000A1E5
		PropertyDescriptor IBindingList.SortProperty
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000BFEB File Offset: 0x0000A1EB
		bool IBindingList.SupportsSearching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000BFEE File Offset: 0x0000A1EE
		bool IBindingList.SupportsSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000C8 RID: 200
		private JToken _content;

		// Token: 0x040000C9 RID: 201
		private object _syncRoot;

		// Token: 0x040000CA RID: 202
		private bool _busy;
	}
}
