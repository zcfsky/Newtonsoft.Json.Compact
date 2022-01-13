using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000003 RID: 3
	public abstract class JsonConverter
	{
		// Token: 0x06000001 RID: 1
		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		// Token: 0x06000002 RID: 2
		public abstract object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer);

		// Token: 0x06000003 RID: 3
		public abstract bool CanConvert(Type objectType);
	}
}
