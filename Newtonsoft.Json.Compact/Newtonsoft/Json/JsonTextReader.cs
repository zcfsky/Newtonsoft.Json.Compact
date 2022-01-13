using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000022 RID: 34
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00007014 File Offset: 0x00005214
		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this._reader = reader;
			this._buffer = new StringBuffer(4096);
			this._currentLineNumber = 1;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007048 File Offset: 0x00005248
		private void ParseString(char quote)
		{
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			while (!flag && this.MoveNext())
			{
				if (flag2)
				{
					num++;
				}
				char currentChar = this._currentChar;
				if (currentChar != '"' && currentChar != '\'')
				{
					if (currentChar != '\\')
					{
						goto IL_136;
					}
					if (!this.MoveNext())
					{
						throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
						{
							quote,
							this._currentLineNumber,
							this._currentLinePosition
						});
					}
					char currentChar2 = this._currentChar;
					if (currentChar2 <= 'f')
					{
						if (currentChar2 == 'b')
						{
							this._buffer.Append('\b');
							goto IL_147;
						}
						if (currentChar2 == 'f')
						{
							this._buffer.Append('\f');
							goto IL_147;
						}
					}
					else
					{
						if (currentChar2 == 'n')
						{
							this._buffer.Append('\n');
							goto IL_147;
						}
						switch (currentChar2)
						{
						case 'r':
							this._buffer.Append('\r');
							goto IL_147;
						case 't':
							this._buffer.Append('\t');
							goto IL_147;
						case 'u':
							flag2 = true;
							goto IL_147;
						}
					}
					this._buffer.Append(this._currentChar);
				}
				else
				{
					if (this._currentChar != quote)
					{
						goto IL_136;
					}
					flag = true;
				}
				IL_147:
				if (num == 4)
				{
					string text = this._buffer.ToString(this._buffer.Position - 4, 4);
					char value = Convert.ToChar(int.Parse(text, (NumberStyles)515, NumberFormatInfo.InvariantInfo));
					this._buffer.Position = this._buffer.Position - 4;
					this._buffer.Append(value);
					flag2 = false;
					num = 0;
					continue;
				}
				continue;
				IL_136:
				this._buffer.Append(this._currentChar);
				goto IL_147;
			}
			if (!flag)
			{
				throw this.CreateJsonReaderException("Unterminated string. Expected delimiter: {0}. Line {1}, position {2}.", new object[]
				{
					quote,
					this._currentLineNumber,
					this._currentLinePosition
				});
			}
			this.ClearCurrentChar();
			string text2 = this._buffer.ToString();
			this._buffer.Position = 0;
            if (text2.StartsWith("/Date(", (StringComparison)4) && text2.EndsWith(")/", (StringComparison)4))
			{
				this.ParseDate(text2);
				return;
			}
			this.SetToken(JsonToken.String, text2);
			this.QuoteChar = quote;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000072A0 File Offset: 0x000054A0
		private JsonReaderException CreateJsonReaderException(string format, params object[] args)
		{
			string message = format.FormatWith(CultureInfo.InvariantCulture, args);
			return new JsonReaderException(message, null, this._currentLineNumber, this._currentLinePosition);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000072D0 File Offset: 0x000054D0
		protected override void SetToken(JsonToken newToken, object value)
		{
			base.SetToken(newToken, value);
			switch (newToken)
			{
			case JsonToken.StartObject:
				this.ClearCurrentChar();
				return;
			case JsonToken.StartArray:
				this.ClearCurrentChar();
				return;
			case JsonToken.StartConstructor:
				this.ClearCurrentChar();
				return;
			case JsonToken.PropertyName:
				this.ClearCurrentChar();
				return;
			default:
				switch (newToken)
				{
				case JsonToken.EndObject:
					this.ClearCurrentChar();
					return;
				case JsonToken.EndArray:
					this.ClearCurrentChar();
					return;
				case JsonToken.EndConstructor:
					this.ClearCurrentChar();
					return;
				default:
					return;
				}
				break;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007348 File Offset: 0x00005548
		private void ParseDate(string text)
		{
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = (DateTimeKind)1;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			if (num != -1)
			{
				dateTimeKind = (DateTimeKind)2;
				text2 = text2.Substring(0, num);
			}
			long javaScriptTicks = long.Parse(text2, (NumberStyles)7, CultureInfo.InvariantCulture);
			DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			DateTime dateTime2;
			switch ((int)dateTimeKind)
			{
			case 0:
				dateTime2 = DateTime.SpecifyKind(dateTime.ToLocalTime(), 0);
				goto IL_86;
			case 2:
				dateTime2 = dateTime.ToLocalTime();
				goto IL_86;
			}
			dateTime2 = dateTime;
			IL_86:
			this.SetToken(JsonToken.Date, dateTime2);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000073EC File Offset: 0x000055EC
		private bool MoveNext()
		{
			int num = this._reader.Read();
			int num2 = num;
			if (num2 != -1)
			{
				if (num2 != 10)
				{
					if (num2 != 13)
					{
						this._currentLinePosition++;
						this._currentCharCarriageReturn = false;
					}
					else
					{
						this._currentLineNumber++;
						this._currentLinePosition = 0;
						this._currentCharCarriageReturn = true;
					}
				}
				else
				{
					if (!this._currentCharCarriageReturn)
					{
						this._currentLineNumber++;
					}
					this._currentLinePosition = 0;
					this._currentCharCarriageReturn = false;
				}
				this._currentChar = (char)num;
				return true;
			}
			return false;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000747B File Offset: 0x0000567B
		private bool HasNext()
		{
			return this._reader.Peek() != -1;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000748E File Offset: 0x0000568E
		private char PeekNext()
		{
			return (char)this._reader.Peek();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000749C File Offset: 0x0000569C
		private void ClearCurrentChar()
		{
			this._currentChar = '\0';
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000074A8 File Offset: 0x000056A8
		public override bool Read()
		{
			while (this._currentChar != '\0' || this.MoveNext())
			{
				switch (base.CurrentState)
				{
				case JsonReader.State.Start:
				case JsonReader.State.Property:
				case JsonReader.State.ArrayStart:
				case JsonReader.State.Array:
				case JsonReader.State.ConstructorStart:
				case JsonReader.State.Constructor:
					return this.ParseValue();
				case JsonReader.State.Complete:
				case JsonReader.State.Closed:
				case JsonReader.State.Error:
					break;
				case JsonReader.State.ObjectStart:
				case JsonReader.State.Object:
					return this.ParseObject();
				case JsonReader.State.PostValue:
					if (this.ParsePostValue())
					{
						return true;
					}
					break;
				default:
					throw this.CreateJsonReaderException("Unexpected state: {0}. Line {1}, position {2}.", new object[]
					{
						base.CurrentState,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			return false;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000755C File Offset: 0x0000575C
		private bool ParsePostValue()
		{
			for (;;)
			{
				char currentChar = this._currentChar;
				if (currentChar <= ',')
				{
					if (currentChar != ' ')
					{
						if (currentChar == ')')
						{
							goto IL_4C;
						}
						if (currentChar != ',')
						{
							goto IL_7A;
						}
						goto IL_64;
					}
					else
					{
						this.ClearCurrentChar();
					}
				}
				else
				{
					if (currentChar == '/')
					{
						goto IL_5C;
					}
					if (currentChar == ']')
					{
						goto IL_3C;
					}
					if (currentChar == '}')
					{
						break;
					}
					goto IL_7A;
				}
				IL_CD:
				if (!this.MoveNext())
				{
					return false;
				}
				continue;
				IL_7A:
				if (char.IsWhiteSpace(this._currentChar))
				{
					this.ClearCurrentChar();
					goto IL_CD;
				}
				goto IL_8F;
			}
			base.SetToken(JsonToken.EndObject);
			this.ClearCurrentChar();
			return true;
			IL_3C:
			base.SetToken(JsonToken.EndArray);
			this.ClearCurrentChar();
			return true;
			IL_4C:
			base.SetToken(JsonToken.EndConstructor);
			this.ClearCurrentChar();
			return true;
			IL_5C:
			this.ParseComment();
			return true;
			IL_64:
			base.SetStateBasedOnCurrent();
			this.ClearCurrentChar();
			return false;
			IL_8F:
			throw this.CreateJsonReaderException("After parsing a value an unexpected character was encoutered: {0}. Line {1}, position {2}.", new object[]
			{
				this._currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007644 File Offset: 0x00005844
		private bool ParseObject()
		{
			for (;;)
			{
				char currentChar = this._currentChar;
				if (currentChar <= ',')
				{
					if (currentChar != ' ')
					{
						if (currentChar != ',')
						{
							goto IL_3E;
						}
						goto IL_34;
					}
				}
				else
				{
					if (currentChar == '/')
					{
						goto IL_2C;
					}
					if (currentChar == '}')
					{
						break;
					}
					goto IL_3E;
				}
				IL_52:
				if (!this.MoveNext())
				{
					return false;
				}
				continue;
				IL_3E:
				if (!char.IsWhiteSpace(this._currentChar))
				{
					goto Block_5;
				}
				goto IL_52;
			}
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_2C:
			this.ParseComment();
			return true;
			IL_34:
			base.SetToken(JsonToken.Undefined);
			return true;
			Block_5:
			return this.ParseProperty();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000076AC File Offset: 0x000058AC
		private bool ParseProperty()
		{
			char quoteChar;
			if (this.ValidIdentifierChar(this._currentChar))
			{
				quoteChar = '\0';
				this.ParseUnquotedProperty();
			}
			else
			{
				if (this._currentChar != '"' && this._currentChar != '\'')
				{
					throw this.CreateJsonReaderException("Invalid property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						this._currentChar,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
				quoteChar = this._currentChar;
				this.ParseQuotedProperty(this._currentChar);
			}
			if (this._currentChar != ':')
			{
				this.MoveNext();
				this.EatWhitespace(false);
				if (this._currentChar != ':')
				{
					throw this.CreateJsonReaderException("Invalid character after parsing property name. Expected ':' but got: {0}. Line {1}, position {2}.", new object[]
					{
						this._currentChar,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
			}
			this.SetToken(JsonToken.PropertyName, this._buffer.ToString());
			this.QuoteChar = quoteChar;
			this._buffer.Position = 0;
			return true;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000077C0 File Offset: 0x000059C0
		private void ParseQuotedProperty(char quoteChar)
		{
			while (this.MoveNext())
			{
				if (this._currentChar == quoteChar)
				{
					return;
				}
				this._buffer.Append(this._currentChar);
			}
			throw this.CreateJsonReaderException("Unclosed quoted property. Expected: {0}. Line {1}, position {2}.", new object[]
			{
				quoteChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000782A File Offset: 0x00005A2A
		private bool ValidIdentifierChar(char value)
		{
			return char.IsLetterOrDigit(this._currentChar) || this._currentChar == '_' || this._currentChar == '$';
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007850 File Offset: 0x00005A50
		private void ParseUnquotedProperty()
		{
			this._buffer.Append(this._currentChar);
			while (this.MoveNext() && !char.IsWhiteSpace(this._currentChar))
			{
				if (this._currentChar == ':')
				{
					return;
				}
				if (!this.ValidIdentifierChar(this._currentChar))
				{
					throw this.CreateJsonReaderException("Invalid JavaScript property identifier character: {0}. Line {1}, position {2}.", new object[]
					{
						this._currentChar,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
				this._buffer.Append(this._currentChar);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000078F0 File Offset: 0x00005AF0
		private bool ParseValue()
		{
			for (;;)
			{
				char currentChar = this._currentChar;
				if (currentChar <= 'N')
				{
					if (currentChar <= '/')
					{
						switch (currentChar)
						{
						case ' ':
							break;
						case '!':
							goto IL_1EC;
						case '"':
							goto IL_C1;
						default:
							switch (currentChar)
							{
							case '\'':
								goto IL_C1;
							case '(':
							case '*':
							case '+':
							case '.':
								goto IL_1EC;
							case ')':
								goto IL_1E2;
							case ',':
								goto IL_1D8;
							case '-':
								goto IL_188;
							case '/':
								goto IL_1A2;
							default:
								goto IL_1EC;
							}
							break;
						}
					}
					else
					{
						if (currentChar == 'I')
						{
							goto IL_180;
						}
						if (currentChar != 'N')
						{
							goto IL_1EC;
						}
						goto IL_178;
					}
				}
				else if (currentChar <= 'f')
				{
					switch (currentChar)
					{
					case '[':
						goto IL_1BB;
					case '\\':
						goto IL_1EC;
					case ']':
						goto IL_1CE;
					default:
						if (currentChar != 'f')
						{
							goto IL_1EC;
						}
						goto IL_D7;
					}
				}
				else
				{
					if (currentChar == 'n')
					{
						goto IL_DF;
					}
					switch (currentChar)
					{
					case 't':
						goto IL_CF;
					case 'u':
						goto IL_1AA;
					default:
						switch (currentChar)
						{
						case '{':
							goto IL_1B2;
						case '|':
							goto IL_1EC;
						case '}':
							goto IL_1C4;
						default:
							goto IL_1EC;
						}
						break;
					}
				}
				IL_265:
				if (!this.MoveNext())
				{
					return false;
				}
				continue;
				IL_1EC:
				if (!char.IsWhiteSpace(this._currentChar))
				{
					goto Block_16;
				}
				goto IL_265;
			}
			IL_C1:
			this.ParseString(this._currentChar);
			return true;
			IL_CF:
			this.ParseTrue();
			return true;
			IL_D7:
			this.ParseFalse();
			return true;
			IL_DF:
			if (this.HasNext())
			{
				char c = this.PeekNext();
				if (c == 'u')
				{
					this.ParseNull();
				}
				else
				{
					if (c != 'e')
					{
						throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
						{
							this._currentChar,
							this._currentLineNumber,
							this._currentLinePosition
						});
					}
					this.ParseConstructor();
				}
				return true;
			}
			throw this.CreateJsonReaderException("Unexpected end. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
			IL_178:
			this.ParseNumberNaN();
			return true;
			IL_180:
			this.ParseNumberPositiveInfinity();
			return true;
			IL_188:
			if (this.PeekNext() == 'I')
			{
				this.ParseNumberNegativeInfinity();
			}
			else
			{
				this.ParseNumber();
			}
			return true;
			IL_1A2:
			this.ParseComment();
			return true;
			IL_1AA:
			this.ParseUndefined();
			return true;
			IL_1B2:
			base.SetToken(JsonToken.StartObject);
			return true;
			IL_1BB:
			base.SetToken(JsonToken.StartArray);
			return true;
			IL_1C4:
			base.SetToken(JsonToken.EndObject);
			return true;
			IL_1CE:
			base.SetToken(JsonToken.EndArray);
			return true;
			IL_1D8:
			base.SetToken(JsonToken.Undefined);
			return true;
			IL_1E2:
			base.SetToken(JsonToken.EndConstructor);
			return true;
			Block_16:
			if (char.IsNumber(this._currentChar) || this._currentChar == '-' || this._currentChar == '.')
			{
				this.ParseNumber();
				return true;
			}
			throw this.CreateJsonReaderException("Unexpected character encountered while parsing value: {0}. Line {1}, position {2}.", new object[]
			{
				this._currentChar,
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007B70 File Offset: 0x00005D70
		private bool EatWhitespace(bool oneOrMore)
		{
			bool flag = false;
			while (this._currentChar == ' ' || char.IsWhiteSpace(this._currentChar))
			{
				flag = true;
				this.MoveNext();
			}
			return !oneOrMore || flag;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007BA8 File Offset: 0x00005DA8
		private void ParseConstructor()
		{
			if (this.MatchValue("new", true) && this.EatWhitespace(true))
			{
				while (char.IsLetter(this._currentChar))
				{
					this._buffer.Append(this._currentChar);
					this.MoveNext();
				}
				this.EatWhitespace(false);
				if (this._currentChar != '(')
				{
					throw this.CreateJsonReaderException("Unexpected character while parsing constructor: {0}. Line {1}, position {2}.", new object[]
					{
						this._currentChar,
						this._currentLineNumber,
						this._currentLinePosition
					});
				}
				string value = this._buffer.ToString();
				this._buffer.Position = 0;
				this.SetToken(JsonToken.StartConstructor, value);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007C6C File Offset: 0x00005E6C
		private void ParseNumber()
		{
			bool flag = false;
			do
			{
				if (this.CurrentIsSeperator())
				{
					flag = true;
				}
				else
				{
					this._buffer.Append(this._currentChar);
				}
			}
			while (!flag && this.MoveNext());
			if (!flag)
			{
				this.ClearCurrentChar();
			}
			string text = this._buffer.ToString();
			object value;
			JsonToken newToken;
			if (text.IndexOf(".", 5) == -1 && text.IndexOf("e", 5) == -1)
			{
				value = Convert.ToInt64(this._buffer.ToString(), CultureInfo.InvariantCulture);
				newToken = JsonToken.Integer;
			}
			else
			{
				value = Convert.ToDouble(this._buffer.ToString(), CultureInfo.InvariantCulture);
				newToken = JsonToken.Float;
			}
			this._buffer.Position = 0;
			this.SetToken(newToken, value);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007D28 File Offset: 0x00005F28
		private void ParseComment()
		{
			this.MoveNext();
			if (this._currentChar == '*')
			{
				while (this.MoveNext())
				{
					if (this._currentChar == '*')
					{
						if (this.MoveNext())
						{
							if (this._currentChar == '/')
							{
                                goto IL_9A;
							}
							this._buffer.Append('*');
							this._buffer.Append(this._currentChar);
						}
					}
					else
					{
						this._buffer.Append(this._currentChar);
					}
				}

            IL_9A:
                this.SetToken(JsonToken.Comment, this._buffer.ToString());
                this._buffer.Position = 0;
                this.ClearCurrentChar();
                return;
			}

			throw this.CreateJsonReaderException("Error parsing comment. Expected: *. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007DF4 File Offset: 0x00005FF4
		private bool MatchValue(string value)
		{
			int num = 0;
			while (this._currentChar == value[num])
			{
				num++;
				if (num >= value.Length || !this.MoveNext())
				{
					break;
				}
			}
			return num == value.Length;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007E30 File Offset: 0x00006030
		private bool MatchValue(string value, bool noTrailingNonSeperatorCharacters)
		{
			bool flag = this.MatchValue(value);
			if (!noTrailingNonSeperatorCharacters)
			{
				return flag;
			}
			bool result = flag && (!this.MoveNext() || this.CurrentIsSeperator());
			if (!this.CurrentIsSeperator())
			{
				this.ClearCurrentChar();
			}
			return result;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007E74 File Offset: 0x00006074
		private bool CurrentIsSeperator()
		{
			char currentChar = this._currentChar;
			if (currentChar <= ',')
			{
				if (currentChar == ' ')
				{
					return true;
				}
				if (currentChar != ')')
				{
					if (currentChar != ',')
					{
						goto IL_5B;
					}
				}
				else
				{
					if (base.CurrentState == JsonReader.State.Constructor || base.CurrentState == JsonReader.State.ConstructorStart)
					{
						return true;
					}
					return false;
				}
			}
			else
			{
				if (currentChar == '/')
				{
					return this.HasNext() && this.PeekNext() == '*';
				}
				if (currentChar != ']' && currentChar != '}')
				{
					goto IL_5B;
				}
			}
			return true;
			IL_5B:
			if (char.IsWhiteSpace(this._currentChar))
			{
				return true;
			}
			return false;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007EEC File Offset: 0x000060EC
		private void ParseTrue()
		{
			if (this.MatchValue(JsonConvert.True, true))
			{
				this.SetToken(JsonToken.Boolean, true);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007F48 File Offset: 0x00006148
		private void ParseNull()
		{
			if (this.MatchValue(JsonConvert.Null, true))
			{
				base.SetToken(JsonToken.Null);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing null value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007F9C File Offset: 0x0000619C
		private void ParseUndefined()
		{
			if (this.MatchValue(JsonConvert.Undefined, true))
			{
				base.SetToken(JsonToken.Undefined);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing undefined value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007FF0 File Offset: 0x000061F0
		private void ParseFalse()
		{
			if (this.MatchValue(JsonConvert.False, true))
			{
				this.SetToken(JsonToken.Boolean, false);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing boolean value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000804C File Offset: 0x0000624C
		private void ParseNumberNegativeInfinity()
		{
			if (this.MatchValue(JsonConvert.NegativeInfinity, true))
			{
				this.SetToken(JsonToken.Float, double.NegativeInfinity);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing negative infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000080AC File Offset: 0x000062AC
		private void ParseNumberPositiveInfinity()
		{
			if (this.MatchValue(JsonConvert.PositiveInfinity, true))
			{
				this.SetToken(JsonToken.Float, double.PositiveInfinity);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing positive infinity value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000810C File Offset: 0x0000630C
		private void ParseNumberNaN()
		{
			if (this.MatchValue(JsonConvert.NaN, true))
			{
				this.SetToken(JsonToken.Float, double.NaN);
				return;
			}
			throw this.CreateJsonReaderException("Error parsing NaN value. Line {0}, position {1}.", new object[]
			{
				this._currentLineNumber,
				this._currentLinePosition
			});
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000816C File Offset: 0x0000636C
		public override void Close()
		{
			base.Close();
			if (this._reader != null)
			{
				this._reader.Close();
			}
			if (this._buffer != null)
			{
				this._buffer.Clear();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000819A File Offset: 0x0000639A
		public bool HasLineInfo()
		{
			return true;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000819D File Offset: 0x0000639D
		public int LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				return this._currentLineNumber;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000081AF File Offset: 0x000063AF
		public int LinePosition
		{
			get
			{
				return this._currentLinePosition;
			}
		}

		// Token: 0x04000093 RID: 147
		private const int LineFeedValue = 10;

		// Token: 0x04000094 RID: 148
		private const int CarriageReturnValue = 13;

		// Token: 0x04000095 RID: 149
		private readonly TextReader _reader;

		// Token: 0x04000096 RID: 150
		private readonly StringBuffer _buffer;

		// Token: 0x04000097 RID: 151
		private char _currentChar;

		// Token: 0x04000098 RID: 152
		private int _currentLinePosition;

		// Token: 0x04000099 RID: 153
		private int _currentLineNumber;

		// Token: 0x0400009A RID: 154
		private bool _currentCharCarriageReturn;
	}
}
