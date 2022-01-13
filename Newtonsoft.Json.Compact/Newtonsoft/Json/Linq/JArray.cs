using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000033 RID: 51
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000CA84 File Offset: 0x0000AC84
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000CA87 File Offset: 0x0000AC87
		public JArray()
		{
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000CA8F File Offset: 0x0000AC8F
		public JArray(JArray other) : base(other)
		{
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000CA98 File Offset: 0x0000AC98
		public JArray(params object[] content) : this((object)content)
		{
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000CAA1 File Offset: 0x0000ACA1
		public JArray(object content)
		{
			base.Add(content);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
		public static JArray Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JArray from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw new Exception("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo);
			if (!reader.Read())
			{
				throw new Exception("Error reading JArray from JsonReader.");
			}
			jarray.ReadContentFrom(reader);
			return jarray;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000CB64 File Offset: 0x0000AD64
		public static JArray Parse(string json)
		{
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JArray.Load(reader);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000CB83 File Offset: 0x0000AD83
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, new JsonSerializer());
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000CB90 File Offset: 0x0000AD90
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtoken.Type
				}));
			}
			return (JArray)jtoken;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000CBE0 File Offset: 0x0000ADE0
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			foreach (JToken jtoken in this.Children())
			{
				jtoken.WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x1700009B RID: 155
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
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
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x1700009C RID: 156
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000CCFA File Offset: 0x0000AEFA
		public int IndexOf(JToken item)
		{
			return base.IndexOfItem(item);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000CD03 File Offset: 0x0000AF03
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000CD0D File Offset: 0x0000AF0D
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000CD16 File Offset: 0x0000AF16
		public void Add(JToken item)
		{
			base.Add(item);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000CD1F File Offset: 0x0000AF1F
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000CD27 File Offset: 0x0000AF27
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000CD30 File Offset: 0x0000AF30
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000CD3A File Offset: 0x0000AF3A
		public int Count
		{
			get
			{
				return this.CountItems();
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600037B RID: 891 RVA: 0x0000CD42 File Offset: 0x0000AF42
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000CD45 File Offset: 0x0000AF45
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000CD4E File Offset: 0x0000AF4E
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}
	}
}
