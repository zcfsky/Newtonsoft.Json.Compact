using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
	// Token: 0x0200000E RID: 14
	public class JsonSerializerSettings
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000074 RID: 116 RVA: 0x0000304D File Offset: 0x0000124D
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003055 File Offset: 0x00001255
		public ReferenceLoopHandling ReferenceLoopHandling { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000076 RID: 118 RVA: 0x0000305E File Offset: 0x0000125E
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003066 File Offset: 0x00001266
		public MissingMemberHandling MissingMemberHandling { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000306F File Offset: 0x0000126F
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003077 File Offset: 0x00001277
		public ObjectCreationHandling ObjectCreationHandling { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003080 File Offset: 0x00001280
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003088 File Offset: 0x00001288
		public NullValueHandling NullValueHandling { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003091 File Offset: 0x00001291
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003099 File Offset: 0x00001299
		public DefaultValueHandling DefaultValueHandling { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000030A2 File Offset: 0x000012A2
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000030AA File Offset: 0x000012AA
		public IList<JsonConverter> Converters { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000030B3 File Offset: 0x000012B3
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000030BB File Offset: 0x000012BB
		public PreserveReferencesHandling PreserveReferencesHandling { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000030C4 File Offset: 0x000012C4
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000030CC File Offset: 0x000012CC
		public TypeNameHandling TypeNameHandling { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000030D5 File Offset: 0x000012D5
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000030DD File Offset: 0x000012DD
		public ConstructorHandling ConstructorHandling { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000030E6 File Offset: 0x000012E6
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000030EE File Offset: 0x000012EE
		public IContractResolver ContractResolver { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000030F7 File Offset: 0x000012F7
		// (set) Token: 0x06000089 RID: 137 RVA: 0x000030FF File Offset: 0x000012FF
		public IReferenceResolver ReferenceResolver { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003108 File Offset: 0x00001308
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003110 File Offset: 0x00001310
		public SerializationBinder Binder { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003119 File Offset: 0x00001319
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003121 File Offset: 0x00001321
		public EventHandler<ErrorEventArgs> Error { get; set; }

		// Token: 0x0600008E RID: 142 RVA: 0x0000312C File Offset: 0x0000132C
		public JsonSerializerSettings()
		{
			this.ReferenceLoopHandling = ReferenceLoopHandling.Error;
			this.MissingMemberHandling = MissingMemberHandling.Ignore;
			this.ObjectCreationHandling = ObjectCreationHandling.Auto;
			this.NullValueHandling = NullValueHandling.Include;
			this.DefaultValueHandling = DefaultValueHandling.Include;
			this.PreserveReferencesHandling = PreserveReferencesHandling.None;
			this.TypeNameHandling = TypeNameHandling.None;
			this.Converters = new List<JsonConverter>();
		}

		// Token: 0x04000014 RID: 20
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		// Token: 0x04000015 RID: 21
		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		// Token: 0x04000016 RID: 22
		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		// Token: 0x04000017 RID: 23
		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		// Token: 0x04000018 RID: 24
		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		// Token: 0x04000019 RID: 25
		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		// Token: 0x0400001A RID: 26
		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		// Token: 0x0400001B RID: 27
		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;
	}
}
