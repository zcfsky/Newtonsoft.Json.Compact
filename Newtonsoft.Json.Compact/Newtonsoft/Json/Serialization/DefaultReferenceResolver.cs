using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000047 RID: 71
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000EFE6 File Offset: 0x0000D1E6
		private BidirectionalDictionary<string, object> Mappings
		{
			get
			{
				if (this._mappings == null)
				{
					this._mappings = new BidirectionalDictionary<string, object>();
				}
				return this._mappings;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000F004 File Offset: 0x0000D204
		public object ResolveReference(string reference)
		{
			object result;
			this.Mappings.TryGetByFirst(reference, out result);
			return result;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000F024 File Offset: 0x0000D224
		public string GetReference(object value)
		{
			string text;
			if (!this.Mappings.TryGetBySecond(value, out text))
			{
				this._referenceCount++;
				text = this._referenceCount.ToString(CultureInfo.InvariantCulture);
				this.Mappings.Add(text, value);
			}
			return text;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000F06E File Offset: 0x0000D26E
		public void AddReference(string reference, object value)
		{
			this.Mappings.Add(reference, value);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000F080 File Offset: 0x0000D280
		public bool IsReferenced(object value)
		{
			string text;
			return this.Mappings.TryGetBySecond(value, out text);
		}

		// Token: 0x04000100 RID: 256
		private int _referenceCount;

		// Token: 0x04000101 RID: 257
		private BidirectionalDictionary<string, object> _mappings;
	}
}
