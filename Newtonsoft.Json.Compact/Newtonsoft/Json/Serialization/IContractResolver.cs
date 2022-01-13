using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000043 RID: 67
	public interface IContractResolver
	{
		// Token: 0x0600040A RID: 1034
		JsonContract ResolveContract(Type type);
	}
}
