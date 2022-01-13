using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005B RID: 91
	internal abstract class JsonSerializerInternalBase
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x000111B4 File Offset: 0x0000F3B4
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x000111BC File Offset: 0x0000F3BC
		internal JsonSerializer Serializer { get; private set; }

		// Token: 0x060004ED RID: 1261 RVA: 0x000111C5 File Offset: 0x0000F3C5
		protected JsonSerializerInternalBase(JsonSerializer serializer)
		{
			ValidationUtils.ArgumentNotNull(serializer, "serializer");
			this.Serializer = serializer;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x000111DF File Offset: 0x0000F3DF
		protected ErrorContext GetErrorContext(object currentObject, object member, Exception error)
		{
			if (this._currentErrorContext == null)
			{
				this._currentErrorContext = new ErrorContext(currentObject, member, error);
			}
			if (this._currentErrorContext.Error != error)
			{
				throw new InvalidOperationException("Current error context error is different to requested error.");
			}
			return this._currentErrorContext;
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00011216 File Offset: 0x0000F416
		protected void ClearErrorContext()
		{
			if (this._currentErrorContext == null)
			{
				throw new InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			this._currentErrorContext = null;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00011234 File Offset: 0x0000F434
		protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, Exception ex)
		{
			ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, ex);
			contract.InvokeOnError(currentObject, errorContext);
			if (!errorContext.Handled)
			{
				this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}

		// Token: 0x04000186 RID: 390
		private ErrorContext _currentErrorContext;
	}
}
