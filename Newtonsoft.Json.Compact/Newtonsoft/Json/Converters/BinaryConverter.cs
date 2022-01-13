using System;
using System.Data.SqlTypes;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000004 RID: 4
	public class BinaryConverter : JsonConverter
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020D8 File Offset: 0x000002D8
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] array = value as byte[];
			if (array == null)
			{
				array = this.GetByteArray(value);
			}
			writer.WriteValue(Convert.ToBase64String(array));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002110 File Offset: 0x00000310
		private byte[] GetByteArray(object value)
		{
			if (value is SqlBinary)
			{
				return ((SqlBinary)value).Value;
			}
			throw new Exception("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000215C File Offset: 0x0000035C
		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
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
					throw new Exception("Unexpected token parsing binary. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						reader.TokenType
					}));
				}
				string text = reader.Value.ToString();
				byte[] array = Convert.FromBase64String(text);
				if (type == typeof(byte[]))
				{
					return array;
				}
				if (typeof(SqlBinary).IsAssignableFrom(type))
				{
					return new SqlBinary(array);
				}
				throw new Exception("Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					objectType
				}));
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000224C File Offset: 0x0000044C
		public override bool CanConvert(Type objectType)
		{
			Type type = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
			return type == typeof(byte[]) || typeof(SqlBinary).IsAssignableFrom(type);
		}
	}
}
