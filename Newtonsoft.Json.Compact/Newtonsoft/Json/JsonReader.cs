using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200000F RID: 15
	public abstract class JsonReader : IDisposable
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000317B File Offset: 0x0000137B
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003183 File Offset: 0x00001383
		protected JsonReader.State CurrentState
		{
			get
			{
				return this._currentState;
			}
			private set
			{
				this._currentState = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000318C File Offset: 0x0000138C
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003194 File Offset: 0x00001394
		public virtual char QuoteChar
		{
			get
			{
				return this._quoteChar;
			}
			protected internal set
			{
				this._quoteChar = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000093 RID: 147 RVA: 0x0000319D File Offset: 0x0000139D
		public virtual JsonToken TokenType
		{
			get
			{
				return this._token;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000031A5 File Offset: 0x000013A5
		public virtual object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000031AD File Offset: 0x000013AD
		public virtual Type ValueType
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000031B8 File Offset: 0x000013B8
		public virtual int Depth
		{
			get
			{
				int num = this._top - 1;
				if (JsonReader.IsStartToken(this.TokenType))
				{
					return num - 1;
				}
				return num;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000031E0 File Offset: 0x000013E0
		public JsonReader()
		{
			this._currentState = JsonReader.State.Start;
			this._stack = new List<JTokenType>();
			this._top = 0;
			this.Push(JTokenType.None);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003208 File Offset: 0x00001408
		private void Push(JTokenType value)
		{
			this._stack.Add(value);
			this._top++;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003224 File Offset: 0x00001424
		private JTokenType Pop()
		{
			JTokenType result = this.Peek();
			this._stack.RemoveAt(this._stack.Count - 1);
			this._top--;
			return result;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000325F File Offset: 0x0000145F
		private JTokenType Peek()
		{
			return this._stack[this._top - 1];
		}

		// Token: 0x0600009B RID: 155
		public abstract bool Read();

		// Token: 0x0600009C RID: 156 RVA: 0x00003274 File Offset: 0x00001474
		public void Skip()
		{
			if (JsonReader.IsStartToken(this.TokenType))
			{
				int depth = this.Depth;
				while (this.Read() && depth < this.Depth)
				{
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000032A6 File Offset: 0x000014A6
		protected void SetToken(JsonToken newToken)
		{
			this.SetToken(newToken, null);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000032B0 File Offset: 0x000014B0
		protected virtual void SetToken(JsonToken newToken, object value)
		{
			this._token = newToken;
			switch (newToken)
			{
			case JsonToken.StartObject:
				this._currentState = JsonReader.State.ObjectStart;
				this.Push(JTokenType.Object);
				break;
			case JsonToken.StartArray:
				this._currentState = JsonReader.State.ArrayStart;
				this.Push(JTokenType.Array);
				break;
			case JsonToken.StartConstructor:
				this._currentState = JsonReader.State.ConstructorStart;
				this.Push(JTokenType.Constructor);
				break;
			case JsonToken.PropertyName:
				this._currentState = JsonReader.State.Property;
				this.Push(JTokenType.Property);
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndObject:
				this.ValidateEnd(JsonToken.EndObject);
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndArray:
				this.ValidateEnd(JsonToken.EndArray);
				this._currentState = JsonReader.State.PostValue;
				break;
			case JsonToken.EndConstructor:
				this.ValidateEnd(JsonToken.EndConstructor);
				this._currentState = JsonReader.State.PostValue;
				break;
			}
			JTokenType jtokenType = this.Peek();
			if (jtokenType == JTokenType.Property && this._currentState == JsonReader.State.PostValue)
			{
				this.Pop();
			}
			if (value != null)
			{
				this._value = value;
				this._valueType = value.GetType();
				return;
			}
			this._value = null;
			this._valueType = null;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000033CC File Offset: 0x000015CC
		private void ValidateEnd(JsonToken endToken)
		{
			JTokenType jtokenType = this.Pop();
			if (this.GetTypeForCloseToken(endToken) != jtokenType)
			{
				throw new JsonReaderException("JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					endToken,
					jtokenType
				}));
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000341C File Offset: 0x0000161C
		protected void SetStateBasedOnCurrent()
		{
			JTokenType jtokenType = this.Peek();
			switch (jtokenType)
			{
			case JTokenType.None:
				this._currentState = JsonReader.State.Finished;
				return;
			case JTokenType.Object:
				this._currentState = JsonReader.State.Object;
				return;
			case JTokenType.Array:
				this._currentState = JsonReader.State.Array;
				return;
			case JTokenType.Constructor:
				this._currentState = JsonReader.State.Constructor;
				return;
			default:
				throw new JsonReaderException("While setting the reader state back to current object an unexpected JsonType was encountered: " + jtokenType);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003484 File Offset: 0x00001684
		internal static bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.None:
			case JsonToken.Comment:
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
			case JsonToken.Date:
				return false;
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
			case JsonToken.PropertyName:
				return true;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected JsonToken value.");
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000034F8 File Offset: 0x000016F8
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
				throw new JsonReaderException("Not a valid close JsonToken: " + token);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003539 File Offset: 0x00001739
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003542 File Offset: 0x00001742
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonReader.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003556 File Offset: 0x00001756
		public virtual void Close()
		{
			this._currentState = JsonReader.State.Closed;
			this._token = JsonToken.None;
			this._value = null;
			this._valueType = null;
		}

		// Token: 0x04000029 RID: 41
		private JsonToken _token;

		// Token: 0x0400002A RID: 42
		private object _value;

		// Token: 0x0400002B RID: 43
		private Type _valueType;

		// Token: 0x0400002C RID: 44
		private char _quoteChar;

		// Token: 0x0400002D RID: 45
		private JsonReader.State _currentState;

		// Token: 0x0400002E RID: 46
		private int _top;

		// Token: 0x0400002F RID: 47
		private readonly List<JTokenType> _stack;

		// Token: 0x02000010 RID: 16
		protected enum State
		{
			// Token: 0x04000031 RID: 49
			Start,
			// Token: 0x04000032 RID: 50
			Complete,
			// Token: 0x04000033 RID: 51
			Property,
			// Token: 0x04000034 RID: 52
			ObjectStart,
			// Token: 0x04000035 RID: 53
			Object,
			// Token: 0x04000036 RID: 54
			ArrayStart,
			// Token: 0x04000037 RID: 55
			Array,
			// Token: 0x04000038 RID: 56
			Closed,
			// Token: 0x04000039 RID: 57
			PostValue,
			// Token: 0x0400003A RID: 58
			ConstructorStart,
			// Token: 0x0400003B RID: 59
			Constructor,
			// Token: 0x0400003C RID: 60
			Error,
			// Token: 0x0400003D RID: 61
			Finished
		}
	}
}
