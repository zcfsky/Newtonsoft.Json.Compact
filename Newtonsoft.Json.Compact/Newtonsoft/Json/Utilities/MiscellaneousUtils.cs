using System;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000078 RID: 120
	internal static class MiscellaneousUtils
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00015D84 File Offset: 0x00013F84
		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string text = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				actualValue
			});
			return new ArgumentOutOfRangeException(paramName, text);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00015DC0 File Offset: 0x00013FC0
		public static bool TryAction<T>(Creator<T> creator, out T output)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			bool result;
			try
			{
				output = creator();
				result = true;
			}
			catch
			{
				output = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00015E08 File Offset: 0x00014008
		public static string ToString(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (!(value is string))
			{
				return value.ToString();
			}
			return "\"" + value.ToString() + "\"";
		}
	}
}
