using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000008 RID: 8
	public interface IJsonLineInfo
	{
		// Token: 0x0600005A RID: 90
		bool HasLineInfo();

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005B RID: 91
		int LineNumber { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005C RID: 92
		int LinePosition { get; }
	}
}
