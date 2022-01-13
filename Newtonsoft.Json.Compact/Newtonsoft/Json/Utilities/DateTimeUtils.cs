using System;
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000068 RID: 104
	internal static class DateTimeUtils
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x00013C4C File Offset: 0x00011E4C
		public static string GetLocalOffset(this DateTime d)
		{
			TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(d);
			return utcOffset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00013CA4 File Offset: 0x00011EA4
		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			switch ((int)kind)
			{
			case 0:
                    return (XmlDateTimeSerializationMode)2;
			case 1:
                    return (XmlDateTimeSerializationMode)1;
			case 2:
                    return (XmlDateTimeSerializationMode)0;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
			}
		}
	}
}
