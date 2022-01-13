using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000017 RID: 23
	public class JTokenReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x00004748 File Offset: 0x00002948
		public JTokenReader(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			this._root = token;
			this._current = token;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000476C File Offset: 0x0000296C
		public override bool Read()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				this.SetToken(this._current);
				return true;
			}
			JContainer jcontainer = this._current as JContainer;
			if (jcontainer != null && this._parent != jcontainer)
			{
				return this.ReadInto(jcontainer);
			}
			return this.ReadOver(this._current);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000047BC File Offset: 0x000029BC
		private bool ReadOver(JToken t)
		{
			if (t == this._root)
			{
				return this.ReadToEnd();
			}
			JToken next = t.Next;
			if (next != null && next != t && t != t.Parent.Last)
			{
				this._current = next;
				this.SetToken(this._current);
				return true;
			}
			if (t.Parent == null)
			{
				return this.ReadToEnd();
			}
			return this.SetEnd(t.Parent);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004825 File Offset: 0x00002A25
		private bool ReadToEnd()
		{
			return false;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004828 File Offset: 0x00002A28
		private bool IsEndElement
		{
			get
			{
				return this._current == this._parent;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004838 File Offset: 0x00002A38
		private JsonToken? GetEndToken(JContainer c)
		{
			switch (c.Type)
			{
			case JTokenType.Object:
				return new JsonToken?(JsonToken.EndObject);
			case JTokenType.Array:
				return new JsonToken?(JsonToken.EndArray);
			case JTokenType.Constructor:
				return new JsonToken?(JsonToken.EndConstructor);
			case JTokenType.Property:
				return default(JsonToken?);
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type.");
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000048A4 File Offset: 0x00002AA4
		private bool ReadInto(JContainer c)
		{
			JToken first = c.First;
			if (first == null)
			{
				return this.SetEnd(c);
			}
			this.SetToken(first);
			this._current = first;
			this._parent = c;
			return true;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000048DC File Offset: 0x00002ADC
		private bool SetEnd(JContainer c)
		{
			JsonToken? endToken = this.GetEndToken(c);
			if (endToken != null)
			{
				base.SetToken(endToken.Value);
				this._current = c;
				this._parent = c;
				return true;
			}
			return this.ReadOver(c);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004920 File Offset: 0x00002B20
		private void SetToken(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Object:
				base.SetToken(JsonToken.StartObject);
				return;
			case JTokenType.Array:
				base.SetToken(JsonToken.StartArray);
				return;
			case JTokenType.Constructor:
				base.SetToken(JsonToken.StartConstructor);
				return;
			case JTokenType.Property:
				this.SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
				return;
			case JTokenType.Comment:
				this.SetToken(JsonToken.Comment, ((JValue)token).Value);
				return;
			case JTokenType.Integer:
				this.SetToken(JsonToken.Integer, ((JValue)token).Value);
				return;
			case JTokenType.Float:
				this.SetToken(JsonToken.Float, ((JValue)token).Value);
				return;
			case JTokenType.String:
				this.SetToken(JsonToken.String, ((JValue)token).Value);
				return;
			case JTokenType.Boolean:
				this.SetToken(JsonToken.Boolean, ((JValue)token).Value);
				return;
			case JTokenType.Null:
				this.SetToken(JsonToken.Null, ((JValue)token).Value);
				return;
			case JTokenType.Undefined:
				this.SetToken(JsonToken.Undefined, ((JValue)token).Value);
				return;
			case JTokenType.Date:
				this.SetToken(JsonToken.Date, ((JValue)token).Value);
				return;
			case JTokenType.Raw:
				this.SetToken(JsonToken.Raw, ((JValue)token).Value);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004A6C File Offset: 0x00002C6C
		bool IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == JsonReader.State.Start)
			{
				return false;
			}
			IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004AA0 File Offset: 0x00002CA0
		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LineNumber;
				}
				return 0;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004AD4 File Offset: 0x00002CD4
		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == JsonReader.State.Start)
				{
					return 0;
				}
				IJsonLineInfo jsonLineInfo = this.IsEndElement ? null : this._current;
				if (jsonLineInfo != null)
				{
					return jsonLineInfo.LinePosition;
				}
				return 0;
			}
		}

		// Token: 0x04000052 RID: 82
		private readonly JToken _root;

		// Token: 0x04000053 RID: 83
		private JToken _parent;

		// Token: 0x04000054 RID: 84
		private JToken _current;
	}
}
