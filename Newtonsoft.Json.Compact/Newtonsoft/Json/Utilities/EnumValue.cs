using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006D RID: 109
	internal class EnumValue<T> where T : struct
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0001492C File Offset: 0x00012B2C
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00014934 File Offset: 0x00012B34
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001493C File Offset: 0x00012B3C
		public EnumValue(string name, T value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x040001B9 RID: 441
		private string _name;

		// Token: 0x040001BA RID: 442
		private T _value;
	}
}
