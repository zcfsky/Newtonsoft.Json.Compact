using System;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000042 RID: 66
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0000E8C2 File Offset: 0x0000CAC2
		public static T GetAttribute(ICustomAttributeProvider type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x040000FC RID: 252
		private static readonly ThreadSafeStore<ICustomAttributeProvider, T> TypeAttributeCache = new ThreadSafeStore<ICustomAttributeProvider, T>(new Func<ICustomAttributeProvider, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
