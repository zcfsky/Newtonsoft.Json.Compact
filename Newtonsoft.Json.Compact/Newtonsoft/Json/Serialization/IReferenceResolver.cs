using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000046 RID: 70
	public interface IReferenceResolver
	{
		// Token: 0x0600041E RID: 1054
		object ResolveReference(string reference);

		// Token: 0x0600041F RID: 1055
		string GetReference(object value);

		// Token: 0x06000420 RID: 1056
		bool IsReferenced(object value);

		// Token: 0x06000421 RID: 1057
		void AddReference(string reference, object value);
	}
}
