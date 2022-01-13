using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x0200002D RID: 45
	public static class Extensions
	{
		// Token: 0x06000257 RID: 599 RVA: 0x00009546 File Offset: 0x00007746
		public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Ancestors()).AsJEnumerable();
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009579 File Offset: 0x00007779
		public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T j) => j.Descendants()).AsJEnumerable();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000095A5 File Offset: 0x000077A5
		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<JObject, JProperty>(source, (JObject d) => d.Properties()).AsJEnumerable<JProperty>();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000095DA File Offset: 0x000077DA
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
		{
			return source.Values(key).AsJEnumerable();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000095E8 File Offset: 0x000077E8
		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000095F1 File Offset: 0x000077F1
		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values<U>(key);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000095FA File Offset: 0x000077FA
		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
		{
			return source.Values<U>(null);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009603 File Offset: 0x00007803
		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000960C File Offset: 0x0000780C
		public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "source");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return jtoken.Convert<JToken, U>();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000993C File Offset: 0x00007B3C
		internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T t2 in source)
			{
				JToken token = t2;
				if (key == null)
				{
					if (token is JValue)
					{
						yield return ((JValue)token).Convert<JValue, U>();
					}
					else
					{
						foreach (JToken t in token.Children())
						{
							yield return t.Convert<JToken, U>();
						}
					}
				}
				else
				{
					JToken value = token[key];
					if (value != null)
					{
						yield return value.Convert<JToken, U>();
					}
				}
			}
			yield break;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009960 File Offset: 0x00007B60
		public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009981 File Offset: 0x00007B81
		public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return Enumerable.SelectMany<T, JToken>(source, (T c) => c.Children()).Convert<JToken, U>();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009B7C File Offset: 0x00007D7C
		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			bool cast = typeof(JToken).IsAssignableFrom(typeof(U));
			foreach (T t in source)
			{
				JToken token = t;
				yield return Convert<JToken,U>(token,cast);
			}
			yield break;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009B9C File Offset: 0x00007D9C
		internal static U Convert<T, U>(this T token) where T : JToken
		{
			bool cast = typeof(JToken).IsAssignableFrom(typeof(U));
            return Convert<JToken, U>(token, cast);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009BCC File Offset: 0x00007DCC
		internal static U Convert<T, U>(this T token, bool cast) where T : JToken
		{
			if (cast)
			{
				return (U)((object)token);
			}
			if (token == null)
			{
				return default(U);
			}
			JValue jvalue = token as JValue;
			if (jvalue == null)
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					token.GetType(),
					typeof(T)
				}));
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jvalue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)((object)System.Convert.ChangeType(jvalue.Value, type, CultureInfo.InvariantCulture));
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009C86 File Offset: 0x00007E86
		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009C8E File Offset: 0x00007E8E
		public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			if (source is IJEnumerable<T>)
			{
				return (IJEnumerable<T>)source;
			}
			return new JEnumerable<T>(source);
		}
	}
}
