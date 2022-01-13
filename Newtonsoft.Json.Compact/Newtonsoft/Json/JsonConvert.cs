using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200002A RID: 42
	public static class JsonConvert
	{
		// Token: 0x060001FE RID: 510 RVA: 0x00008908 File Offset: 0x00006B08
		public static string ToString(DateTime value)
		{
			TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(value);
			return JsonConvert.ToStringInternal(value, utcOffset, value.Kind);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000892F File Offset: 0x00006B2F
		public static string ToString(DateTimeOffset value)
		{
			return JsonConvert.ToStringInternal(value.UtcDateTime, value.Offset, (DateTimeKind)2);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008948 File Offset: 0x00006B48
		internal static string ToStringInternal(DateTime value, TimeSpan offset, DateTimeKind kind)
		{
			long num = JsonConvert.ConvertDateTimeToJavaScriptTicks(value);
			string text;
			switch ((int)kind)
			{
			case 0:
			case 2:
				text = offset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + offset.Minutes.ToString("00;00", CultureInfo.InvariantCulture);
				goto IL_5E;
			}
			text = string.Empty;
			IL_5E:
			return "\"\\/Date(" + num.ToString(CultureInfo.InvariantCulture) + text + ")\\/\"";
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000089D0 File Offset: 0x00006BD0
		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return (dateTime.ToUniversalTime().Ticks - JsonConvert.InitialJavaScriptDateTicks) / 10000L;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000089FC File Offset: 0x00006BFC
		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			DateTime result = new DateTime(javaScriptTicks * 10000L + JsonConvert.InitialJavaScriptDateTicks, (DateTimeKind)1);
			return result;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008A20 File Offset: 0x00006C20
		public static string ToString(bool value)
		{
			if (!value)
			{
				return JsonConvert.False;
			}
			return JsonConvert.True;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008A30 File Offset: 0x00006C30
		public static string ToString(char value)
		{
			return JsonConvert.ToString(char.ToString(value));
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008A3D File Offset: 0x00006C3D
		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008A4A File Offset: 0x00006C4A
		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008A59 File Offset: 0x00006C59
		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008A68 File Offset: 0x00006C68
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008A77 File Offset: 0x00006C77
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008A86 File Offset: 0x00006C86
		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008A95 File Offset: 0x00006C95
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008AA4 File Offset: 0x00006CA4
		public static string ToString(float value)
		{
			return JsonConvert.EnsureDecimalPlace((double)value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008ABE File Offset: 0x00006CBE
		public static string ToString(double value)
		{
			return JsonConvert.EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008AD7 File Offset: 0x00006CD7
		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008B0C File Offset: 0x00006D0C
		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00008B26 File Offset: 0x00006D26
		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008B35 File Offset: 0x00006D35
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00008B44 File Offset: 0x00006D44
		public static string ToString(decimal value)
		{
			return JsonConvert.EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00008B58 File Offset: 0x00006D58
		public static string ToString(Guid value)
		{
			return '"' + value.ToString("D", CultureInfo.InvariantCulture) + '"';
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008B7E File Offset: 0x00006D7E
		public static string ToString(string value)
		{
			return JsonConvert.ToString(value, '"');
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008B88 File Offset: 0x00006D88
		public static string ToString(string value, char delimter)
		{
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimter, true);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008B94 File Offset: 0x00006D94
		public static string ToString(object value)
		{
			if (value == null)
			{
				return JsonConvert.Null;
			}
			if (value is IConvertible)
			{
				IConvertible convertible = (IConvertible)value;
				switch ((int)convertible.GetTypeCode())
				{
				case 2:
					return JsonConvert.Null;
				case 3:
					return JsonConvert.ToString(convertible.ToBoolean(CultureInfo.InvariantCulture));
				case 4:
					return JsonConvert.ToString(convertible.ToChar(CultureInfo.InvariantCulture));
				case 5:
					return JsonConvert.ToString(convertible.ToSByte(CultureInfo.InvariantCulture));
				case 6:
					return JsonConvert.ToString(convertible.ToByte(CultureInfo.InvariantCulture));
				case 7:
					return JsonConvert.ToString(convertible.ToInt16(CultureInfo.InvariantCulture));
				case 8:
					return JsonConvert.ToString(convertible.ToUInt16(CultureInfo.InvariantCulture));
				case 9:
					return JsonConvert.ToString(convertible.ToInt32(CultureInfo.InvariantCulture));
				case 10:
					return JsonConvert.ToString(convertible.ToUInt32(CultureInfo.InvariantCulture));
				case 11:
					return JsonConvert.ToString(convertible.ToInt64(CultureInfo.InvariantCulture));
				case 12:
					return JsonConvert.ToString(convertible.ToUInt64(CultureInfo.InvariantCulture));
				case 13:
					return JsonConvert.ToString(convertible.ToSingle(CultureInfo.InvariantCulture));
				case 14:
					return JsonConvert.ToString(convertible.ToDouble(CultureInfo.InvariantCulture));
				case 15:
					return JsonConvert.ToString(convertible.ToDecimal(CultureInfo.InvariantCulture));
				case 16:
					return JsonConvert.ToString(convertible.ToDateTime(CultureInfo.InvariantCulture));
				case 18:
					return JsonConvert.ToString(convertible.ToString(CultureInfo.InvariantCulture));
				}
			}
			else if (value is DateTimeOffset)
			{
				return JsonConvert.ToString((DateTimeOffset)value);
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008D54 File Offset: 0x00006F54
		internal static bool IsJsonPrimitive(object value)
		{
			if (value == null)
			{
				return true;
			}
			if (value is IConvertible)
			{
				IConvertible convertible = (IConvertible)value;
				switch ((int)convertible.GetTypeCode())
				{
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 18:
					return true;
				}
				return false;
			}
			return value is DateTimeOffset;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00008DD9 File Offset: 0x00006FD9
		public static string SerializeObject(object value)
		{
            return JsonConvert.SerializeObject(value, Formatting.None, (JsonSerializerSettings)null);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008DE3 File Offset: 0x00006FE3
		public static string SerializeObject(object value, Formatting formatting)
		{
            return JsonConvert.SerializeObject(value, formatting, (JsonSerializerSettings)null);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00008DED File Offset: 0x00006FED
		public static string SerializeObject(object value, params JsonConverter[] converters)
		{
			return JsonConvert.SerializeObject(value, Formatting.None, converters);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008DF8 File Offset: 0x00006FF8
		public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.SerializeObject(value, formatting, settings);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00008E28 File Offset: 0x00007028
		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = formatting;
				jsonSerializer.Serialize(jsonTextWriter, value);
			}
			return stringWriter.ToString();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00008E80 File Offset: 0x00007080
		public static object DeserializeObject(string value)
		{
            return JsonConvert.DeserializeObject(value, null, (JsonSerializerSettings)null);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008E8A File Offset: 0x0000708A
		public static object DeserializeObject(string value, Type type)
		{
            return JsonConvert.DeserializeObject(value, type, (JsonSerializerSettings)null);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00008E94 File Offset: 0x00007094
		public static T DeserializeObject<T>(string value)
		{
            return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings)null);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008E9D File Offset: 0x0000709D
		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008EA5 File Offset: 0x000070A5
		public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), converters));
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008EBD File Offset: 0x000070BD
		public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
		{
			return (T)((object)JsonConvert.DeserializeObject(value, typeof(T), settings));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008ED8 File Offset: 0x000070D8
		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			JsonSerializerSettings settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings
			{
				Converters = converters
			} : null;
			return JsonConvert.DeserializeObject(value, type, settings);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008F08 File Offset: 0x00007108
		public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
		{
			StringReader reader = new StringReader(value);
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			object result;
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				result = jsonSerializer.Deserialize(jsonReader, type);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
			return result;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008F6C File Offset: 0x0000716C
		public static void PopulateObject(string value, object target)
		{
			JsonConvert.PopulateObject(value, target, null);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008F78 File Offset: 0x00007178
		public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
		{
			StringReader reader = new StringReader(value);
			JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
			using (JsonReader jsonReader = new JsonTextReader(reader))
			{
				jsonSerializer.Populate(jsonReader, target);
				if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008FDC File Offset: 0x000071DC
		public static string SerializeXmlNode(XmlNode node)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return JsonConvert.SerializeObject(node, new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009001 File Offset: 0x00007201
		public static XmlNode DeserializeXmlNode(string value)
		{
			return JsonConvert.DeserializeXmlNode(value, null);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000900C File Offset: 0x0000720C
		public static XmlNode DeserializeXmlNode(string value, string deserializeRootElementName)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			return (XmlDocument)JsonConvert.DeserializeObject(value, typeof(XmlDocument), new JsonConverter[]
			{
				xmlNodeConverter
			});
		}

		// Token: 0x040000A9 RID: 169
		public static readonly string True = "true";

		// Token: 0x040000AA RID: 170
		public static readonly string False = "false";

		// Token: 0x040000AB RID: 171
		public static readonly string Null = "null";

		// Token: 0x040000AC RID: 172
		public static readonly string Undefined = "undefined";

		// Token: 0x040000AD RID: 173
		public static readonly string PositiveInfinity = "Infinity";

		// Token: 0x040000AE RID: 174
		public static readonly string NegativeInfinity = "-Infinity";

		// Token: 0x040000AF RID: 175
		public static readonly string NaN = "NaN";

		// Token: 0x040000B0 RID: 176
		internal static long InitialJavaScriptDateTicks = new DateTime(1970, 1, 1, 0, 0, 0, 1).Ticks;

		// Token: 0x040000B1 RID: 177
		internal static DateTime MinimumJavaScriptDate = new DateTime(100, 1, 1);
	}
}
