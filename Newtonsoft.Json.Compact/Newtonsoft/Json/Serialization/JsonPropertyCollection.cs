using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005A RID: 90
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		// Token: 0x060004E6 RID: 1254 RVA: 0x000110A5 File Offset: 0x0000F2A5
		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000110B0 File Offset: 0x0000F2B0
		public void AddProperty(JsonProperty property)
		{
			if (base.Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base[property.PropertyName];
				if (!jsonProperty.Ignored)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						property.PropertyName,
						property.Member.DeclaringType
					}));
				}
				base.Remove(jsonProperty);
			}
			base.Add(property);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001112C File Offset: 0x0000F32C
		public bool TryGetClosestMatchProperty(string propertyName, out JsonProperty property)
		{
			return this.TryGetProperty(propertyName, (StringComparison)4, out property) || this.TryGetProperty(propertyName, (StringComparison)5, out property);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001114C File Offset: 0x0000F34C
		public bool TryGetProperty(string propertyName, StringComparison comparisonType, out JsonProperty matchingProperty)
		{
			foreach (JsonProperty jsonProperty in this)
			{
				if (string.Compare(propertyName, jsonProperty.PropertyName, comparisonType) == 0)
				{
					matchingProperty = jsonProperty;
					return true;
				}
			}
			matchingProperty = null;
			return false;
		}
	}
}
