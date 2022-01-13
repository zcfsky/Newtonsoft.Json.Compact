using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005F RID: 95
	internal static class JsonTypeReflector
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x000130A2 File Offset: 0x000112A2
		public static JsonContainerAttribute GetJsonContainerAttribute(Type type)
		{
			return CachedAttributeGetter<JsonContainerAttribute>.GetAttribute(type);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000130AA File Offset: 0x000112AA
		public static JsonObjectAttribute GetJsonObjectAttribute(Type type)
		{
			return JsonTypeReflector.GetJsonContainerAttribute(type) as JsonObjectAttribute;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000130B8 File Offset: 0x000112B8
		public static MemberSerialization GetObjectMemberSerialization(Type objectType)
		{
			JsonObjectAttribute jsonObjectAttribute = JsonTypeReflector.GetJsonObjectAttribute(objectType);
			if (jsonObjectAttribute == null)
			{
				return MemberSerialization.OptOut;
			}
			return jsonObjectAttribute.MemberSerialization;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000130D7 File Offset: 0x000112D7
		private static Type GetConverterType(ICustomAttributeProvider attributeProvider)
		{
			return JsonTypeReflector.ConverterTypeCache.Get(attributeProvider);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000130E4 File Offset: 0x000112E4
		private static Type GetConverterTypeFromAttribute(ICustomAttributeProvider attributeProvider)
		{
			JsonConverterAttribute attribute = JsonTypeReflector.GetAttribute<JsonConverterAttribute>(attributeProvider);
			if (attribute == null)
			{
				return null;
			}
			return attribute.ConverterType;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00013104 File Offset: 0x00011304
		public static JsonConverter GetConverter(ICustomAttributeProvider attributeProvider, Type targetConvertedType)
		{
			Type converterType = JsonTypeReflector.GetConverterType(attributeProvider);
			if (converterType == null)
			{
				return null;
			}
			JsonConverter jsonConverter = JsonConverterAttribute.CreateJsonConverterInstance(converterType);
			if (!jsonConverter.CanConvert(targetConvertedType))
			{
				throw new JsonSerializationException("JsonConverter {0} on {1} is not compatible with member type {2}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					jsonConverter.GetType().Name,
					attributeProvider,
					targetConvertedType.Name
				}));
			}
			return jsonConverter;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00013166 File Offset: 0x00011366
		public static T GetAttribute<T>(ICustomAttributeProvider attributeProvider) where T : Attribute
		{
			return ReflectionUtils.GetAttribute<T>(attributeProvider, true);
		}

		// Token: 0x04000197 RID: 407
		public const string IdPropertyName = "$id";

		// Token: 0x04000198 RID: 408
		public const string RefPropertyName = "$ref";

		// Token: 0x04000199 RID: 409
		public const string TypePropertyName = "$type";

		// Token: 0x0400019A RID: 410
		public const string ArrayValuesPropertyName = "$values";

		// Token: 0x0400019B RID: 411
		private static readonly ThreadSafeStore<ICustomAttributeProvider, Type> ConverterTypeCache = new ThreadSafeStore<ICustomAttributeProvider, Type>(new Func<ICustomAttributeProvider, Type>(JsonTypeReflector.GetConverterTypeFromAttribute));
	}
}
