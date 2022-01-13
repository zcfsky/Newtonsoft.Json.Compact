using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000034 RID: 52
	public class JProperty : JContainer
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600037E RID: 894 RVA: 0x0000CD56 File Offset: 0x0000AF56
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0000CD5E File Offset: 0x0000AF5E
		// (set) Token: 0x06000380 RID: 896 RVA: 0x0000CD68 File Offset: 0x0000AF68
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return base.Content;
			}
			set
			{
				base.CheckReentrancy();
				JToken jtoken = value ?? new JValue((object)null);
				if (base.Content == null)
				{
					jtoken = base.EnsureParentToken(jtoken);
					base.Content = jtoken;
					base.Content.Parent = this;
					base.Content.Next = base.Content;
					return;
				}
				base.Content.Replace(jtoken);
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000CDC8 File Offset: 0x0000AFC8
		internal override void ReplaceItem(JToken existing, JToken replacement)
		{
			if (JContainer.IsTokenUnchanged(existing, replacement))
			{
				return;
			}
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanging(this);
			}
			base.ReplaceItem(existing, replacement);
			if (base.Parent != null)
			{
				((JObject)base.Parent).InternalPropertyChanged(this);
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000CE19 File Offset: 0x0000B019
		public JProperty(JProperty other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000CE30 File Offset: 0x0000B030
		internal override void AddItem(bool isLast, JToken previous, JToken item)
		{
			if (this.Value != null)
			{
				throw new Exception("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeof(JProperty)
				}));
			}
			this.Value = item;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000CE76 File Offset: 0x0000B076
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000CE87 File Offset: 0x0000B087
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			this.Value = item;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000CE9C File Offset: 0x0000B09C
		internal override bool RemoveItem(JToken item)
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		internal override void RemoveItemAt(int index)
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000CF0C File Offset: 0x0000B10C
		internal override void InsertItem(int index, JToken item)
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000CF42 File Offset: 0x0000B142
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000CF50 File Offset: 0x0000B150
		internal override void ClearItems()
		{
			throw new Exception("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(JProperty)
			}));
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000CF86 File Offset: 0x0000B186
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenInternal());
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000D06C File Offset: 0x0000B26C
		private IEnumerable<JToken> ChildrenInternal()
		{
			yield return this.Value;
			yield break;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000D08C File Offset: 0x0000B28C
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000D0BF File Offset: 0x0000B2BF
		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000D0C7 File Offset: 0x0000B2C7
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000D0CA File Offset: 0x0000B2CA
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public JProperty(string name, params object[] content) : this(name, (object)content)
		{
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000D0EE File Offset: 0x0000B2EE
		public JProperty(string name, object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : base.CreateFromContent(content));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000D126 File Offset: 0x0000B326
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			this.Value.WriteTo(writer, converters);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000D141 File Offset: 0x0000B341
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ ((this.Value != null) ? this.Value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000D168 File Offset: 0x0000B368
		public static JProperty Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JProperty from JsonReader.");
			}
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw new Exception("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo);
			if (!reader.Read())
			{
				throw new Exception("Error reading JProperty from JsonReader.");
			}
			jproperty.ReadContentFrom(reader);
			return jproperty;
		}

		// Token: 0x040000D0 RID: 208
		private readonly string _name;
	}
}
