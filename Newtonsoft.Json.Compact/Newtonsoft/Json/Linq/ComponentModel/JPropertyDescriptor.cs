using System;
using System.ComponentModel;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq.ComponentModel
{
	// Token: 0x02000013 RID: 19
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x060000DA RID: 218 RVA: 0x0000455F File Offset: 0x0000275F
		public JPropertyDescriptor(string name, Type propertyType) : base(name, null)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			ValidationUtils.ArgumentNotNull(propertyType, "propertyType");
			this._propertyType = propertyType;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004586 File Offset: 0x00002786
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000458E File Offset: 0x0000278E
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004594 File Offset: 0x00002794
		public override object GetValue(object component)
		{
			return JPropertyDescriptor.CastInstance(component)[this.Name];
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000045B4 File Offset: 0x000027B4
		public override void ResetValue(object component)
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000045B8 File Offset: 0x000027B8
		public override void SetValue(object component, object value)
		{
			JToken value2 = (value is JToken) ? ((JToken)value) : new JValue(value);
			JPropertyDescriptor.CastInstance(component)[this.Name] = value2;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000045EE File Offset: 0x000027EE
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000045F1 File Offset: 0x000027F1
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000045FD File Offset: 0x000027FD
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004600 File Offset: 0x00002800
		public override Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004608 File Offset: 0x00002808
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}

		// Token: 0x04000050 RID: 80
		private readonly Type _propertyType;
	}
}
