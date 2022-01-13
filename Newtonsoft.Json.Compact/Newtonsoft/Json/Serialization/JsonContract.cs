using System;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200004B RID: 75
	public abstract class JsonContract
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0000F1C0 File Offset: 0x0000D3C0
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
		public Type UnderlyingType { get; private set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0000F1D1 File Offset: 0x0000D3D1
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0000F1D9 File Offset: 0x0000D3D9
		public Type CreatedType { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0000F1E2 File Offset: 0x0000D3E2
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0000F1EA File Offset: 0x0000D3EA
		public bool? IsReference { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0000F1F3 File Offset: 0x0000D3F3
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0000F1FB File Offset: 0x0000D3FB
		public ConstructorInfo DefaultContstructor { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0000F204 File Offset: 0x0000D404
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0000F20C File Offset: 0x0000D40C
		public MethodInfo OnError { get; set; }

		// Token: 0x06000443 RID: 1091 RVA: 0x0000F215 File Offset: 0x0000D415
		internal void InvokeOnSerializing(object o)
		{
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000F217 File Offset: 0x0000D417
		internal void InvokeOnSerialized(object o)
		{
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000F219 File Offset: 0x0000D419
		internal void InvokeOnDeserializing(object o)
		{
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000F21B File Offset: 0x0000D41B
		internal void InvokeOnDeserialized(object o)
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000F220 File Offset: 0x0000D420
		internal void InvokeOnError(object o, ErrorContext errorContext)
		{
			if (this.OnError != null)
			{
				this.OnError.Invoke(o, new object[]
				{
					JsonContract.SerializationStreamingContextParameter,
					errorContext
				});
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000F25B File Offset: 0x0000D45B
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
		}

		// Token: 0x04000109 RID: 265
		private static readonly StreamingContext SerializationStreamingContextParameter = new StreamingContext(StreamingContextStates.All);

		// Token: 0x0400010A RID: 266
		private static readonly object[] SerializationEventParameterValues = new object[]
		{
			JsonContract.SerializationStreamingContextParameter
		};
	}
}
