using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200004C RID: 76
	public class JsonArrayContract : JsonContract
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000F2AE File Offset: 0x0000D4AE
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
		internal Type CollectionItemType { get; private set; }

		// Token: 0x0600044C RID: 1100 RVA: 0x0000F2C0 File Offset: 0x0000D4C0
		public JsonArrayContract(Type underlyingType) : base(underlyingType)
		{
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
			}
			else
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
			}
			if (this.CollectionItemType != null)
			{
				this._isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(this.CollectionItemType);
			}
			if (this.IsTypeGenericCollectionInterface(base.UnderlyingType))
			{
				base.CreatedType = ReflectionUtils.MakeGenericType(typeof(List<>), new Type[]
				{
					this.CollectionItemType
				});
				return;
			}
			base.CreatedType = base.UnderlyingType;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000F36C File Offset: 0x0000D56C
		internal IWrappedCollection CreateWrapper(object list)
		{
			if (list is IList && (this.CollectionItemType == null || !this._isCollectionItemTypeNullableType))
			{
				return new CollectionWrapper<object>((IList)list);
			}
			if (this._genericWrapperType == null)
			{
				this._genericWrapperType = ReflectionUtils.MakeGenericType(typeof(CollectionWrapper<>), new Type[]
				{
					this.CollectionItemType
				});
				this._genericWrapperConstructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
			}
			return (IWrappedCollection)this._genericWrapperConstructor.Invoke(new object[]
			{
				list
			});
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000F40C File Offset: 0x0000D60C
		private bool IsTypeGenericCollectionInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList) || genericTypeDefinition == typeof(ICollection) || genericTypeDefinition == typeof(IEnumerable);
		}

		// Token: 0x04000110 RID: 272
		private bool _isCollectionItemTypeNullableType;

		// Token: 0x04000111 RID: 273
		private Type _genericCollectionDefinitionType;

		// Token: 0x04000112 RID: 274
		private Type _genericWrapperType;

		// Token: 0x04000113 RID: 275
		private ConstructorInfo _genericWrapperConstructor;
	}
}
