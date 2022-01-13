using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.ComponentModel
{
	// Token: 0x02000014 RID: 20
	public class JTypeDescriptor : ICustomTypeDescriptor
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000461D File Offset: 0x0000281D
		public JTypeDescriptor(JObject value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			this._value = value;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004637 File Offset: 0x00002837
		public virtual PropertyDescriptorCollection GetProperties()
		{
			return this.GetProperties(null);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004640 File Offset: 0x00002840
		private static Type GetTokenPropertyType(JToken token)
		{
			if (!(token is JValue))
			{
				return token.GetType();
			}
			JValue jvalue = (JValue)token;
			if (jvalue.Value == null)
			{
				return typeof(object);
			}
			return jvalue.Value.GetType();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004684 File Offset: 0x00002884
		public virtual PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			if (this._value != null)
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in this._value)
				{
					propertyDescriptorCollection.Add(new JPropertyDescriptor(keyValuePair.Key, JTypeDescriptor.GetTokenPropertyType(keyValuePair.Value)));
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000046FC File Offset: 0x000028FC
		public AttributeCollection GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004703 File Offset: 0x00002903
		public string GetClassName()
		{
			return null;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004706 File Offset: 0x00002906
		public string GetComponentName()
		{
			return null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004709 File Offset: 0x00002909
		public TypeConverter GetConverter()
		{
			return new TypeConverter();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004710 File Offset: 0x00002910
		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004713 File Offset: 0x00002913
		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004716 File Offset: 0x00002916
		public object GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004719 File Offset: 0x00002919
		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004720 File Offset: 0x00002920
		public EventDescriptorCollection GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004727 File Offset: 0x00002927
		public object GetPropertyOwner(PropertyDescriptor pd)
		{
			return null;
		}

		// Token: 0x04000051 RID: 81
		private readonly JObject _value;
	}
}
