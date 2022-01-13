using System;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000058 RID: 88
	public class JsonObjectContract : JsonContract
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00010F83 File Offset: 0x0000F183
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x00010F8B File Offset: 0x0000F18B
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00010F94 File Offset: 0x0000F194
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00010F9C File Offset: 0x0000F19C
		public JsonPropertyCollection Properties { get; private set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00010FA5 File Offset: 0x0000F1A5
		// (set) Token: 0x060004CB RID: 1227 RVA: 0x00010FAD File Offset: 0x0000F1AD
		public ConstructorInfo ParametrizedConstructor { get; set; }

		// Token: 0x060004CC RID: 1228 RVA: 0x00010FB6 File Offset: 0x0000F1B6
		public JsonObjectContract(Type underlyingType) : base(underlyingType)
		{
			this.Properties = new JsonPropertyCollection();
			base.CreatedType = underlyingType;
		}
	}
}
