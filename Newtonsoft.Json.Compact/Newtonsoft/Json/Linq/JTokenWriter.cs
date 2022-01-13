using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200001B RID: 27
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00005B1E File Offset: 0x00003D1E
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005B35 File Offset: 0x00003D35
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005B56 File Offset: 0x00003D56
		public JTokenWriter()
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005B5E File Offset: 0x00003D5E
		public override void Flush()
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005B60 File Offset: 0x00003D60
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005B68 File Offset: 0x00003D68
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005B7B File Offset: 0x00003D7B
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.Add(container);
			}
			this._parent = container;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005BA1 File Offset: 0x00003DA1
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005BDB File Offset: 0x00003DDB
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005BEE File Offset: 0x00003DEE
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005C03 File Offset: 0x00003E03
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005C0B File Offset: 0x00003E0B
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this.AddParent(new JProperty(name));
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005C20 File Offset: 0x00003E20
		private void AddValue(object value, JsonToken token)
		{
			this.AddValue(new JValue(value), token);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C2F File Offset: 0x00003E2F
		private void AddValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				this._parent.Add(value);
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					return;
				}
			}
			else
			{
				this._value = value;
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005C6C File Offset: 0x00003E6C
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, JsonToken.Null);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005C7D File Offset: 0x00003E7D
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, JsonToken.Undefined);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005C8E File Offset: 0x00003E8E
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddValue(JValue.CreateRaw(json), JsonToken.Raw);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddValue(value ?? string.Empty, JsonToken.String);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005CBF File Offset: 0x00003EBF
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005CD5 File Offset: 0x00003ED5
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005CEB File Offset: 0x00003EEB
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005D01 File Offset: 0x00003F01
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005D17 File Offset: 0x00003F17
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005D2D File Offset: 0x00003F2D
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005D43 File Offset: 0x00003F43
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Boolean);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005D5A File Offset: 0x00003F5A
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005D70 File Offset: 0x00003F70
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005D86 File Offset: 0x00003F86
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.AddValue(value.ToString(), JsonToken.String);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005D9E File Offset: 0x00003F9E
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Integer);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005DCA File Offset: 0x00003FCA
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Float);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005DE0 File Offset: 0x00003FE0
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005DF7 File Offset: 0x00003FF7
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, JsonToken.Date);
		}

		// Token: 0x04000074 RID: 116
		private JContainer _token;

		// Token: 0x04000075 RID: 117
		private JContainer _parent;

		// Token: 0x04000076 RID: 118
		private JValue _value;
	}
}
