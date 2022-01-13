using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000048 RID: 72
	public class DefaultSerializationBinder : SerializationBinder
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0000F0A4 File Offset: 0x0000D2A4
		public override Type BindToType(string assemblyName, string typeName)
		{
			if (assemblyName == null)
			{
				return Type.GetType(typeName);
			}
			Assembly assembly = Assembly.Load(assemblyName);
			if (assembly == null)
			{
				throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					assemblyName
				}));
			}
			Type type = assembly.GetType(typeName);
			if (type == null)
			{
				throw new JsonSerializationException("Could not find type '{0}'.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					typeName
				}));
			}
			return type;
		}

		// Token: 0x04000102 RID: 258
		internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
	}
}
