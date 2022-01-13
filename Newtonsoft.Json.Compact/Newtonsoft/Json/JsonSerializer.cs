using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x0200002C RID: 44
	public class JsonSerializer
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600022D RID: 557 RVA: 0x00009062 File Offset: 0x00007262
		// (remove) Token: 0x0600022E RID: 558 RVA: 0x0000907B File Offset: 0x0000727B
		public virtual event EventHandler<ErrorEventArgs> Error;

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00009094 File Offset: 0x00007294
		// (set) Token: 0x06000230 RID: 560 RVA: 0x000090AF File Offset: 0x000072AF
		public virtual IReferenceResolver ReferenceResolver
		{
			get
			{
				if (this._referenceResolver == null)
				{
					this._referenceResolver = new DefaultReferenceResolver();
				}
				return this._referenceResolver;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				this._referenceResolver = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000231 RID: 561 RVA: 0x000090CB File Offset: 0x000072CB
		// (set) Token: 0x06000232 RID: 562 RVA: 0x000090D3 File Offset: 0x000072D3
		public virtual SerializationBinder Binder
		{
			get
			{
				return this._binder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				this._binder = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000233 RID: 563 RVA: 0x000090EF File Offset: 0x000072EF
		// (set) Token: 0x06000234 RID: 564 RVA: 0x000090F7 File Offset: 0x000072F7
		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._typeNameHandling = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00009113 File Offset: 0x00007313
		// (set) Token: 0x06000236 RID: 566 RVA: 0x0000911B File Offset: 0x0000731B
		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._preserveReferencesHandling = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00009137 File Offset: 0x00007337
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000913F File Offset: 0x0000733F
		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._referenceLoopHandling = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000915B File Offset: 0x0000735B
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00009163 File Offset: 0x00007363
		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._missingMemberHandling = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000917F File Offset: 0x0000737F
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00009187 File Offset: 0x00007387
		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return this._nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._nullValueHandling = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600023D RID: 573 RVA: 0x000091A3 File Offset: 0x000073A3
		// (set) Token: 0x0600023E RID: 574 RVA: 0x000091AB File Offset: 0x000073AB
		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._defaultValueHandling = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000091C7 File Offset: 0x000073C7
		// (set) Token: 0x06000240 RID: 576 RVA: 0x000091CF File Offset: 0x000073CF
		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._objectCreationHandling = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000091EB File Offset: 0x000073EB
		// (set) Token: 0x06000242 RID: 578 RVA: 0x000091F3 File Offset: 0x000073F3
		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._constructorHandling = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000920F File Offset: 0x0000740F
		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (this._converters == null)
				{
					this._converters = new JsonConverterCollection();
				}
				return this._converters;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000922A File Offset: 0x0000742A
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00009245 File Offset: 0x00007445
		public virtual IContractResolver ContractResolver
		{
			get
			{
				if (this._contractResolver == null)
				{
					this._contractResolver = DefaultContractResolver.Instance;
				}
				return this._contractResolver;
			}
			set
			{
				this._contractResolver = value;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009250 File Offset: 0x00007450
		public JsonSerializer()
		{
			this._referenceLoopHandling = ReferenceLoopHandling.Error;
			this._missingMemberHandling = MissingMemberHandling.Ignore;
			this._nullValueHandling = NullValueHandling.Include;
			this._defaultValueHandling = DefaultValueHandling.Include;
			this._objectCreationHandling = ObjectCreationHandling.Auto;
			this._preserveReferencesHandling = PreserveReferencesHandling.None;
			this._constructorHandling = ConstructorHandling.Default;
			this._typeNameHandling = TypeNameHandling.None;
			this._binder = DefaultSerializationBinder.Instance;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000092A8 File Offset: 0x000074A8
		public static JsonSerializer Create(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			if (settings != null)
			{
				if (!CollectionUtils.IsNullOrEmpty<JsonConverter>(settings.Converters))
				{
					jsonSerializer.Converters.AddRange(settings.Converters);
				}
				jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
				jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
				jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
				jsonSerializer.MissingMemberHandling = settings.MissingMemberHandling;
				jsonSerializer.ObjectCreationHandling = settings.ObjectCreationHandling;
				jsonSerializer.NullValueHandling = settings.NullValueHandling;
				jsonSerializer.DefaultValueHandling = settings.DefaultValueHandling;
				jsonSerializer.ConstructorHandling = settings.ConstructorHandling;
				if (settings.Error != null)
				{
					JsonSerializer jsonSerializer2 = jsonSerializer;
					jsonSerializer2.Error = (EventHandler<ErrorEventArgs>)Delegate.Combine(jsonSerializer2.Error, settings.Error);
				}
				if (settings.ContractResolver != null)
				{
					jsonSerializer.ContractResolver = settings.ContractResolver;
				}
				if (settings.ReferenceResolver != null)
				{
					jsonSerializer.ReferenceResolver = settings.ReferenceResolver;
				}
				if (settings.Binder != null)
				{
					jsonSerializer.Binder = settings.Binder;
				}
			}
			return jsonSerializer;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000093A0 File Offset: 0x000075A0
		public void Populate(TextReader reader, object target)
		{
			this.Populate(new JsonTextReader(reader), target);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000093AF File Offset: 0x000075AF
		public void Populate(JsonReader reader, object target)
		{
			this.PopulateInternal(reader, target);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000093BC File Offset: 0x000075BC
		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			jsonSerializerInternalReader.Populate(reader, target);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000093EE File Offset: 0x000075EE
		public object Deserialize(JsonReader reader)
		{
			return this.Deserialize(reader, null);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000093F8 File Offset: 0x000075F8
		public object Deserialize(TextReader reader, Type objectType)
		{
			return this.Deserialize(new JsonTextReader(reader), objectType);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009407 File Offset: 0x00007607
		public object Deserialize(JsonReader reader, Type objectType)
		{
			return this.DeserializeInternal(reader, objectType);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009414 File Offset: 0x00007614
		internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			return jsonSerializerInternalReader.Deserialize(reader, objectType);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000943B File Offset: 0x0000763B
		public void Serialize(TextWriter textWriter, object value)
		{
			this.Serialize(new JsonTextWriter(textWriter), value);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000944A File Offset: 0x0000764A
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			this.SerializeInternal(jsonWriter, value);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009454 File Offset: 0x00007654
		internal virtual void SerializeInternal(JsonWriter jsonWriter, object value)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			JsonSerializerInternalWriter jsonSerializerInternalWriter = new JsonSerializerInternalWriter(this);
			jsonSerializerInternalWriter.Serialize(jsonWriter, value);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000947B File Offset: 0x0000767B
		internal bool HasClassConverter(Type objectType, out JsonConverter converter)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			converter = JsonTypeReflector.GetConverter(objectType, objectType);
			return converter != null;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000949C File Offset: 0x0000769C
		internal bool HasMatchingConverter(Type type, out JsonConverter matchingConverter)
		{
			return JsonSerializer.HasMatchingConverter(this._converters, type, out matchingConverter) || JsonSerializer.HasMatchingConverter(JsonSerializer.BuiltInConverters, type, out matchingConverter);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000094C0 File Offset: 0x000076C0
		internal static bool HasMatchingConverter(IList<JsonConverter> converters, Type objectType, out JsonConverter matchingConverter)
		{
			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						matchingConverter = jsonConverter;
						return true;
					}
				}
			}
			matchingConverter = null;
			return false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000950C File Offset: 0x0000770C
		internal void OnError(ErrorEventArgs e)
		{
			EventHandler<ErrorEventArgs> error = this.Error;
			if (error != null)
			{
				error.Invoke(this, e);
			}
		}

		// Token: 0x040000B2 RID: 178
		private TypeNameHandling _typeNameHandling;

		// Token: 0x040000B3 RID: 179
		private PreserveReferencesHandling _preserveReferencesHandling;

		// Token: 0x040000B4 RID: 180
		private ReferenceLoopHandling _referenceLoopHandling;

		// Token: 0x040000B5 RID: 181
		private MissingMemberHandling _missingMemberHandling;

		// Token: 0x040000B6 RID: 182
		private ObjectCreationHandling _objectCreationHandling;

		// Token: 0x040000B7 RID: 183
		private NullValueHandling _nullValueHandling;

		// Token: 0x040000B8 RID: 184
		private DefaultValueHandling _defaultValueHandling;

		// Token: 0x040000B9 RID: 185
		private ConstructorHandling _constructorHandling;

		// Token: 0x040000BA RID: 186
		private JsonConverterCollection _converters;

		// Token: 0x040000BB RID: 187
		private IContractResolver _contractResolver;

		// Token: 0x040000BC RID: 188
		private IReferenceResolver _referenceResolver;

		// Token: 0x040000BD RID: 189
		private SerializationBinder _binder;

		// Token: 0x040000BE RID: 190
		private static readonly IList<JsonConverter> BuiltInConverters = new List<JsonConverter>();
	}
}
