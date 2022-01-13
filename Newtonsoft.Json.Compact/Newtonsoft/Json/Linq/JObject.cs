using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000032 RID: 50
	public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600033A RID: 826 RVA: 0x0000C29A File Offset: 0x0000A49A
		// (remove) Token: 0x0600033B RID: 827 RVA: 0x0000C2B3 File Offset: 0x0000A4B3
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600033C RID: 828 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		public JObject()
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
		public JObject(JObject other) : base(other)
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C2DD File Offset: 0x0000A4DD
		public JObject(params object[] content) : this((object)content)
		{
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C2E6 File Offset: 0x0000A4E6
		public JObject(object content)
		{
			base.Add(content);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C2F8 File Offset: 0x0000A4F8
		internal override bool DeepEquals(JToken node)
		{
			JObject jobject = node as JObject;
			return jobject != null && base.ContentsEqual(jobject);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C34C File Offset: 0x0000A54C
		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					o.GetType(),
					base.GetType()
				}));
			}
			JProperty property = (JProperty)o;
			bool flag = Enumerable.Any<JProperty>(this.Properties(), (JProperty p) => string.Equals(p.Name, property.Name, (StringComparison)4) && p != existing);
			if (flag)
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					property.Name,
					base.GetType()
				}));
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C404 File Offset: 0x0000A604
		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
			this.OnListChanged(new ListChangedEventArgs((ListChangedType)4, base.IndexOfItem(childProperty)));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000C425 File Offset: 0x0000A625
		internal void InternalPropertyChanging(JProperty childProperty)
		{
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C427 File Offset: 0x0000A627
		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000C42F File Offset: 0x0000A62F
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C432 File Offset: 0x0000A632
		public IEnumerable<JProperty> Properties()
		{
			return Enumerable.Cast<JProperty>(this.Children());
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C460 File Offset: 0x0000A660
		public JProperty Property(string name)
		{
			return Enumerable.SingleOrDefault<JProperty>(Enumerable.Where<JProperty>(this.Properties(), (JProperty p) => string.Equals(p.Name, name, (StringComparison)4)));
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C49E File Offset: 0x0000A69E
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(Enumerable.Select<JProperty, JToken>(this.Properties(), (JProperty p) => p.Value));
		}

		// Token: 0x17000094 RID: 148
		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				return this[text];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						MiscellaneousUtils.ToString(key)
					}));
				}
				this[text] = value;
			}
		}

		// Token: 0x17000095 RID: 149
		public JToken this[string propertyName]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jproperty = this.Property(propertyName);
				if (jproperty == null)
				{
					return null;
				}
				return jproperty.Value;
			}
			set
			{
				JProperty jproperty = this.Property(propertyName);
				if (jproperty != null)
				{
					jproperty.Value = value;
					return;
				}
				base.Add(new JProperty(propertyName, value));
				this.OnPropertyChanged(propertyName);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000C5D0 File Offset: 0x0000A7D0
		public static JObject Load(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw new Exception("Error reading JObject from JsonReader.");
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new Exception("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					reader.TokenType
				}));
			}
			JObject jobject = new JObject();
			jobject.SetLineInfo(reader as IJsonLineInfo);
			if (!reader.Read())
			{
				throw new Exception("Error reading JObject from JsonReader.");
			}
			jobject.ReadContentFrom(reader);
			return jobject;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000C664 File Offset: 0x0000A864
		public static JObject Parse(string json)
		{
			JsonReader reader = new JsonTextReader(new StringReader(json));
			return JObject.Load(reader);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C683 File Offset: 0x0000A883
		public new static JObject FromObject(object o)
		{
			return JObject.FromObject(o, new JsonSerializer());
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000C690 File Offset: 0x0000A890
		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken != null && jtoken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jtoken.Type
				}));
			}
			return (JObject)jtoken;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			foreach (JProperty jproperty in this.Properties())
			{
				jproperty.WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000C740 File Offset: 0x0000A940
		public void Add(string propertyName, JToken value)
		{
			base.Add(new JProperty(propertyName, value));
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C74F File Offset: 0x0000A94F
		bool IDictionary<string, JToken>.ContainsKey(string key)
		{
			return this.Property(key) != null;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000C75E File Offset: 0x0000A95E
		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000C768 File Offset: 0x0000A968
		public bool Remove(string propertyName)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				return false;
			}
			jproperty.Remove();
			return true;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000C78C File Offset: 0x0000A98C
		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty jproperty = this.Property(propertyName);
			if (jproperty == null)
			{
				value = null;
				return false;
			}
			value = jproperty.Value;
			return true;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000C7B2 File Offset: 0x0000A9B2
		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000C7B9 File Offset: 0x0000A9B9
		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			base.Add(new JProperty(item.Key, item.Value));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty jproperty = this.Property(item.Key);
			return jproperty != null && jproperty.Value == item.Value;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000C80C File Offset: 0x0000AA0C
		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JProperty jproperty in this.Properties())
			{
				array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
				num++;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public int Count
		{
			get
			{
				return Enumerable.Count<JToken>(this.Children());
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000C8D6 File Offset: 0x0000AAD6
		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C8D9 File Offset: 0x0000AAD9
		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string,JToken>>)this).Contains(item))
			{
				return false;
			}
			this.Remove(item.Key);
			return true;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C8F5 File Offset: 0x0000AAF5
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JProperty property in this.Properties())
			{
				yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
			}
			yield break;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000CA68 File Offset: 0x0000AC68
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
