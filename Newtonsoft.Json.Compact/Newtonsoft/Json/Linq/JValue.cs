using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000035 RID: 53
	public class JValue : JToken, IEquatable<JValue>
	{
		// Token: 0x06000396 RID: 918 RVA: 0x0000D1FC File Offset: 0x0000B3FC
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000D212 File Offset: 0x0000B412
		public JValue(JValue other) : this(other.Value, other.Type)
		{
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000D226 File Offset: 0x0000B426
		public JValue(long value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000D235 File Offset: 0x0000B435
		public JValue(ulong value) : this(value, JTokenType.Integer)
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000D244 File Offset: 0x0000B444
		public JValue(double value) : this(value, JTokenType.Float)
		{
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000D253 File Offset: 0x0000B453
		public JValue(DateTime value) : this(value, JTokenType.Date)
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000D263 File Offset: 0x0000B463
		public JValue(bool value) : this(value, JTokenType.Boolean)
		{
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000D273 File Offset: 0x0000B473
		public JValue(string value) : this(value, JTokenType.String)
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000D280 File Offset: 0x0000B480
		public JValue(object value) : this(value, JValue.GetValueType(default(JTokenType?), value))
		{
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && (this == jvalue || (this._valueType == jvalue.Type && this.Compare(this._value, jvalue.Value)));
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000D2E5 File Offset: 0x0000B4E5
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D2E8 File Offset: 0x0000B4E8
		private bool Compare(object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return true;
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Boolean:
				return objA.Equals(objB);
			case JTokenType.Integer:
				if (objA is ulong || objB is ulong)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).Equals(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).Equals(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				return Convert.ToDouble(objA, CultureInfo.InvariantCulture).Equals(Convert.ToDouble(objB, CultureInfo.InvariantCulture));
			case JTokenType.Date:
				return objA.Equals(objB);
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", this._valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				this._valueType
			}));
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D3F0 File Offset: 0x0000B5F0
		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D3F9 File Offset: 0x0000B5F9
		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D402 File Offset: 0x0000B602
		public static JValue CreateRaw(string value)
		{
			return new JValue(value, JTokenType.Raw);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D40C File Offset: 0x0000B60C
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D4D0 File Offset: 0x0000B6D0
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType value = current.Value;
			if (value == JTokenType.Comment || value == JTokenType.String || value == JTokenType.Raw)
			{
				return current.Value;
			}
			return JTokenType.String;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000D506 File Offset: 0x0000B706
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0000D50E File Offset: 0x0000B70E
		// (set) Token: 0x060003AA RID: 938 RVA: 0x0000D518 File Offset: 0x0000B718
		public new object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				Type type = (this._value != null) ? this._value.GetType() : null;
				Type type2 = (value != null) ? value.GetType() : null;
				if (type != type2)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D56C File Offset: 0x0000B76C
		private static void WriteConvertableValue(JsonWriter writer, IList<JsonConverter> converters, Action<object> defaultWrite, object value)
		{
			JsonConverter jsonConverter;
			if (value != null && JsonSerializer.HasMatchingConverter(converters, value.GetType(), out jsonConverter))
			{
				jsonConverter.WriteJson(writer, value, new JsonSerializer());
				return;
			}
			defaultWrite.Invoke(value);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D63C File Offset: 0x0000B83C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			switch (this._valueType)
			{
			case JTokenType.Comment:
				writer.WriteComment(this._value.ToString());
				return;
			case JTokenType.Integer:
				JValue.WriteConvertableValue(writer, converters, delegate(object v)
				{
					writer.WriteValue(Convert.ToInt64(v, CultureInfo.InvariantCulture));
				}, this._value);
				return;
			case JTokenType.Float:
				JValue.WriteConvertableValue(writer, converters, delegate(object v)
				{
					writer.WriteValue(Convert.ToDouble(v, CultureInfo.InvariantCulture));
				}, this._value);
				return;
			case JTokenType.String:
				JValue.WriteConvertableValue(writer, converters, delegate(object v)
				{
					writer.WriteValue((v != null) ? v.ToString() : null);
				}, this._value);
				return;
			case JTokenType.Boolean:
				JValue.WriteConvertableValue(writer, converters, delegate(object v)
				{
					writer.WriteValue(Convert.ToBoolean(v, CultureInfo.InvariantCulture));
				}, this._value);
				return;
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Date:
				JValue.WriteConvertableValue(writer, converters, delegate(object v)
				{
					if (v is DateTimeOffset)
					{
						writer.WriteValue((DateTimeOffset)v);
						return;
					}
					writer.WriteValue(Convert.ToDateTime(v, CultureInfo.InvariantCulture));
				}, this._value);
				return;
			case JTokenType.Raw:
				writer.WriteRawValue(this._value.ToString());
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
		internal override int GetDeepHashCode()
		{
			int num = (this._value != null) ? this._value.GetHashCode() : 0;
			return this._valueType.GetHashCode() ^ num;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000D7FA File Offset: 0x0000B9FA
		public bool Equals(JValue other)
		{
			return other != null && this._value == other._value;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000D810 File Offset: 0x0000BA10
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			JValue jvalue = obj as JValue;
			if (jvalue != null)
			{
				return this.Equals(jvalue);
			}
			return base.Equals(obj);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000D83B File Offset: 0x0000BA3B
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x040000D1 RID: 209
		private JTokenType _valueType;

		// Token: 0x040000D2 RID: 210
		private object _value;
	}
}
