using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007B RID: 123
	internal static class StringUtils
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x00016D19 File Offset: 0x00014F19
		public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00016D30 File Offset: 0x00014F30
		public static bool ContainsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsWhiteSpace(s[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00016D70 File Offset: 0x00014F70
		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00016DB8 File Offset: 0x00014FB8
		public static string EnsureEndsWith(string target, string value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (target.Length >= value.Length)
			{
				if (string.Compare(target, target.Length - value.Length, value, 0, value.Length, (StringComparison)5) == 0)
				{
					return target;
				}
				string text = target.TrimEnd(null);
                if (string.Compare(text, text.Length - value.Length, value, 0, value.Length, (StringComparison)5) == 0)
				{
					return target;
				}
			}
			return target + value;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00016E3E File Offset: 0x0001503E
		public static bool IsNullOrEmptyOrWhiteSpace(string s)
		{
			return string.IsNullOrEmpty(s) || StringUtils.IsWhiteSpace(s);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00016E55 File Offset: 0x00015055
		public static void IfNotNullOrEmpty(string value, Action<string> action)
		{
			StringUtils.IfNotNullOrEmpty(value, action, null);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00016E5F File Offset: 0x0001505F
		private static void IfNotNullOrEmpty(string value, Action<string> trueAction, Action<string> falseAction)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (trueAction != null)
				{
					trueAction.Invoke(value);
					return;
				}
			}
			else if (falseAction != null)
			{
				falseAction.Invoke(value);
			}
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00016E7E File Offset: 0x0001507E
		public static string Indent(string s, int indentation)
		{
			return StringUtils.Indent(s, indentation, ' ');
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00016EB4 File Offset: 0x000150B4
		public static string Indent(string s, int indentation, char indentChar)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (indentation <= 0)
			{
				throw new ArgumentException("Must be greater than zero.", "indentation");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(new string(indentChar, indentation));
				tw.Write(line);
			});
			return stringWriter.ToString();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00016F28 File Offset: 0x00015128
		private static void ActionTextReaderLine(TextReader textReader, TextWriter textWriter, StringUtils.ActionLine lineAction)
		{
			bool flag = true;
			string line;
			while ((line = textReader.ReadLine()) != null)
			{
				if (!flag)
				{
					textWriter.WriteLine();
				}
				else
				{
					flag = false;
				}
				lineAction(textWriter, line);
			}
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00016FA0 File Offset: 0x000151A0
		public static string NumberLines(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			StringReader textReader = new StringReader(s);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			int lineNumber = 1;
			StringUtils.ActionTextReaderLine(textReader, stringWriter, delegate(TextWriter tw, string line)
			{
				tw.Write(lineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4));
				tw.Write(". ");
				tw.Write(line);
				lineNumber++;
			});
			return stringWriter.ToString();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00016FF3 File Offset: 0x000151F3
		public static string NullEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00017000 File Offset: 0x00015200
		public static string ReplaceNewLines(string s, string replacement)
		{
			StringReader stringReader = new StringReader(s);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(replacement);
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00017045 File Offset: 0x00015245
		public static string Truncate(string s, int maximumLength)
		{
			return StringUtils.Truncate(s, maximumLength, "...");
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00017054 File Offset: 0x00015254
		public static string Truncate(string s, int maximumLength, string suffix)
		{
			if (suffix == null)
			{
				throw new ArgumentNullException("suffix");
			}
			if (maximumLength <= 0)
			{
				throw new ArgumentException("Maximum length must be greater than zero.", "maximumLength");
			}
			int num = maximumLength - suffix.Length;
			if (num <= 0)
			{
				throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");
			}
			if (s != null && s.Length > maximumLength)
			{
				string text = s.Substring(0, num);
				text = text.Trim();
				return text + suffix;
			}
			return s;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000170C4 File Offset: 0x000152C4
		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder stringBuilder = new StringBuilder(capacity);
			return new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x000170E8 File Offset: 0x000152E8
		public static int? GetLength(string value)
		{
			if (value == null)
			{
				return default(int?);
			}
			return new int?(value.Length);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00017110 File Offset: 0x00015310
		public static string ToCharAsUnicode(char c)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				StringUtils.WriteCharAsUnicode(stringWriter, c);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00017154 File Offset: 0x00015354
		public static void WriteCharAsUnicode(TextWriter writer, char c)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			char c2 = MathUtils.IntToHex((int)(c >> 12 & '\u000f'));
			char c3 = MathUtils.IntToHex((int)(c >> 8 & '\u000f'));
			char c4 = MathUtils.IntToHex((int)(c >> 4 & '\u000f'));
			char c5 = MathUtils.IntToHex((int)(c & '\u000f'));
			writer.Write('\\');
			writer.Write('u');
			writer.Write(c2);
			writer.Write(c3);
			writer.Write(c4);
			writer.Write(c5);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001720C File Offset: 0x0001540C
		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> enumerable = Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, (StringComparison)5) == 0);
			if (Enumerable.Count<TSource>(enumerable) <= 1)
			{
				return Enumerable.SingleOrDefault<TSource>(enumerable);
			}
            IEnumerable<TSource> enumerable2 = Enumerable.Where<TSource>(source, (TSource s) => string.Compare(valueSelector.Invoke(s), testValue, (StringComparison)4) == 0);
			return Enumerable.SingleOrDefault<TSource>(enumerable2);
		}

		// Token: 0x040001E1 RID: 481
		public const string CarriageReturnLineFeed = "\r\n";

		// Token: 0x040001E2 RID: 482
		public const string Empty = "";

		// Token: 0x040001E3 RID: 483
		public const char CarriageReturn = '\r';

		// Token: 0x040001E4 RID: 484
		public const char LineFeed = '\n';

		// Token: 0x040001E5 RID: 485
		public const char Tab = '\t';

		// Token: 0x0200007C RID: 124
		// (Invoke) Token: 0x06000665 RID: 1637
		private delegate void ActionLine(TextWriter textWriter, string line);
	}
}
