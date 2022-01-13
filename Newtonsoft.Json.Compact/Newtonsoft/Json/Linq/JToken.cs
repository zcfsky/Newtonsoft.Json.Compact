using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200002E RID: 46
	public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009CAF File Offset: 0x00007EAF
		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (JToken._equalityComparer == null)
				{
					JToken._equalityComparer = new JTokenEqualityComparer();
				}
				return JToken._equalityComparer;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00009CC7 File Offset: 0x00007EC7
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00009CCF File Offset: 0x00007ECF
		public JContainer Parent
		{
			[DebuggerStepThrough]
			get
			{
				return this._parent;
			}
			internal set
			{
				this._parent = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00009CD8 File Offset: 0x00007ED8
		public JToken Root
		{
			get
			{
				JContainer parent = this.Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		// Token: 0x06000270 RID: 624
		internal abstract JToken CloneToken();

		// Token: 0x06000271 RID: 625
		internal abstract bool DeepEquals(JToken node);

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000272 RID: 626
		public abstract JTokenType Type { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000273 RID: 627
		public abstract bool HasValues { get; }

		// Token: 0x06000274 RID: 628 RVA: 0x00009D01 File Offset: 0x00007F01
		public static bool DeepEquals(JToken t1, JToken t2)
		{
			return t1 == t2 || (t1 != null && t2 != null && t1.DeepEquals(t2));
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00009D18 File Offset: 0x00007F18
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00009D3D File Offset: 0x00007F3D
		public JToken Next
		{
			get
			{
				if (this._parent != null && this._next != this._parent.First)
				{
					return this._next;
				}
				return null;
			}
			internal set
			{
				this._next = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000277 RID: 631 RVA: 0x00009D48 File Offset: 0x00007F48
		public JToken Previous
		{
			get
			{
				if (this._parent == null)
				{
					return null;
				}
				JToken next = this._parent.Content._next;
				JToken result = null;
				while (next != this)
				{
					result = next;
					next = next.Next;
				}
				return result;
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009D82 File Offset: 0x00007F82
		internal JToken()
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009D8A File Offset: 0x00007F8A
		public void AddAfterSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.AddInternal(this.Next == null, this, content);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009DB8 File Offset: 0x00007FB8
		public void AddBeforeSelf(object content)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			JToken jtoken = this.Previous;
			if (jtoken == null)
			{
				jtoken = this._parent.Last;
			}
			this._parent.AddInternal(false, jtoken, content);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009EF8 File Offset: 0x000080F8
		public IEnumerable<JToken> Ancestors()
		{
			for (JToken parent = this.Parent; parent != null; parent = parent.Parent)
			{
				yield return parent;
			}
			yield break;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A024 File Offset: 0x00008224
		public IEnumerable<JToken> AfterSelf()
		{
			if (this.Parent != null)
			{
				for (JToken o = this.Next; o != null; o = o.Next)
				{
					yield return o;
				}
			}
			yield break;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A14C File Offset: 0x0000834C
		public IEnumerable<JToken> BeforeSelf()
		{
			for (JToken o = this.Parent.First; o != this; o = o.Next)
			{
				yield return o;
			}
			yield break;
		}

		// Token: 0x17000073 RID: 115
		public virtual JToken this[object key]
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A1D4 File Offset: 0x000083D4
		public virtual T Value<T>(object key)
		{
			JToken token = this[key];
			return token.Convert<JToken, T>();
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000A1F0 File Offset: 0x000083F0
		public virtual JToken First
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000A224 File Offset: 0x00008424
		public virtual JToken Last
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A258 File Offset: 0x00008458
		public virtual JEnumerable<JToken> Children()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				base.GetType()
			}));
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A28A File Offset: 0x0000848A
		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(Enumerable.OfType<T>(this.Children()));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A2A4 File Offset: 0x000084A4
		public virtual IEnumerable<T> Values<T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				base.GetType()
			}));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000A2D6 File Offset: 0x000084D6
		public void Remove()
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.RemoveItem(this);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public void Replace(JToken value)
		{
			if (this._parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			this._parent.ReplaceItem(this, value);
		}

		// Token: 0x06000288 RID: 648
		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		// Token: 0x06000289 RID: 649 RVA: 0x0000A31A File Offset: 0x0000851A
		public override string ToString()
		{
			return this.ToString(Formatting.Indented, new JsonConverter[0]);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A32C File Offset: 0x0000852C
		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				this.WriteTo(new JsonTextWriter(stringWriter)
				{
					Formatting = formatting
				}, converters);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A380 File Offset: 0x00008580
		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value is JProperty)
			{
				value = ((JProperty)value).Value;
			}
			return value as JValue;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A3B8 File Offset: 0x000085B8
		private static bool IsNullable(JToken o)
		{
			return o.Type == JTokenType.Undefined || o.Type == JTokenType.Null;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A3D0 File Offset: 0x000085D0
		private static bool ValidateFloat(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Float || o.Type == JTokenType.Integer || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A3F1 File Offset: 0x000085F1
		private static bool ValidateInteger(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Integer || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A409 File Offset: 0x00008609
		private static bool ValidateDate(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Date || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A422 File Offset: 0x00008622
		private static bool ValidateBoolean(JToken o, bool nullable)
		{
			return o.Type == JTokenType.Boolean || (nullable && JToken.IsNullable(o));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A43B File Offset: 0x0000863B
		private static bool ValidateString(JToken o)
		{
			return o.Type == JTokenType.String || o.Type == JTokenType.Comment || o.Type == JTokenType.Raw || JToken.IsNullable(o);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A461 File Offset: 0x00008661
		private static string GetType(JToken t)
		{
			if (t == null)
			{
				return "{null}";
			}
			return t.Type.ToString();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A47C File Offset: 0x0000867C
		public static explicit operator bool(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateBoolean(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (bool)jvalue.Value;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A4D0 File Offset: 0x000086D0
		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (DateTimeOffset)jvalue.Value;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000A524 File Offset: 0x00008724
		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return default(bool?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateBoolean(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (bool?)jvalue.Value;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A584 File Offset: 0x00008784
		public static explicit operator long(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (long)jvalue.Value;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A5D8 File Offset: 0x000087D8
		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return default(DateTime?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (DateTime?)jvalue.Value;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A638 File Offset: 0x00008838
		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return default(DateTimeOffset?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (DateTimeOffset?)jvalue.Value;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A698 File Offset: 0x00008898
		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return default(decimal?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(decimal?);
			}
			return new decimal?(Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A714 File Offset: 0x00008914
		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return default(double?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (double?)jvalue.Value;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A774 File Offset: 0x00008974
		public static explicit operator int(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A7CC File Offset: 0x000089CC
		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return default(int?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(int?);
			}
			return new int?(Convert.ToInt32(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A848 File Offset: 0x00008A48
		public static explicit operator DateTime(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateDate(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (DateTime)jvalue.Value;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A89C File Offset: 0x00008A9C
		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return default(long?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (long?)jvalue.Value;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A8FC File Offset: 0x00008AFC
		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return default(float?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			if (jvalue.Value == null)
			{
				return default(float?);
			}
			return new float?(Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture));
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A978 File Offset: 0x00008B78
		public static explicit operator decimal(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return Convert.ToDecimal(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return default(uint?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (uint?)jvalue.Value;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000AA30 File Offset: 0x00008C30
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return default(ulong?);
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (ulong?)jvalue.Value;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AA90 File Offset: 0x00008C90
		public static explicit operator double(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (double)jvalue.Value;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000AAE4 File Offset: 0x00008CE4
		public static explicit operator float(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateFloat(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return Convert.ToSingle(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AB3C File Offset: 0x00008D3C
		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateString(jvalue))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return (string)jvalue.Value;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000AB94 File Offset: 0x00008D94
		public static explicit operator uint(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return Convert.ToUInt32(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000ABEC File Offset: 0x00008DEC
		public static explicit operator ulong(JToken value)
		{
			JValue jvalue = JToken.EnsureValue(value);
			if (jvalue == null || !JToken.ValidateInteger(jvalue, false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					JToken.GetType(jvalue)
				}));
			}
			return Convert.ToUInt64(jvalue.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AC42 File Offset: 0x00008E42
		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000AC4A File Offset: 0x00008E4A
		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000AC57 File Offset: 0x00008E57
		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000AC64 File Offset: 0x00008E64
		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000AC6C File Offset: 0x00008E6C
		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AC79 File Offset: 0x00008E79
		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AC86 File Offset: 0x00008E86
		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AC93 File Offset: 0x00008E93
		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000ACA0 File Offset: 0x00008EA0
		public static implicit operator JToken(ushort value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000ACA9 File Offset: 0x00008EA9
		public static implicit operator JToken(int value)
		{
			return new JValue((long)value);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000ACB2 File Offset: 0x00008EB2
		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000ACBF File Offset: 0x00008EBF
		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000ACC7 File Offset: 0x00008EC7
		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000ACE1 File Offset: 0x00008EE1
		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000ACEE File Offset: 0x00008EEE
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000ACFB File Offset: 0x00008EFB
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000AD08 File Offset: 0x00008F08
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000AD15 File Offset: 0x00008F15
		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000AD1D File Offset: 0x00008F1D
		public static implicit operator JToken(float value)
		{
			return new JValue((double)value);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000AD26 File Offset: 0x00008F26
		public static implicit operator JToken(string value)
		{
			return new JValue(value);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000AD2E File Offset: 0x00008F2E
		public static implicit operator JToken(uint value)
		{
			return new JValue((long)((ulong)value));
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000AD37 File Offset: 0x00008F37
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000AD3F File Offset: 0x00008F3F
		IEnumerator IEnumerable.GetEnumerator()
		{
            return ((IEnumerable<JToken>)this).GetEnumerator();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000AD48 File Offset: 0x00008F48
		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x060002C1 RID: 705
		internal abstract int GetDeepHashCode();

		// Token: 0x17000076 RID: 118
		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key]
		{
			get
			{
				return this[key];
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000AD6C File Offset: 0x00008F6C
		public JsonReader CreateReader()
		{
			return new JTokenReader(this);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000AD74 File Offset: 0x00008F74
		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			JToken token;
			using (JTokenWriter jtokenWriter = new JTokenWriter())
			{
				jsonSerializer.Serialize(jtokenWriter, o);
				token = jtokenWriter.Token;
			}
			return token;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000ADCC File Offset: 0x00008FCC
		public static JToken FromObject(object o)
		{
			return JToken.FromObjectInternal(o, new JsonSerializer());
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000ADD9 File Offset: 0x00008FD9
		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return JToken.FromObjectInternal(o, jsonSerializer);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000ADE4 File Offset: 0x00008FE4
		public static JToken ReadFrom(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JToken from JsonReader.");
			}
			if (reader.TokenType == JsonToken.StartObject)
			{
				return JObject.Load(reader);
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				return JArray.Load(reader);
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				return JProperty.Load(reader);
			}
			if (reader.TokenType == JsonToken.StartConstructor)
			{
				return JConstructor.Load(reader);
			}
			if (!JsonReader.IsStartToken(reader.TokenType))
			{
				return new JValue(reader.Value);
			}
			throw new Exception("Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				reader.TokenType
			}));
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000AE9A File Offset: 0x0000909A
		internal void SetLineInfo(IJsonLineInfo lineInfo)
		{
			if (lineInfo == null || !lineInfo.HasLineInfo())
			{
				return;
			}
			this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000AEBA File Offset: 0x000090BA
		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			this._lineNumber = new int?(lineNumber);
			this._linePosition = new int?(linePosition);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000AED4 File Offset: 0x000090D4
		bool IJsonLineInfo.HasLineInfo()
		{
			return this._lineNumber != null && this._linePosition != null;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000AEF0 File Offset: 0x000090F0
		int IJsonLineInfo.LineNumber
		{
			get
			{
				int? lineNumber = this._lineNumber;
				if (lineNumber == null)
				{
					return 0;
				}
				return lineNumber.GetValueOrDefault();
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000AF18 File Offset: 0x00009118
		int IJsonLineInfo.LinePosition
		{
			get
			{
				int? linePosition = this._linePosition;
				if (linePosition == null)
				{
					return 0;
				}
				return linePosition.GetValueOrDefault();
			}
		}

		// Token: 0x040000C1 RID: 193
		private JContainer _parent;

		// Token: 0x040000C2 RID: 194
		internal JToken _next;

		// Token: 0x040000C3 RID: 195
		private static JTokenEqualityComparer _equalityComparer;

		// Token: 0x040000C4 RID: 196
		private int? _lineNumber;

		// Token: 0x040000C5 RID: 197
		private int? _linePosition;
	}
}
