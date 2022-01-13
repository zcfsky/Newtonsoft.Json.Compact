using System;
using System.IO;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006F RID: 111
	internal static class JavaScriptUtils
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x00014964 File Offset: 0x00012B64
		public static void WriteEscapedJavaScriptChar(TextWriter writer, char c, char delimiter)
		{
			if (c <= '"')
			{
				switch (c)
				{
				case '\b':
					writer.Write("\\b");
					return;
				case '\t':
					writer.Write("\\t");
					return;
				case '\n':
					writer.Write("\\n");
					return;
				case '\v':
					break;
				case '\f':
					writer.Write("\\f");
					return;
				case '\r':
					writer.Write("\\r");
					return;
				default:
					if (c == '"')
					{
						writer.Write((delimiter == '"') ? "\\\"" : "\"");
						return;
					}
					break;
				}
			}
			else
			{
				if (c == '\'')
				{
					writer.Write((delimiter == '\'') ? "\\'" : "'");
					return;
				}
				if (c == '\\')
				{
					writer.Write("\\\\");
					return;
				}
			}
			if (c > '\u001f')
			{
				writer.Write(c);
				return;
			}
			StringUtils.WriteCharAsUnicode(writer, c);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014A3C File Offset: 0x00012C3C
		public static void WriteEscapedJavaScriptString(TextWriter writer, string value, char delimiter, bool appendDelimiters)
		{
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					JavaScriptUtils.WriteEscapedJavaScriptChar(writer, value[i], delimiter);
				}
			}
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00014A7F File Offset: 0x00012C7F
		public static string ToEscapedJavaScriptString(string value)
		{
			return JavaScriptUtils.ToEscapedJavaScriptString(value, '"', true);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014A8C File Offset: 0x00012C8C
		public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
		{
			string result;
			using (StringWriter stringWriter = StringUtils.CreateStringWriter(StringUtils.GetLength(value) ?? 16))
			{
				JavaScriptUtils.WriteEscapedJavaScriptString(stringWriter, value, delimiter, appendDelimiters);
				result = stringWriter.ToString();
			}
			return result;
		}
	}
}
