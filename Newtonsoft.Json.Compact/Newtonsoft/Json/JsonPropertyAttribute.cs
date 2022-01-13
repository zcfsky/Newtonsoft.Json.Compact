using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000023 RID: 35
	[AttributeUsage((AttributeTargets)384, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000081B8 File Offset: 0x000063B8
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000081DE File Offset: 0x000063DE
		public NullValueHandling NullValueHandling
		{
			get
			{
				NullValueHandling? nullValueHandling = this._nullValueHandling;
				if (nullValueHandling == null)
				{
					return NullValueHandling.Include;
				}
				return nullValueHandling.GetValueOrDefault();
			}
			set
			{
				this._nullValueHandling = new NullValueHandling?(value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000081EC File Offset: 0x000063EC
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00008212 File Offset: 0x00006412
		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				DefaultValueHandling? defaultValueHandling = this._defaultValueHandling;
				if (defaultValueHandling == null)
				{
					return DefaultValueHandling.Include;
				}
				return defaultValueHandling.GetValueOrDefault();
			}
			set
			{
				this._defaultValueHandling = new DefaultValueHandling?(value);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00008220 File Offset: 0x00006420
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00008246 File Offset: 0x00006446
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				ReferenceLoopHandling? referenceLoopHandling = this._referenceLoopHandling;
				if (referenceLoopHandling == null)
				{
					return ReferenceLoopHandling.Error;
				}
				return referenceLoopHandling.GetValueOrDefault();
			}
			set
			{
				this._referenceLoopHandling = new ReferenceLoopHandling?(value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00008254 File Offset: 0x00006454
		// (set) Token: 0x060001BA RID: 442 RVA: 0x0000827A File Offset: 0x0000647A
		public bool IsReference
		{
			get
			{
				return this._isReference ?? false;
			}
			set
			{
				this._isReference = new bool?(value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00008288 File Offset: 0x00006488
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00008290 File Offset: 0x00006490
		public string PropertyName { get; set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00008299 File Offset: 0x00006499
		// (set) Token: 0x060001BE RID: 446 RVA: 0x000082A1 File Offset: 0x000064A1
		public bool IsRequired { get; set; }

		// Token: 0x060001BF RID: 447 RVA: 0x000082AA File Offset: 0x000064AA
		public JsonPropertyAttribute()
		{
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000082B2 File Offset: 0x000064B2
		public JsonPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x0400009B RID: 155
		internal NullValueHandling? _nullValueHandling;

		// Token: 0x0400009C RID: 156
		internal DefaultValueHandling? _defaultValueHandling;

		// Token: 0x0400009D RID: 157
		internal ReferenceLoopHandling? _referenceLoopHandling;

		// Token: 0x0400009E RID: 158
		internal bool? _isReference;
	}
}
