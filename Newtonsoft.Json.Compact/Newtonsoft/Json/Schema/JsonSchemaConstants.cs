using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x02000053 RID: 83
	internal static class JsonSchemaConstants
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x00010688 File Offset: 0x0000E888
		// Note: this type is marked as 'beforefieldinit'.
		static JsonSchemaConstants()
		{
			Dictionary<string, JsonSchemaType> dictionary = new Dictionary<string, JsonSchemaType>();
			dictionary.Add("string", JsonSchemaType.String);
			dictionary.Add("object", JsonSchemaType.Object);
			dictionary.Add("integer", JsonSchemaType.Integer);
			dictionary.Add("number", JsonSchemaType.Float);
			dictionary.Add("null", JsonSchemaType.Null);
			dictionary.Add("boolean", JsonSchemaType.Boolean);
			dictionary.Add("array", JsonSchemaType.Array);
			dictionary.Add("any", JsonSchemaType.Any);
			JsonSchemaConstants.JsonSchemaTypeMapping = dictionary;
		}

		// Token: 0x04000145 RID: 325
		public const string TypePropertyName = "type";

		// Token: 0x04000146 RID: 326
		public const string PropertiesPropertyName = "properties";

		// Token: 0x04000147 RID: 327
		public const string ItemsPropertyName = "items";

		// Token: 0x04000148 RID: 328
		public const string OptionalPropertyName = "optional";

		// Token: 0x04000149 RID: 329
		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		// Token: 0x0400014A RID: 330
		public const string RequiresPropertyName = "requires";

		// Token: 0x0400014B RID: 331
		public const string IdentityPropertyName = "identity";

		// Token: 0x0400014C RID: 332
		public const string MinimumPropertyName = "minimum";

		// Token: 0x0400014D RID: 333
		public const string MaximumPropertyName = "maximum";

		// Token: 0x0400014E RID: 334
		public const string MinimumItemsPropertyName = "minItems";

		// Token: 0x0400014F RID: 335
		public const string MaximumItemsPropertyName = "maxItems";

		// Token: 0x04000150 RID: 336
		public const string PatternPropertyName = "pattern";

		// Token: 0x04000151 RID: 337
		public const string MaximumLengthPropertyName = "maxLength";

		// Token: 0x04000152 RID: 338
		public const string MinimumLengthPropertyName = "minLength";

		// Token: 0x04000153 RID: 339
		public const string EnumPropertyName = "enum";

		// Token: 0x04000154 RID: 340
		public const string OptionsPropertyName = "options";

		// Token: 0x04000155 RID: 341
		public const string ReadOnlyPropertyName = "readonly";

		// Token: 0x04000156 RID: 342
		public const string TitlePropertyName = "title";

		// Token: 0x04000157 RID: 343
		public const string DescriptionPropertyName = "description";

		// Token: 0x04000158 RID: 344
		public const string FormatPropertyName = "format";

		// Token: 0x04000159 RID: 345
		public const string DefaultPropertyName = "default";

		// Token: 0x0400015A RID: 346
		public const string TransientPropertyName = "transient";

		// Token: 0x0400015B RID: 347
		public const string MaximumDecimalsPropertyName = "maxDecimal";

		// Token: 0x0400015C RID: 348
		public const string HiddenPropertyName = "hidden";

		// Token: 0x0400015D RID: 349
		public const string DisallowPropertyName = "disallow";

		// Token: 0x0400015E RID: 350
		public const string ExtendsPropertyName = "extends";

		// Token: 0x0400015F RID: 351
		public const string IdPropertyName = "id";

		// Token: 0x04000160 RID: 352
		public const string OptionValuePropertyName = "value";

		// Token: 0x04000161 RID: 353
		public const string OptionLabelPropertyName = "label";

		// Token: 0x04000162 RID: 354
		public const string ReferencePropertyName = "$ref";

		// Token: 0x04000163 RID: 355
		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping;
	}
}
