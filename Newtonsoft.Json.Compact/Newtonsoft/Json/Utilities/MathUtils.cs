using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000076 RID: 118
	internal class MathUtils
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00015C1D File Offset: 0x00013E1D
		public static int HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return (int)(h - '0');
			}
			if (h >= 'a' && h <= 'f')
			{
				return (int)(h - 'a' + '\n');
			}
			if (h >= 'A' && h <= 'F')
			{
				return (int)(h - 'A' + '\n');
			}
			return -1;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00015C53 File Offset: 0x00013E53
		public static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00015C68 File Offset: 0x00013E68
		public static int GetDecimalPlaces(double value)
		{
			int num = 10;
			double num2 = Math.Pow(0.1, (double)num);
			if (value == 0.0)
			{
				return 0;
			}
			int num3 = 0;
			while (value - Math.Floor(value) > num2 && num3 < num)
			{
				value *= 10.0;
				num3++;
			}
			return num3;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00015CBC File Offset: 0x00013EBC
		public static int? Min(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Min(val1.Value, val2.Value));
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00015CEC File Offset: 0x00013EEC
		public static int? Max(int? val1, int? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new int?(Math.Max(val1.Value, val2.Value));
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00015D1C File Offset: 0x00013F1C
		public static double? Min(double? val1, double? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new double?(Math.Min(val1.Value, val2.Value));
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00015D4C File Offset: 0x00013F4C
		public static double? Max(double? val1, double? val2)
		{
			if (val1 == null)
			{
				return val2;
			}
			if (val2 == null)
			{
				return val1;
			}
			return new double?(Math.Max(val1.Value, val2.Value));
		}
	}
}
