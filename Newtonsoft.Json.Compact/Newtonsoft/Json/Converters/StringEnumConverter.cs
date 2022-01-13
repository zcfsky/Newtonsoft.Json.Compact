using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000006 RID: 6
	public class StringEnumConverter : JsonConverter
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022FC File Offset: 0x000004FC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string text = @enum.ToString("G");
			if (char.IsNumber(text[0]) || text[0] == '-')
			{
				writer.WriteValue(value);
				return;
			}
			writer.WriteValue(text);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002350 File Offset: 0x00000550
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
				if (reader.TokenType == JsonToken.String)
				{
					return Enum.Parse(type, reader.Value.ToString(), true);
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
				}
				throw new Exception("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002408 File Offset: 0x00000608
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type.IsEnum;
		}
	}
}
