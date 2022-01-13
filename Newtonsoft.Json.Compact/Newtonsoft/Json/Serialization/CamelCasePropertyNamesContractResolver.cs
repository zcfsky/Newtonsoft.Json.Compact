using System;
using System.Globalization;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000045 RID: 69
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		// Token: 0x0600041C RID: 1052 RVA: 0x0000EF80 File Offset: 0x0000D180
		protected override string ResolvePropertyName(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return propertyName;
			}
			if (!char.IsUpper(propertyName[0]))
			{
				return propertyName;
			}
			string text = char.ToLower(propertyName[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
			if (propertyName.Length > 1)
			{
				text += propertyName.Substring(1);
			}
			return text;
		}
	}
}
