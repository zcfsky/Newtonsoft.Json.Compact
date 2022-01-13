using System;
using System.IO;

namespace Newtonsoft.Json
{
	// Token: 0x02000025 RID: 37
	public class JsonTextWriter : JsonWriter
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x000082C9 File Offset: 0x000064C9
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x000082D1 File Offset: 0x000064D1
		public int Indentation
		{
			get
			{
				return this._indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				this._indentation = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000082E9 File Offset: 0x000064E9
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000082F1 File Offset: 0x000064F1
		public char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				this._quoteChar = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000830F File Offset: 0x0000650F
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00008317 File Offset: 0x00006517
		public char IndentChar
		{
			get
			{
				return this._indentChar;
			}
			set
			{
				this._indentChar = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00008320 File Offset: 0x00006520
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00008328 File Offset: 0x00006528
		public bool QuoteName
		{
			get
			{
				return this._quoteName;
			}
			set
			{
				this._quoteName = value;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008331 File Offset: 0x00006531
		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			this._writer = textWriter;
			this._quoteChar = '"';
			this._quoteName = true;
			this._indentChar = ' ';
			this._indentation = 2;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000836C File Offset: 0x0000656C
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008379 File Offset: 0x00006579
		public override void Close()
		{
			base.Close();
			this._writer.Close();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000838C File Offset: 0x0000658C
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this._writer.Write("{");
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000083A4 File Offset: 0x000065A4
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this._writer.Write("[");
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000083BC File Offset: 0x000065BC
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this._writer.Write("new ");
			this._writer.Write(name);
			this._writer.Write("(");
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000083F4 File Offset: 0x000065F4
		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				this._writer.Write("}");
				return;
			case JsonToken.EndArray:
				this._writer.Write("]");
				return;
			case JsonToken.EndConstructor:
				this._writer.Write(")");
				return;
			default:
				throw new JsonWriterException("Invalid JsonToken: " + token);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008464 File Offset: 0x00006664
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			if (this._quoteName)
			{
				this._writer.Write(this._quoteChar);
			}
			this._writer.Write(name);
			if (this._quoteName)
			{
				this._writer.Write(this._quoteChar);
			}
			this._writer.Write(':');
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000084C4 File Offset: 0x000066C4
		protected override void WriteIndent()
		{
			if (base.Formatting == Formatting.Indented)
			{
				this._writer.Write(Environment.NewLine);
				int num = base.Top * this._indentation;
				for (int i = 0; i < num; i++)
				{
					this._writer.Write(this._indentChar);
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008515 File Offset: 0x00006715
		protected override void WriteValueDelimiter()
		{
			this._writer.Write(',');
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008524 File Offset: 0x00006724
		protected override void WriteIndentSpace()
		{
			this._writer.Write(' ');
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008533 File Offset: 0x00006733
		private void WriteValueInternal(string value, JsonToken token)
		{
			this._writer.Write(value);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008541 File Offset: 0x00006741
		public override void WriteNull()
		{
			base.WriteNull();
			this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008556 File Offset: 0x00006756
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000856B File Offset: 0x0000676B
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this._writer.Write(json);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008580 File Offset: 0x00006780
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.WriteValueInternal((value != null) ? JsonConvert.ToString(value, this._quoteChar) : JsonConvert.Null, JsonToken.String);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000085A7 File Offset: 0x000067A7
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000085BD File Offset: 0x000067BD
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000085D3 File Offset: 0x000067D3
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000085E9 File Offset: 0x000067E9
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000085FF File Offset: 0x000067FF
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008615 File Offset: 0x00006815
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000862B File Offset: 0x0000682B
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008642 File Offset: 0x00006842
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008658 File Offset: 0x00006858
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000866E File Offset: 0x0000686E
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008684 File Offset: 0x00006884
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000869A File Offset: 0x0000689A
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000086B0 File Offset: 0x000068B0
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000086C6 File Offset: 0x000068C6
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Date);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000086DD File Offset: 0x000068DD
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Date);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000086F4 File Offset: 0x000068F4
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this._writer.Write("/*");
			this._writer.Write(text);
			this._writer.Write("*/");
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008729 File Offset: 0x00006929
		public override void WriteWhitespace(string ws)
		{
			base.WriteWhitespace(ws);
			this._writer.Write(ws);
		}

		// Token: 0x040000A1 RID: 161
		private TextWriter _writer;

		// Token: 0x040000A2 RID: 162
		private char _indentChar;

		// Token: 0x040000A3 RID: 163
		private int _indentation;

		// Token: 0x040000A4 RID: 164
		private char _quoteChar;

		// Token: 0x040000A5 RID: 165
		private bool _quoteName;
	}
}
