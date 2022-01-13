using System;

namespace Newtonsoft.Json
{
	// Token: 0x0200002B RID: 43
	public class JsonSerializationException : Exception
	{
		// Token: 0x0600022A RID: 554 RVA: 0x00009047 File Offset: 0x00007247
		public JsonSerializationException()
		{
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000904F File Offset: 0x0000724F
		public JsonSerializationException(string message) : base(message)
		{
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009058 File Offset: 0x00007258
		public JsonSerializationException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
