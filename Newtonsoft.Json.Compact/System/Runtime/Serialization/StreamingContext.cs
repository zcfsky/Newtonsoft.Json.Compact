using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000062 RID: 98
	public struct StreamingContext
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x0001318F File Offset: 0x0001138F
		public StreamingContext(StreamingContextStates state)
		{
			this = new StreamingContext(state, null);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00013199 File Offset: 0x00011399
		public StreamingContext(StreamingContextStates state, object additional)
		{
			this.m_state = state;
			this.m_additionalContext = additional;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x000131A9 File Offset: 0x000113A9
		public object Context
		{
			get
			{
				return this.m_additionalContext;
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000131B1 File Offset: 0x000113B1
		public override bool Equals(object obj)
		{
			return obj is StreamingContext && ((StreamingContext)obj).m_additionalContext == this.m_additionalContext && ((StreamingContext)obj).m_state == this.m_state;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000131E5 File Offset: 0x000113E5
		public override int GetHashCode()
		{
			return (int)this.m_state;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000131ED File Offset: 0x000113ED
		public StreamingContextStates State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x040001A6 RID: 422
		internal object m_additionalContext;

		// Token: 0x040001A7 RID: 423
		internal StreamingContextStates m_state;
	}
}
