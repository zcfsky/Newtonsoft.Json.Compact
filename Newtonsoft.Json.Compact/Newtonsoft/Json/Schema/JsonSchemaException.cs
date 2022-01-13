using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000038 RID: 56
	public class JsonSchemaException : Exception
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000D908 File Offset: 0x0000BB08
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000D910 File Offset: 0x0000BB10
		public int LineNumber { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000D919 File Offset: 0x0000BB19
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000D921 File Offset: 0x0000BB21
		public int LinePosition { get; private set; }

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D92A File Offset: 0x0000BB2A
		public JsonSchemaException()
		{
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D932 File Offset: 0x0000BB32
		public JsonSchemaException(string message) : base(message)
		{
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D93B File Offset: 0x0000BB3B
		public JsonSchemaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D945 File Offset: 0x0000BB45
		internal JsonSchemaException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
