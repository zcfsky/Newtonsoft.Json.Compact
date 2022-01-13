using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200004D RID: 77
	public class JsonDictionaryContract : JsonContract
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0000F453 File Offset: 0x0000D653
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0000F45B File Offset: 0x0000D65B
		internal Type DictionaryKeyType { get; private set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0000F464 File Offset: 0x0000D664
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x0000F46C File Offset: 0x0000D66C
		internal Type DictionaryValueType { get; private set; }

		// Token: 0x06000453 RID: 1107 RVA: 0x0000F478 File Offset: 0x0000D678
		public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
		{
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			if (this.IsTypeGenericDictionaryInterface(base.UnderlyingType))
			{
				base.CreatedType = ReflectionUtils.MakeGenericType(typeof(Dictionary<,>), new Type[]
				{
					type,
					type2
				});
				return;
			}
			base.CreatedType = underlyingType;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000F51C File Offset: 0x0000D71C
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (dictionary is IDictionary)
			{
				return new DictionaryWrapper<object, object>((IDictionary)dictionary);
			}
			if (this._genericWrapperType == null)
			{
				this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(DictionaryWrapper<, >), new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				this._genericWrapperConstructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
			}
			return (IWrappedDictionary)this._genericWrapperConstructor.Invoke(new object[]
			{
				dictionary
			});
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000F5B4 File Offset: 0x0000D7B4
		private bool IsTypeGenericDictionaryInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary);
		}

		// Token: 0x04000115 RID: 277
		private Type _genericCollectionDefinitionType;

		// Token: 0x04000116 RID: 278
		private Type _genericWrapperType;

		// Token: 0x04000117 RID: 279
		private ConstructorInfo _genericWrapperConstructor;
	}
}
