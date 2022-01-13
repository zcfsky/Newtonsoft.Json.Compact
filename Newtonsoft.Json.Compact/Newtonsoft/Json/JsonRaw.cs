using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000028 RID: 40
	public class JsonRaw
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000087AF File Offset: 0x000069AF
		public string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000087B7 File Offset: 0x000069B7
		public JsonRaw(string content)
		{
			ValidationUtils.ArgumentNotNull(content, "content");
			this._content = content;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000087D4 File Offset: 0x000069D4
		public override bool Equals(object obj)
		{
			JsonRaw jsonRaw = obj as JsonRaw;
			return jsonRaw != null && string.Compare(this._content, jsonRaw.Content, (StringComparison)5) == 0;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008802 File Offset: 0x00006A02
		public override int GetHashCode()
		{
			return this._content.GetHashCode();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000880F File Offset: 0x00006A0F
		public override string ToString()
		{
			return this._content;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008818 File Offset: 0x00006A18
		public static JsonRaw Create(JsonReader reader)
		{
			JsonRaw result;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					result = new JsonRaw(stringWriter.ToString());
				}
			}
			return result;
		}

		// Token: 0x040000A8 RID: 168
		private string _content;
	}
}
