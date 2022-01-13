using System;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006E RID: 110
	internal class EnumValues<T> : KeyedCollection<string, EnumValue<T>> where T : struct
	{
		// Token: 0x060005C0 RID: 1472 RVA: 0x00014952 File Offset: 0x00012B52
		protected override string GetKeyForItem(EnumValue<T> item)
		{
			return item.Name;
		}
	}
}
