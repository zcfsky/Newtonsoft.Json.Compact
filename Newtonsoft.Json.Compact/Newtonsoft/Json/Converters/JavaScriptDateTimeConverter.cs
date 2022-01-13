using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200001F RID: 31
	public class JavaScriptDateTimeConverter : JsonConverter
	{
		// Token: 0x06000178 RID: 376 RVA: 0x000060CC File Offset: 0x000042CC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			long value2;
			if (value is DateTime)
			{
				DateTime dateTime = ((DateTime)value).ToUniversalTime();
				value2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime);
			}
			else
			{
				value2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(((DateTimeOffset)value).ToUniversalTime().UtcDateTime);
			}
			writer.WriteStartConstructor("Date");
			writer.WriteValue(value2);
			writer.WriteEndConstructor();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006130 File Offset: 0x00004330
		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
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
				if (reader.TokenType != JsonToken.StartConstructor || string.Compare(reader.Value.ToString(), "Date", (StringComparison)4) != 0)
				{
					throw new Exception("Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType,
						reader.Value
					}));
				}
				reader.Read();
				if (reader.TokenType != JsonToken.Integer)
				{
					throw new Exception("Unexpected token parsing date. Expected Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				long javaScriptTicks = (long)reader.Value;
				DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
				reader.Read();
				if (reader.TokenType != JsonToken.EndConstructor)
				{
					throw new Exception("Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				if (type == typeof(DateTimeOffset))
				{
					return new DateTimeOffset(dateTime);
				}
				return dateTime;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000628C File Offset: 0x0000448C
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return typeof(DateTime).IsAssignableFrom(type) || typeof(DateTimeOffset).IsAssignableFrom(type);
		}
	}
}
