using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007E RID: 126
	internal static class ValidationUtils
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x0001738C File Offset: 0x0001558C
		public static void ArgumentNotNullOrEmpty(string value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (value.Length == 0)
			{
				throw new ArgumentException("'{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					parameterName
				}), parameterName);
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000173D0 File Offset: 0x000155D0
		public static void ArgumentNotNullOrEmptyOrWhitespace(string value, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(value, parameterName);
			if (StringUtils.IsWhiteSpace(value))
			{
				throw new ArgumentException("'{0}' cannot only be whitespace.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					parameterName
				}), parameterName);
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00017410 File Offset: 0x00015610
		public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
		{
			ValidationUtils.ArgumentNotNull(enumType, "enumType");
			if (!enumType.IsEnum)
			{
				throw new ArgumentException("Type {0} is not an Enum.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					enumType
				}), parameterName);
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00017454 File Offset: 0x00015654
		public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty<T>(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				parameterName
			}));
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00017483 File Offset: 0x00015683
		public static void ArgumentNotNullOrEmpty<T>(ICollection<T> collection, string parameterName, string message)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (collection.Count == 0)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000174A0 File Offset: 0x000156A0
		public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(collection, parameterName, "Collection '{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				parameterName
			}));
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000174CF File Offset: 0x000156CF
		public static void ArgumentNotNullOrEmpty(ICollection collection, string parameterName, string message)
		{
			if (collection == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (collection.Count == 0)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x000174EB File Offset: 0x000156EB
		public static void ArgumentNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x000174F7 File Offset: 0x000156F7
		public static void ArgumentNotNegative(int value, string parameterName)
		{
			if (value <= 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Argument cannot be negative.");
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001750F File Offset: 0x0001570F
		public static void ArgumentNotNegative(int value, string parameterName, string message)
		{
			if (value <= 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00017523 File Offset: 0x00015723
		public static void ArgumentNotZero(int value, string parameterName)
		{
			if (value == 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Argument cannot be zero.");
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001753A File Offset: 0x0001573A
		public static void ArgumentNotZero(int value, string parameterName, string message)
		{
			if (value == 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00017550 File Offset: 0x00015750
		public static void ArgumentIsPositive<T>(T value, string parameterName) where T : struct, IComparable<T>
		{
			if (value.CompareTo(default(T)) != 1)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, "Positive number required.");
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00017588 File Offset: 0x00015788
		public static void ArgumentIsPositive(int value, string parameterName, string message)
		{
			if (value > 0)
			{
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException(parameterName, value, message);
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001759C File Offset: 0x0001579C
		public static void ObjectNotDisposed(bool disposed, Type objectType)
		{
			if (disposed)
			{
				throw new ObjectDisposedException(objectType.Name);
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000175AD File Offset: 0x000157AD
		public static void ArgumentConditionTrue(bool condition, string parameterName, string message)
		{
			if (!condition)
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x040001E9 RID: 489
		public const string EmailAddressRegex = "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$";

		// Token: 0x040001EA RID: 490
		public const string CurrencyRegex = "(^\\$?(?!0,?\\d)\\d{1,3}(,?\\d{3})*(\\.\\d\\d)?)$";

		// Token: 0x040001EB RID: 491
		public const string DateRegex = "^(((0?[1-9]|[12]\\d|3[01])[\\.\\-\\/](0?[13578]|1[02])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|[12]\\d|30)[\\.\\-\\/](0?[13456789]|1[012])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|1\\d|2[0-8])[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|(29[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00|[048])))$";

		// Token: 0x040001EC RID: 492
		public const string NumericRegex = "\\d*";
	}
}
