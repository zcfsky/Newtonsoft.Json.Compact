using System;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000059 RID: 89
	public class JsonProperty
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00010FD1 File Offset: 0x0000F1D1
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00010FD9 File Offset: 0x0000F1D9
		public string PropertyName { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00010FE2 File Offset: 0x0000F1E2
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00010FEA File Offset: 0x0000F1EA
		public MemberInfo Member { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00010FF3 File Offset: 0x0000F1F3
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00010FFB File Offset: 0x0000F1FB
		public bool Ignored { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00011004 File Offset: 0x0000F204
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x0001100C File Offset: 0x0000F20C
		public bool Readable { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00011015 File Offset: 0x0000F215
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x0001101D File Offset: 0x0000F21D
		public bool Writable { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00011026 File Offset: 0x0000F226
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x0001102E File Offset: 0x0000F22E
		public JsonConverter MemberConverter { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00011037 File Offset: 0x0000F237
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x0001103F File Offset: 0x0000F23F
		public object DefaultValue { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00011048 File Offset: 0x0000F248
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00011050 File Offset: 0x0000F250
		public bool Required { get; set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00011059 File Offset: 0x0000F259
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00011061 File Offset: 0x0000F261
		public bool? IsReference { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x0001106A File Offset: 0x0000F26A
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00011072 File Offset: 0x0000F272
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x0001107B File Offset: 0x0000F27B
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00011083 File Offset: 0x0000F283
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x0001108C File Offset: 0x0000F28C
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x00011094 File Offset: 0x0000F294
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }
	}
}
