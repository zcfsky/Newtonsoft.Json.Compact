using System;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000005 RID: 5
	public abstract class CustomCreationConverter<T> : JsonConverter
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002297 File Offset: 0x00000497
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022A4 File Offset: 0x000004A4
		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x0600000C RID: 12
		public abstract T Create(Type objectType);

		// Token: 0x0600000D RID: 13 RVA: 0x000022DF File Offset: 0x000004DF
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}
	}
}
