using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000027 RID: 39
	public class JsonReaderException : Exception
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00008759 File Offset: 0x00006959
		// (set) Token: 0x060001EF RID: 495 RVA: 0x00008761 File Offset: 0x00006961
		public int LineNumber { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000876A File Offset: 0x0000696A
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00008772 File Offset: 0x00006972
		public int LinePosition { get; private set; }

		// Token: 0x060001F2 RID: 498 RVA: 0x0000877B File Offset: 0x0000697B
		public JsonReaderException()
		{
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008783 File Offset: 0x00006983
		public JsonReaderException(string message) : base(message)
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000878C File Offset: 0x0000698C
		public JsonReaderException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008796 File Offset: 0x00006996
		internal JsonReaderException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException)
		{
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
		}
	}
}
