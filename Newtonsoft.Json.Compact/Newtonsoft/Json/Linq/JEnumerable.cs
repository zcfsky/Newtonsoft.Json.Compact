using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x02000031 RID: 49
	public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable where T : JToken
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000C248 File Offset: 0x0000A448
		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			this._enumerable = enumerable;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000C25C File Offset: 0x0000A45C
		public IEnumerator<T> GetEnumerator()
		{
			return this._enumerable.GetEnumerator();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000C269 File Offset: 0x0000A469
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000092 RID: 146
		public IJEnumerable<JToken> this[object key]
		{
			get
			{
                return new JEnumerable<JToken>(Extensions.Values<T,JToken>(this._enumerable,key));
			}
		}

		// Token: 0x040000CC RID: 204
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		// Token: 0x040000CD RID: 205
		private IEnumerable<T> _enumerable;
	}
}
