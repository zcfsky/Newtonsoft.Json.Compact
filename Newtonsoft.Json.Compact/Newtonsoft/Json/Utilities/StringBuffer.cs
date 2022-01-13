using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200007A RID: 122
	internal class StringBuffer
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00016C2A File Offset: 0x00014E2A
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x00016C32 File Offset: 0x00014E32
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00016C3B File Offset: 0x00014E3B
		public StringBuffer()
		{
			this._buffer = StringBuffer._emptyBuffer;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00016C4E File Offset: 0x00014E4E
		public StringBuffer(int initalSize)
		{
			this._buffer = new char[initalSize];
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00016C64 File Offset: 0x00014E64
		public void Append(char value)
		{
			if (this._position + 1 > this._buffer.Length)
			{
				this.EnsureSize(1);
			}
			this._buffer[this._position++] = value;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00016CA3 File Offset: 0x00014EA3
		public void Clear()
		{
			this._buffer = StringBuffer._emptyBuffer;
			this._position = 0;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00016CB8 File Offset: 0x00014EB8
		private void EnsureSize(int appendLength)
		{
			char[] array = new char[(this._position + appendLength) * 2];
			Array.Copy(this._buffer, array, this._position);
			this._buffer = array;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00016CEE File Offset: 0x00014EEE
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00016CFD File Offset: 0x00014EFD
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x040001DE RID: 478
		private char[] _buffer;

		// Token: 0x040001DF RID: 479
		private int _position;

		// Token: 0x040001E0 RID: 480
		private static readonly char[] _emptyBuffer = new char[0];
	}
}
