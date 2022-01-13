using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000016 RID: 22
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000472A File Offset: 0x0000292A
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004733 File Offset: 0x00002933
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
