using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200000C RID: 12
    [AttributeUsage((AttributeTargets)388, AllowMultiple = false)]
	public class JsonConverterAttribute : Attribute
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002FA4 File Offset: 0x000011A4
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002FAC File Offset: 0x000011AC
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002FCC File Offset: 0x000011CC
		internal static JsonConverter CreateJsonConverterInstance(Type converterType)
		{
			JsonConverter result;
			try
			{
				result = (JsonConverter)Activator.CreateInstance(converterType);
			}
			catch (Exception ex)
			{
				throw new Exception("Error creating {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					converterType
				}), ex);
			}
			return result;
		}

		// Token: 0x04000012 RID: 18
		private readonly Type _converterType;
	}
}
