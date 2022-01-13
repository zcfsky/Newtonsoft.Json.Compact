using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000041 RID: 65
	public abstract class SerializationBinder
	{
		// Token: 0x06000406 RID: 1030
		public abstract Type BindToType(string assemblyName, string typeName);
	}
}
