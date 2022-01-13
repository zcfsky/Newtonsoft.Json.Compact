using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000030 RID: 48
	public class JConstructor : JContainer
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000BFF1 File Offset: 0x0000A1F1
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000BFF9 File Offset: 0x0000A1F9
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000C002 File Offset: 0x0000A202
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000C005 File Offset: 0x0000A205
		public JConstructor()
		{
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000C00D File Offset: 0x0000A20D
		public JConstructor(JConstructor other) : base(other)
		{
			this._name = other.Name;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000C022 File Offset: 0x0000A222
		public JConstructor(string name, params object[] content) : this(name, (object)content)
		{
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000C02C File Offset: 0x0000A22C
		public JConstructor(string name, object content) : this(name)
		{
			base.Add(content);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C03C File Offset: 0x0000A23C
		public JConstructor(string name)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
			this._name = name;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000C058 File Offset: 0x0000A258
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C08B File Offset: 0x0000A28B
		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C094 File Offset: 0x0000A294
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			foreach (JToken jtoken in this.Children())
			{
				jtoken.WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x17000091 RID: 145
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this.GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C19F File Offset: 0x0000A39F
		internal override int GetDeepHashCode()
		{
			return this._name.GetHashCode() ^ base.ContentsHashCode();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
		public static JConstructor Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JConstructor from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw new Exception("Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo);
			if (!reader.Read())
			{
				throw new Exception("Error reading JConstructor from JsonReader.");
			}
			jconstructor.ReadContentFrom(reader);
			return jconstructor;
		}

		// Token: 0x040000CB RID: 203
		private string _name;
	}
}
