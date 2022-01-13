using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001E RID: 30
	public class IsoDateTimeConverter : JsonConverter
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00005E0E File Offset: 0x0000400E
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00005E16 File Offset: 0x00004016
		public DateTimeStyles DateTimeStyles
		{
			get
			{
				return this._dateTimeStyles;
			}
			set
			{
				this._dateTimeStyles = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00005E1F File Offset: 0x0000401F
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00005E30 File Offset: 0x00004030
		public string DateTimeFormat
		{
			get
			{
				return this._dateTimeFormat ?? string.Empty;
			}
			set
			{
				this._dateTimeFormat = StringUtils.NullEmptyString(value);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00005E3E File Offset: 0x0000403E
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00005E4F File Offset: 0x0000404F
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.CurrentCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005E58 File Offset: 0x00004058
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string value2;
			if (value is DateTime)
			{
				DateTime dateTime = (DateTime)value;
				if (((int)this._dateTimeStyles & 16) == 16 || ((int)this._dateTimeStyles & 64) == 64)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				value2 = dateTime.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			else
			{
				if (!(value is DateTimeOffset))
				{
					throw new Exception("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						ReflectionUtils.GetObjectType(value)
					}));
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
				if (((int)this._dateTimeStyles & 16) == 16 || ((int)this._dateTimeStyles & 64) == 64)
				{
					dateTimeOffset = dateTimeOffset.ToUniversalTime();
				}
				value2 = dateTimeOffset.ToString(this._dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", this.Culture);
			}
			writer.WriteValue(value2);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005F34 File Offset: 0x00004134
		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw new Exception("Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						objectType
					}));
				}
				return null;
			}
			else
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw new Exception("Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				string text = reader.Value.ToString();
				if (string.IsNullOrEmpty(text) && flag)
				{
					return null;
				}
				if (type == typeof(DateTimeOffset))
				{
					if (!string.IsNullOrEmpty(this._dateTimeFormat))
					{
						return DateTimeOffset.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
					}
					return DateTimeOffset.Parse(text, this.Culture, this._dateTimeStyles);
				}
				else
				{
					if (!string.IsNullOrEmpty(this._dateTimeFormat))
					{
						return DateTime.ParseExact(text, this._dateTimeFormat, this.Culture, this._dateTimeStyles);
					}
					return DateTime.Parse(text, this.Culture, this._dateTimeStyles);
				}
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006070 File Offset: 0x00004270
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return typeof(DateTime).IsAssignableFrom(type) || typeof(DateTimeOffset).IsAssignableFrom(type);
		}

		// Token: 0x0400007E RID: 126
		private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		// Token: 0x0400007F RID: 127
        private DateTimeStyles _dateTimeStyles = (DateTimeStyles)128;

		// Token: 0x04000080 RID: 128
		private string _dateTimeFormat;

		// Token: 0x04000081 RID: 129
		private CultureInfo _culture;
	}
}
