using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000026 RID: 38
	public class JsonWriterException : Exception
	{
		// Token: 0x060001EB RID: 491 RVA: 0x0000873E File Offset: 0x0000693E
		public JsonWriterException()
		{
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008746 File Offset: 0x00006946
		public JsonWriterException(string message) : base(message)
		{
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000874F File Offset: 0x0000694F
		public JsonWriterException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
