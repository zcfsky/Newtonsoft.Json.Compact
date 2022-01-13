using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200005E RID: 94
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000526 RID: 1318 RVA: 0x00012E95 File Offset: 0x00011095
		// (remove) Token: 0x06000527 RID: 1319 RVA: 0x00012EA3 File Offset: 0x000110A3
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00012EB1 File Offset: 0x000110B1
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00012EBE File Offset: 0x000110BE
		public override IReferenceResolver ReferenceResolver
		{
			get
			{
				return this._serializer.ReferenceResolver;
			}
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00012ECC File Offset: 0x000110CC
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x00012ED9 File Offset: 0x000110D9
		// (set) Token: 0x0600052C RID: 1324 RVA: 0x00012EE6 File Offset: 0x000110E6
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00012EF4 File Offset: 0x000110F4
		// (set) Token: 0x0600052E RID: 1326 RVA: 0x00012F01 File Offset: 0x00011101
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00012F0F File Offset: 0x0001110F
		// (set) Token: 0x06000530 RID: 1328 RVA: 0x00012F1C File Offset: 0x0001111C
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x00012F2A File Offset: 0x0001112A
		// (set) Token: 0x06000532 RID: 1330 RVA: 0x00012F37 File Offset: 0x00011137
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00012F45 File Offset: 0x00011145
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x00012F52 File Offset: 0x00011152
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00012F60 File Offset: 0x00011160
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00012F6D File Offset: 0x0001116D
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00012F7B File Offset: 0x0001117B
		// (set) Token: 0x06000538 RID: 1336 RVA: 0x00012F88 File Offset: 0x00011188
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00012F96 File Offset: 0x00011196
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00012FA3 File Offset: 0x000111A3
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00012FB1 File Offset: 0x000111B1
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00012FBE File Offset: 0x000111BE
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00012FCC File Offset: 0x000111CC
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00012FD9 File Offset: 0x000111D9
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012FE7 File Offset: 0x000111E7
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001300D File Offset: 0x0001120D
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00013033 File Offset: 0x00011233
		internal override object DeserializeInternal(JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00013058 File Offset: 0x00011258
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001307D File Offset: 0x0001127D
		internal override void SerializeInternal(JsonWriter jsonWriter, object value)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x04000194 RID: 404
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x04000195 RID: 405
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x04000196 RID: 406
		private readonly JsonSerializer _serializer;
	}
}
