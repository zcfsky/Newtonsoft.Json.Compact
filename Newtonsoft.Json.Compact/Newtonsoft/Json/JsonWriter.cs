using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000019 RID: 25
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004B08 File Offset: 0x00002D08
		protected internal int Top
		{
			get
			{
				return this._top;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004B10 File Offset: 0x00002D10
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				default:
					throw new JsonWriterException("Invalid state: " + this._currentState);
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00004B7C File Offset: 0x00002D7C
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00004B84 File Offset: 0x00002D84
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				this._formatting = value;
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004B8D File Offset: 0x00002D8D
		public JsonWriter()
		{
			this._stack = new List<JTokenType>(8);
			this._stack.Add(JTokenType.None);
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004BBC File Offset: 0x00002DBC
		private void Push(JTokenType value)
		{
			this._top++;
			if (this._stack.Count <= this._top)
			{
				this._stack.Add(value);
				return;
			}
			this._stack[this._top] = value;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004C0C File Offset: 0x00002E0C
		private JTokenType Pop()
		{
			JTokenType result = this.Peek();
			this._top--;
			return result;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004C2F File Offset: 0x00002E2F
		private JTokenType Peek()
		{
			return this._stack[this._top];
		}

		// Token: 0x0600010B RID: 267
		public abstract void Flush();

		// Token: 0x0600010C RID: 268 RVA: 0x00004C42 File Offset: 0x00002E42
		public virtual void Close()
		{
			this.AutoCompleteAll();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004C4A File Offset: 0x00002E4A
		public virtual void WriteStartObject()
		{
			this.AutoComplete(JsonToken.StartObject);
			this.Push(JTokenType.Object);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004C5A File Offset: 0x00002E5A
		public void WriteEndObject()
		{
			this.AutoCompleteClose(JsonToken.EndObject);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004C64 File Offset: 0x00002E64
		public virtual void WriteStartArray()
		{
			this.AutoComplete(JsonToken.StartArray);
			this.Push(JTokenType.Array);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004C74 File Offset: 0x00002E74
		public void WriteEndArray()
		{
			this.AutoCompleteClose(JsonToken.EndArray);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004C7E File Offset: 0x00002E7E
		public virtual void WriteStartConstructor(string name)
		{
			this.AutoComplete(JsonToken.StartConstructor);
			this.Push(JTokenType.Constructor);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004C8E File Offset: 0x00002E8E
		public void WriteEndConstructor()
		{
			this.AutoCompleteClose(JsonToken.EndConstructor);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004C98 File Offset: 0x00002E98
		public virtual void WritePropertyName(string name)
		{
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004CA1 File Offset: 0x00002EA1
		public void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public void WriteToken(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			int initialDepth;
			if (reader.TokenType == JsonToken.None)
			{
				initialDepth = -1;
			}
			else if (!this.IsStartToken(reader.TokenType))
			{
				initialDepth = reader.Depth + 1;
			}
			else
			{
				initialDepth = reader.Depth;
			}
			this.WriteToken(reader, initialDepth);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004CFC File Offset: 0x00002EFC
		internal void WriteToken(JsonReader reader, int initialDepth)
		{
			for (;;)
			{
				switch (reader.TokenType)
				{
				case JsonToken.None:
					goto IL_18F;
				case JsonToken.StartObject:
					this.WriteStartObject();
					goto IL_18F;
				case JsonToken.StartArray:
					this.WriteStartArray();
					goto IL_18F;
				case JsonToken.StartConstructor:
				{
					string text = reader.Value.ToString();
					if (string.Compare(text, "Date", (StringComparison)4) == 0)
					{
						this.WriteConstructorDate(reader);
						goto IL_18F;
					}
					this.WriteStartConstructor(reader.Value.ToString());
					goto IL_18F;
				}
				case JsonToken.PropertyName:
					this.WritePropertyName(reader.Value.ToString());
					goto IL_18F;
				case JsonToken.Comment:
					this.WriteComment(reader.Value.ToString());
					goto IL_18F;
				case JsonToken.Raw:
					this.WriteRawValue((string)reader.Value);
					goto IL_18F;
				case JsonToken.Integer:
					this.WriteValue((long)reader.Value);
					goto IL_18F;
				case JsonToken.Float:
					this.WriteValue((double)reader.Value);
					goto IL_18F;
				case JsonToken.String:
					this.WriteValue(reader.Value.ToString());
					goto IL_18F;
				case JsonToken.Boolean:
					this.WriteValue((bool)reader.Value);
					goto IL_18F;
				case JsonToken.Null:
					this.WriteNull();
					goto IL_18F;
				case JsonToken.Undefined:
					this.WriteUndefined();
					goto IL_18F;
				case JsonToken.EndObject:
					this.WriteEndObject();
					goto IL_18F;
				case JsonToken.EndArray:
					this.WriteEndArray();
					goto IL_18F;
				case JsonToken.EndConstructor:
					this.WriteEndConstructor();
					goto IL_18F;
				case JsonToken.Date:
					this.WriteValue((DateTime)reader.Value);
					goto IL_18F;
				}
				break;
				IL_18F:
				if (initialDepth - 1 >= reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0) || !reader.Read())
				{
					return;
				}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", reader.TokenType, "Unexpected token type.");
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004EC4 File Offset: 0x000030C4
		private void WriteConstructorDate(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new Exception("Unexpected end while reading date constructor.");
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw new Exception("Unexpected token while reading date constructor. Expected Integer, got " + reader.TokenType);
			}
			long javaScriptTicks = (long)reader.Value;
			DateTime value = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (!reader.Read())
			{
				throw new Exception("Unexpected end while reading date constructor.");
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw new Exception("Unexpected token while reading date constructor. Expected EndConstructor, got " + reader.TokenType);
			}
			this.WriteValue(value);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004F5C File Offset: 0x0000315C
		private bool IsEndToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004F88 File Offset: 0x00003188
		private bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004FB0 File Offset: 0x000031B0
		private void WriteEnd(JTokenType type)
		{
			switch (type)
			{
			case JTokenType.Object:
				this.WriteEndObject();
				return;
			case JTokenType.Array:
				this.WriteEndArray();
				return;
			case JTokenType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw new JsonWriterException("Unexpected type when writing end: " + type);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004FFF File Offset: 0x000031FF
		private void AutoCompleteAll()
		{
			while (this._top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005014 File Offset: 0x00003214
		private JTokenType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JTokenType.Object;
			case JsonToken.EndArray:
				return JTokenType.Array;
			case JsonToken.EndConstructor:
				return JTokenType.Constructor;
			default:
				throw new JsonWriterException("No type for token: " + token);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005058 File Offset: 0x00003258
		private JsonToken GetCloseTokenForType(JTokenType type)
		{
			switch (type)
			{
			case JTokenType.Object:
				return JsonToken.EndObject;
			case JTokenType.Array:
				return JsonToken.EndArray;
			case JTokenType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw new JsonWriterException("No close token for type: " + type);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000509C File Offset: 0x0000329C
		private void AutoCompleteClose(JsonToken tokenBeingClosed)
		{
			int num = 0;
			for (int i = 0; i < this._top; i++)
			{
				int num2 = this._top - i;
				if (this._stack[num2] == this.GetTypeForCloseToken(tokenBeingClosed))
				{
					num = i + 1;
					break;
				}
			}
			if (num == 0)
			{
				throw new JsonWriterException("No token to close.");
			}
			for (int j = 0; j < num; j++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
			}
			JTokenType jtokenType = this.Peek();
			switch (jtokenType)
			{
			case JTokenType.None:
				this._currentState = JsonWriter.State.Start;
				return;
			case JTokenType.Object:
				this._currentState = JsonWriter.State.Object;
				return;
			case JTokenType.Array:
				this._currentState = JsonWriter.State.Array;
				return;
			case JTokenType.Constructor:
				this._currentState = JsonWriter.State.Array;
				return;
			default:
				throw new JsonWriterException("Unknown JsonType: " + jtokenType);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005183 File Offset: 0x00003383
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005185 File Offset: 0x00003385
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005187 File Offset: 0x00003387
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005189 File Offset: 0x00003389
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000518C File Offset: 0x0000338C
		private void AutoComplete(JsonToken tokenBeingWritten)
		{
			int num;
			switch (tokenBeingWritten)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
				num = 7;
				break;
			default:
				num = (int)tokenBeingWritten;
				break;
			}
			JsonWriter.State state = JsonWriter.stateArray[num, (int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw new JsonWriterException("Token {0} in state {1} would result in an invalid JavaScript object.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					tokenBeingWritten.ToString(),
					this._currentState.ToString()
				}));
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			else if (this._currentState == JsonWriter.State.Property && this._formatting == Formatting.Indented)
			{
				this.WriteIndentSpace();
			}
			if ((tokenBeingWritten == JsonToken.PropertyName && this.WriteState != WriteState.Start) || this.WriteState == WriteState.Array || this.WriteState == WriteState.Constructor)
			{
				this.WriteIndent();
			}
			this._currentState = state;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005291 File Offset: 0x00003491
		public virtual void WriteNull()
		{
			this.AutoComplete(JsonToken.Null);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000529B File Offset: 0x0000349B
		public virtual void WriteUndefined()
		{
			this.AutoComplete(JsonToken.Undefined);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000052A5 File Offset: 0x000034A5
		public virtual void WriteRaw(string json)
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000052A7 File Offset: 0x000034A7
		public virtual void WriteRawValue(string json)
		{
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000052B8 File Offset: 0x000034B8
		public virtual void WriteValue(string value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000052C2 File Offset: 0x000034C2
		public virtual void WriteValue(int value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000052CB File Offset: 0x000034CB
		public virtual void WriteValue(uint value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000052D4 File Offset: 0x000034D4
		public virtual void WriteValue(long value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000052DD File Offset: 0x000034DD
		public virtual void WriteValue(ulong value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000052E6 File Offset: 0x000034E6
		public virtual void WriteValue(float value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000052EF File Offset: 0x000034EF
		public virtual void WriteValue(double value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000052F8 File Offset: 0x000034F8
		public virtual void WriteValue(bool value)
		{
			this.AutoComplete(JsonToken.Boolean);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005302 File Offset: 0x00003502
		public virtual void WriteValue(short value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000530B File Offset: 0x0000350B
		public virtual void WriteValue(ushort value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005314 File Offset: 0x00003514
		public virtual void WriteValue(char value)
		{
			this.AutoComplete(JsonToken.String);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000531E File Offset: 0x0000351E
		public virtual void WriteValue(byte value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005327 File Offset: 0x00003527
		public virtual void WriteValue(sbyte value)
		{
			this.AutoComplete(JsonToken.Integer);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005330 File Offset: 0x00003530
		public virtual void WriteValue(decimal value)
		{
			this.AutoComplete(JsonToken.Float);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005339 File Offset: 0x00003539
		public virtual void WriteValue(DateTime value)
		{
			this.AutoComplete(JsonToken.Date);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005343 File Offset: 0x00003543
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.AutoComplete(JsonToken.Date);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000534D File Offset: 0x0000354D
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000536C File Offset: 0x0000356C
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000538B File Offset: 0x0000358B
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000053AA File Offset: 0x000035AA
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000053C9 File Offset: 0x000035C9
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000053E8 File Offset: 0x000035E8
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005407 File Offset: 0x00003607
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005428 File Offset: 0x00003628
		public virtual void WriteValue(short? value)
		{
			short? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : default(int?);
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005478 File Offset: 0x00003678
		public virtual void WriteValue(ushort? value)
		{
			ushort? num = value;
			int? num2 = (num != null) ? new int?((int)num.GetValueOrDefault()) : default(int?);
			if (num2 == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000054C8 File Offset: 0x000036C8
		public virtual void WriteValue(char? value)
		{
			char? c = value;
			int? num = (c != null) ? new int?((int)c.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005518 File Offset: 0x00003718
		public virtual void WriteValue(byte? value)
		{
			byte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005568 File Offset: 0x00003768
		public virtual void WriteValue(sbyte? value)
		{
			sbyte? b = value;
			int? num = (b != null) ? new int?((int)b.GetValueOrDefault()) : default(int?);
			if (num == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000055B5 File Offset: 0x000037B5
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000055D4 File Offset: 0x000037D4
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000055F3 File Offset: 0x000037F3
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.Value);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005614 File Offset: 0x00003814
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is IConvertible)
			{
				IConvertible convertible = value as IConvertible;
				switch ((int)convertible.GetTypeCode())
				{
				case 2:
					this.WriteNull();
					return;
				case 3:
					this.WriteValue(convertible.ToBoolean(CultureInfo.InvariantCulture));
					return;
				case 4:
					this.WriteValue(convertible.ToChar(CultureInfo.InvariantCulture));
					return;
				case 5:
					this.WriteValue(convertible.ToSByte(CultureInfo.InvariantCulture));
					return;
				case 6:
					this.WriteValue(convertible.ToByte(CultureInfo.InvariantCulture));
					return;
				case 7:
					this.WriteValue(convertible.ToInt16(CultureInfo.InvariantCulture));
					return;
				case 8:
					this.WriteValue(convertible.ToUInt16(CultureInfo.InvariantCulture));
					return;
				case 9:
					this.WriteValue(convertible.ToInt32(CultureInfo.InvariantCulture));
					return;
				case 10:
					this.WriteValue(convertible.ToUInt32(CultureInfo.InvariantCulture));
					return;
				case 11:
					this.WriteValue(convertible.ToInt64(CultureInfo.InvariantCulture));
					return;
				case 12:
					this.WriteValue(convertible.ToUInt64(CultureInfo.InvariantCulture));
					return;
				case 13:
					this.WriteValue(convertible.ToSingle(CultureInfo.InvariantCulture));
					return;
				case 14:
					this.WriteValue(convertible.ToDouble(CultureInfo.InvariantCulture));
					return;
				case 15:
					this.WriteValue(convertible.ToDecimal(CultureInfo.InvariantCulture));
					return;
				case 16:
					this.WriteValue(convertible.ToDateTime(CultureInfo.InvariantCulture));
					return;
				case 18:
					this.WriteValue(convertible.ToString(CultureInfo.InvariantCulture));
					return;
				}
			}
			else if (value is DateTimeOffset)
			{
				this.WriteValue((DateTimeOffset)value);
				return;
			}
			throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				value.GetType()
			}));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000057E4 File Offset: 0x000039E4
		public virtual void WriteComment(string text)
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000057ED File Offset: 0x000039ED
		public virtual void WriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw new JsonWriterException("Only white space characters should be used.");
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005805 File Offset: 0x00003A05
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000580E File Offset: 0x00003A0E
		private void Dispose(bool disposing)
		{
			if (this.WriteState != WriteState.Closed)
			{
				this.Close();
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005820 File Offset: 0x00003A20
		// Note: this type is marked as 'beforefieldinit'.
		static JsonWriter()
		{
			JsonWriter.State[,] array = new JsonWriter.State[8, 10];
			array[0, 0] = JsonWriter.State.Error;
			array[0, 1] = JsonWriter.State.Error;
			array[0, 2] = JsonWriter.State.Error;
			array[0, 3] = JsonWriter.State.Error;
			array[0, 4] = JsonWriter.State.Error;
			array[0, 5] = JsonWriter.State.Error;
			array[0, 6] = JsonWriter.State.Error;
			array[0, 7] = JsonWriter.State.Error;
			array[0, 8] = JsonWriter.State.Error;
			array[0, 9] = JsonWriter.State.Error;
			array[1, 0] = JsonWriter.State.ObjectStart;
			array[1, 1] = JsonWriter.State.ObjectStart;
			array[1, 2] = JsonWriter.State.Error;
			array[1, 3] = JsonWriter.State.Error;
			array[1, 4] = JsonWriter.State.ObjectStart;
			array[1, 5] = JsonWriter.State.ObjectStart;
			array[1, 6] = JsonWriter.State.ObjectStart;
			array[1, 7] = JsonWriter.State.ObjectStart;
			array[1, 8] = JsonWriter.State.Error;
			array[1, 9] = JsonWriter.State.Error;
			array[2, 0] = JsonWriter.State.ArrayStart;
			array[2, 1] = JsonWriter.State.ArrayStart;
			array[2, 2] = JsonWriter.State.Error;
			array[2, 3] = JsonWriter.State.Error;
			array[2, 4] = JsonWriter.State.ArrayStart;
			array[2, 5] = JsonWriter.State.ArrayStart;
			array[2, 6] = JsonWriter.State.ArrayStart;
			array[2, 7] = JsonWriter.State.ArrayStart;
			array[2, 8] = JsonWriter.State.Error;
			array[2, 9] = JsonWriter.State.Error;
			array[3, 0] = JsonWriter.State.ConstructorStart;
			array[3, 1] = JsonWriter.State.ConstructorStart;
			array[3, 2] = JsonWriter.State.Error;
			array[3, 3] = JsonWriter.State.Error;
			array[3, 4] = JsonWriter.State.ConstructorStart;
			array[3, 5] = JsonWriter.State.ConstructorStart;
			array[3, 6] = JsonWriter.State.ConstructorStart;
			array[3, 7] = JsonWriter.State.ConstructorStart;
			array[3, 8] = JsonWriter.State.Error;
			array[3, 9] = JsonWriter.State.Error;
			array[4, 0] = JsonWriter.State.Property;
			array[4, 1] = JsonWriter.State.Error;
			array[4, 2] = JsonWriter.State.Property;
			array[4, 3] = JsonWriter.State.Property;
			array[4, 4] = JsonWriter.State.Error;
			array[4, 5] = JsonWriter.State.Error;
			array[4, 6] = JsonWriter.State.Error;
			array[4, 7] = JsonWriter.State.Error;
			array[4, 8] = JsonWriter.State.Error;
			array[4, 9] = JsonWriter.State.Error;
			array[5, 1] = JsonWriter.State.Property;
			array[5, 2] = JsonWriter.State.ObjectStart;
			array[5, 3] = JsonWriter.State.Object;
			array[5, 4] = JsonWriter.State.ArrayStart;
			array[5, 5] = JsonWriter.State.Array;
			array[5, 6] = JsonWriter.State.Constructor;
			array[5, 7] = JsonWriter.State.Constructor;
			array[5, 8] = JsonWriter.State.Error;
			array[5, 9] = JsonWriter.State.Error;
			array[6, 1] = JsonWriter.State.Property;
			array[6, 2] = JsonWriter.State.ObjectStart;
			array[6, 3] = JsonWriter.State.Object;
			array[6, 4] = JsonWriter.State.ArrayStart;
			array[6, 5] = JsonWriter.State.Array;
			array[6, 6] = JsonWriter.State.Constructor;
			array[6, 7] = JsonWriter.State.Constructor;
			array[6, 8] = JsonWriter.State.Error;
			array[6, 9] = JsonWriter.State.Error;
			array[7, 1] = JsonWriter.State.Object;
			array[7, 2] = JsonWriter.State.Error;
			array[7, 3] = JsonWriter.State.Error;
			array[7, 4] = JsonWriter.State.Array;
			array[7, 5] = JsonWriter.State.Array;
			array[7, 6] = JsonWriter.State.Constructor;
			array[7, 7] = JsonWriter.State.Constructor;
			array[7, 8] = JsonWriter.State.Error;
			array[7, 9] = JsonWriter.State.Error;
			JsonWriter.stateArray = array;
		}

		// Token: 0x04000064 RID: 100
		private static readonly JsonWriter.State[,] stateArray;

		// Token: 0x04000065 RID: 101
		private int _top;

		// Token: 0x04000066 RID: 102
		private readonly List<JTokenType> _stack;

		// Token: 0x04000067 RID: 103
		private JsonWriter.State _currentState;

		// Token: 0x04000068 RID: 104
		private Formatting _formatting;

		// Token: 0x0200001A RID: 26
		private enum State
		{
			// Token: 0x0400006A RID: 106
			Start,
			// Token: 0x0400006B RID: 107
			Property,
			// Token: 0x0400006C RID: 108
			ObjectStart,
			// Token: 0x0400006D RID: 109
			Object,
			// Token: 0x0400006E RID: 110
			ArrayStart,
			// Token: 0x0400006F RID: 111
			Array,
			// Token: 0x04000070 RID: 112
			ConstructorStart,
			// Token: 0x04000071 RID: 113
			Constructor,
			// Token: 0x04000072 RID: 114
			Closed,
			// Token: 0x04000073 RID: 115
			Error
		}
	}
}
