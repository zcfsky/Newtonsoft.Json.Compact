using System;
using System.Globalization;

namespace System
{
	// Token: 0x02000007 RID: 7
	public struct DateTimeOffset : IComparable, IFormattable, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002435 File Offset: 0x00000635
		static DateTimeOffset()
		{
			DateTimeOffset.MaxValue = new DateTimeOffset(3155378975999999999L, TimeSpan.Zero);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002460 File Offset: 0x00000660
		public DateTimeOffset(DateTime dateTime)
		{
			TimeSpan utcOffset;
			if ((int)dateTime.Kind != 1)
			{
				utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			}
			else
			{
				utcOffset = new TimeSpan(0L);
			}
			this._offsetMinutes = DateTimeOffset.ValidateOffset(utcOffset);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, utcOffset);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024A8 File Offset: 0x000006A8
		public DateTimeOffset(DateTime dateTime, TimeSpan offset)
		{
			if ((int)dateTime.Kind == 2)
			{
				if (offset != TimeZone.CurrentTimeZone.GetUtcOffset(dateTime))
				{
					throw new ArgumentException("The UTC Offset of the local dateTime parameter does not match the offset argument.", "offset");
				}
			}
            else if ((int)dateTime.Kind == 1 && offset != TimeSpan.Zero)
			{
				throw new ArgumentException("The UTC Offset for Utc DateTime instances must be 0.", "offset");
			}
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002524 File Offset: 0x00000724
		public DateTimeOffset(long ticks, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			DateTime dateTime = new DateTime(ticks);
			this._dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002552 File Offset: 0x00000752
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second), offset);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000257C File Offset: 0x0000077C
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025B4 File Offset: 0x000007B4
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
		{
			this._offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this._dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025F0 File Offset: 0x000007F0
		public DateTimeOffset Add(TimeSpan timeSpan)
		{
			return new DateTimeOffset(this.ClockDateTime.Add(timeSpan), this.Offset);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002618 File Offset: 0x00000818
		public DateTimeOffset AddDays(double days)
		{
			return new DateTimeOffset(this.ClockDateTime.AddDays(days), this.Offset);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002640 File Offset: 0x00000840
		public DateTimeOffset AddHours(double hours)
		{
			return new DateTimeOffset(this.ClockDateTime.AddHours(hours), this.Offset);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002668 File Offset: 0x00000868
		public DateTimeOffset AddMilliseconds(double milliseconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMilliseconds(milliseconds), this.Offset);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002690 File Offset: 0x00000890
		public DateTimeOffset AddMinutes(double minutes)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMinutes(minutes), this.Offset);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026B8 File Offset: 0x000008B8
		public DateTimeOffset AddMonths(int months)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMonths(months), this.Offset);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000026E0 File Offset: 0x000008E0
		public DateTimeOffset AddSeconds(double seconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddSeconds(seconds), this.Offset);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002708 File Offset: 0x00000908
		public DateTimeOffset AddTicks(long ticks)
		{
			return new DateTimeOffset(this.ClockDateTime.AddTicks(ticks), this.Offset);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002730 File Offset: 0x00000930
		public DateTimeOffset AddYears(int years)
		{
			return new DateTimeOffset(this.ClockDateTime.AddYears(years), this.Offset);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002757 File Offset: 0x00000957
		public static int Compare(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Compare(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000276C File Offset: 0x0000096C
		public int CompareTo(DateTimeOffset other)
		{
			DateTime utcDateTime = other.UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000027A0 File Offset: 0x000009A0
		public bool Equals(DateTimeOffset other)
		{
			return this.UtcDateTime.Equals(other.UtcDateTime);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027C4 File Offset: 0x000009C4
		public override bool Equals(object obj)
		{
			if (obj is DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
				return this.UtcDateTime.Equals(dateTimeOffset.UtcDateTime);
			}
			return false;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000027F7 File Offset: 0x000009F7
		public static bool Equals(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Equals(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000280C File Offset: 0x00000A0C
		public bool EqualsExact(DateTimeOffset other)
		{
			return this.ClockDateTime == other.ClockDateTime && this.Offset == other.Offset && this.ClockDateTime.Kind == other.ClockDateTime.Kind;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002862 File Offset: 0x00000A62
		public static DateTimeOffset FromFileTime(long fileTime)
		{
			return new DateTimeOffset(DateTime.FromFileTime(fileTime));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002870 File Offset: 0x00000A70
		public override int GetHashCode()
		{
			return this.UtcDateTime.GetHashCode();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002891 File Offset: 0x00000A91
		public static DateTimeOffset operator +(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime + timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028AC File Offset: 0x00000AAC
		public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime == right.UtcDateTime;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000028C1 File Offset: 0x00000AC1
		public static bool operator >(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime > right.UtcDateTime;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028D6 File Offset: 0x00000AD6
		public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime >= right.UtcDateTime;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028EB File Offset: 0x00000AEB
		public static implicit operator DateTimeOffset(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028F3 File Offset: 0x00000AF3
		public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime != right.UtcDateTime;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002908 File Offset: 0x00000B08
		public static bool operator <(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime < right.UtcDateTime;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000291D File Offset: 0x00000B1D
		public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime <= right.UtcDateTime;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002932 File Offset: 0x00000B32
		public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime - right.UtcDateTime;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002947 File Offset: 0x00000B47
		public static DateTimeOffset operator -(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime - timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002962 File Offset: 0x00000B62
		public static DateTimeOffset Parse(string input)
		{
			return new DateTimeOffset(DateTime.Parse(input, DateTimeFormatInfo.CurrentInfo, 0));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002975 File Offset: 0x00000B75
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
		{
			return DateTimeOffset.Parse(input, formatProvider, 0);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000297F File Offset: 0x00000B7F
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			return new DateTimeOffset(DateTime.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000029A0 File Offset: 0x00000BA0
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			return DateTimeOffset.ParseExact(input, format, formatProvider, 0);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000029AB File Offset: 0x00000BAB
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			return new DateTimeOffset(DateTime.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000029CD File Offset: 0x00000BCD
		public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			return new DateTimeOffset(DateTime.ParseExact(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000029F0 File Offset: 0x00000BF0
		public TimeSpan Subtract(DateTimeOffset value)
		{
			return this.UtcDateTime.Subtract(value.UtcDateTime);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A14 File Offset: 0x00000C14
		public DateTimeOffset Subtract(TimeSpan value)
		{
			return new DateTimeOffset(this.ClockDateTime.Subtract(value), this.Offset);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A3C File Offset: 0x00000C3C
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is DateTimeOffset))
			{
				throw new ArgumentException("Object must be of type DateTimeOffset.");
			}
			DateTime utcDateTime = ((DateTimeOffset)obj).UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A90 File Offset: 0x00000C90
		public long ToFileTime()
		{
			return this.UtcDateTime.ToFileTime();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002AAC File Offset: 0x00000CAC
		public DateTimeOffset ToLocalTime()
		{
			return new DateTimeOffset(this.UtcDateTime.ToLocalTime());
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002ACC File Offset: 0x00000CCC
		public DateTimeOffset ToOffset(TimeSpan offset)
		{
			return new DateTimeOffset((this._dateTime + offset).Ticks, offset);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (!string.IsNullOrEmpty(format))
			{
				format = format.Replace("K", "zzz");
				format = format.Replace("zzz", this.Offset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + ":" + this.Offset.Minutes.ToString("00;00", CultureInfo.InvariantCulture));
				format = format.Replace("zz", this.Offset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture));
				format = format.Replace("z", this.Offset.Hours.ToString("+0;-0", CultureInfo.InvariantCulture));
			}
			return this.ClockDateTime.ToString(format, formatProvider);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public DateTimeOffset ToUniversalTime()
		{
			return new DateTimeOffset(this.UtcDateTime);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private static DateTime ValidateDate(DateTime dateTime, TimeSpan offset)
		{
			long num = dateTime.Ticks - offset.Ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("offset", "The UTC time represented when the offset is applied must be between year 0 and 10,000.");
			}
			return new DateTime(num, 0);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C3C File Offset: 0x00000E3C
		private static short ValidateOffset(TimeSpan offset)
		{
			long ticks = offset.Ticks;
			if (ticks % 600000000L != 0L)
			{
				throw new ArgumentException("Offset must be specified in whole minutes.", "offset");
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				throw new ArgumentOutOfRangeException("offset", "Offset must be within plus or minus 14 hours.");
			}
			return (short)(offset.Ticks / 600000000L);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002CA4 File Offset: 0x00000EA4
		private static DateTimeStyles ValidateStyles(DateTimeStyles style, string parameterName)
		{
			if (((int)style & -256) != null)
			{
				throw new ArgumentException("An undefined DateTimeStyles value is being used.", parameterName);
			}
            if (((int)style & 32) != null && ((int)style & 64) != null)
			{
				throw new ArgumentException("The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.", parameterName);
			}
            if (((int)style & 8) != null)
			{
				throw new ArgumentException("The DateTimeStyles value 'NoCurrentDateDefault' is not allowed when parsing DateTimeOffset.", parameterName);
			}
            style &= (DateTimeStyles)(-129);
            style &= (DateTimeStyles)(-33);
			return style;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002D00 File Offset: 0x00000F00
		private DateTime ClockDateTime
		{
			get
			{
				return new DateTime((this._dateTime + this.Offset).Ticks, 0);
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002D2C File Offset: 0x00000F2C
		public DateTime Date
		{
			get
			{
				return this.ClockDateTime.Date;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002D47 File Offset: 0x00000F47
		public DateTime DateTime
		{
			get
			{
				return this.ClockDateTime;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002D50 File Offset: 0x00000F50
		public int Day
		{
			get
			{
				return this.ClockDateTime.Day;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002D6C File Offset: 0x00000F6C
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.ClockDateTime.DayOfWeek;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002D88 File Offset: 0x00000F88
		public int DayOfYear
		{
			get
			{
				return this.ClockDateTime.DayOfYear;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public int Hour
		{
			get
			{
				return this.ClockDateTime.Hour;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public DateTime LocalDateTime
		{
			get
			{
				return this.UtcDateTime.ToLocalTime();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002DDC File Offset: 0x00000FDC
		public int Millisecond
		{
			get
			{
				return this.ClockDateTime.Millisecond;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public int Minute
		{
			get
			{
				return this.ClockDateTime.Minute;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002E14 File Offset: 0x00001014
		public int Month
		{
			get
			{
				return this.ClockDateTime.Month;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002E2F File Offset: 0x0000102F
		public static DateTimeOffset Now
		{
			get
			{
				return new DateTimeOffset(DateTime.Now);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002E3B File Offset: 0x0000103B
		public TimeSpan Offset
		{
			get
			{
				return new TimeSpan(0, (int)this._offsetMinutes, 0);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002E4C File Offset: 0x0000104C
		public int Second
		{
			get
			{
				return this.ClockDateTime.Second;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002E68 File Offset: 0x00001068
		public long Ticks
		{
			get
			{
				return this.ClockDateTime.Ticks;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002E84 File Offset: 0x00001084
		public TimeSpan TimeOfDay
		{
			get
			{
				return this.ClockDateTime.TimeOfDay;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002E9F File Offset: 0x0000109F
		public DateTime UtcDateTime
		{
			get
			{
				return DateTime.SpecifyKind(this._dateTime, (DateTimeKind)1);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002EAD File Offset: 0x000010AD
		public static DateTimeOffset UtcNow
		{
			get
			{
				return new DateTimeOffset(DateTime.UtcNow);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002EBC File Offset: 0x000010BC
		public long UtcTicks
		{
			get
			{
				return this.UtcDateTime.Ticks;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002ED8 File Offset: 0x000010D8
		public int Year
		{
			get
			{
				return this.ClockDateTime.Year;
			}
		}

		// Token: 0x04000004 RID: 4
		internal const long MaxOffset = 504000000000L;

		// Token: 0x04000005 RID: 5
		internal const long MinOffset = -504000000000L;

		// Token: 0x04000006 RID: 6
		private DateTime _dateTime;

		// Token: 0x04000007 RID: 7
		private short _offsetMinutes;

		// Token: 0x04000008 RID: 8
		public static readonly DateTimeOffset MaxValue;

		// Token: 0x04000009 RID: 9
		public static readonly DateTimeOffset MinValue = new DateTimeOffset(0L, TimeSpan.Zero);
	}
}
