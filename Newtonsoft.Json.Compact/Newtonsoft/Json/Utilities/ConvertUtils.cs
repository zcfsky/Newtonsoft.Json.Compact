using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000065 RID: 101
	internal static class ConvertUtils
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x00013284 File Offset: 0x00011484
		public static bool CanConvertType(Type initialType, Type targetType, bool allowTypeNameToString)
		{
			ValidationUtils.ArgumentNotNull(initialType, "initialType");
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			return targetType == initialType || (typeof(IConvertible).IsAssignableFrom(initialType) && typeof(IConvertible).IsAssignableFrom(targetType)) || (initialType == typeof(DateTime) && targetType == typeof(DateTimeOffset)) || (initialType == typeof(Guid) && (targetType == typeof(Guid) || targetType == typeof(string))) || (initialType == typeof(Type) && targetType == typeof(string)) || (initialType == typeof(DBNull) && ReflectionUtils.IsNullable(targetType));
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001335C File Offset: 0x0001155C
		private static bool IsComponentConverter(TypeConverter converter)
		{
			return false;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001335F File Offset: 0x0001155F
		public static T Convert<T>(object initialValue)
		{
			return ConvertUtils.Convert<T>(initialValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001336C File Offset: 0x0001156C
		public static T Convert<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.Convert(initialValue, culture, typeof(T)));
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013384 File Offset: 0x00011584
		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				return initialValue;
			}
			if (initialValue is string && typeof(Type).IsAssignableFrom(targetType))
			{
				return Type.GetType((string)initialValue, true);
			}
			if (targetType.IsInterface || targetType.IsGenericTypeDefinition || targetType.IsAbstract)
			{
				throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					targetType
				}), "targetType");
			}
			if (initialValue is IConvertible && typeof(IConvertible).IsAssignableFrom(targetType))
			{
				if (targetType.IsEnum)
				{
					if (initialValue is string)
					{
						return Enum.Parse(targetType, initialValue.ToString(), true);
					}
					if (ConvertUtils.IsInteger(initialValue))
					{
						return Enum.ToObject(targetType, initialValue);
					}
				}
				return System.Convert.ChangeType(initialValue, targetType, culture);
			}
			if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
			{
				return new DateTimeOffset((DateTime)initialValue);
			}
			if (initialValue is string)
			{
				if (targetType == typeof(Guid))
				{
					return new Guid((string)initialValue);
				}
				if (targetType == typeof(Uri))
				{
					return new Uri((string)initialValue);
				}
			}
			if (initialValue == DBNull.Value)
			{
				if (ReflectionUtils.IsNullable(targetType))
				{
					return ConvertUtils.EnsureTypeAssignable(null, type, targetType);
				}
				throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type,
					targetType
				}));
			}
			else
			{
				if (initialValue is INullable)
				{
					return ConvertUtils.EnsureTypeAssignable(ConvertUtils.ToValue((INullable)initialValue), type, targetType);
				}
				throw new Exception("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					type,
					targetType
				}));
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00013554 File Offset: 0x00011754
		public static bool TryConvert<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvert<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001359C File Offset: 0x0001179C
		public static bool TryConvert<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvert(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000135EC File Offset: 0x000117EC
		public static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.Convert(initialValue, culture, targetType), out convertedValue);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013626 File Offset: 0x00011826
		public static T ConvertOrCast<T>(object initialValue)
		{
			return ConvertUtils.ConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013633 File Offset: 0x00011833
		public static T ConvertOrCast<T>(object initialValue, CultureInfo culture)
		{
			return (T)((object)ConvertUtils.ConvertOrCast(initialValue, culture, typeof(T)));
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001364C File Offset: 0x0001184C
		public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
		{
			object result;
			if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
			{
				return result;
			}
			return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00013674 File Offset: 0x00011874
		public static bool TryConvertOrCast<T>(object initialValue, out T convertedValue)
		{
			return ConvertUtils.TryConvertOrCast<T>(initialValue, CultureInfo.CurrentCulture, out convertedValue);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000136BC File Offset: 0x000118BC
		public static bool TryConvertOrCast<T>(object initialValue, CultureInfo culture, out T convertedValue)
		{
			return MiscellaneousUtils.TryAction<T>(delegate
			{
				object obj;
				ConvertUtils.TryConvertOrCast(initialValue, CultureInfo.CurrentCulture, typeof(T), out obj);
				return (T)((object)obj);
			}, out convertedValue);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001370C File Offset: 0x0001190C
		public static bool TryConvertOrCast(object initialValue, CultureInfo culture, Type targetType, out object convertedValue)
		{
			return MiscellaneousUtils.TryAction<object>(() => ConvertUtils.ConvertOrCast(initialValue, culture, targetType), out convertedValue);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013748 File Offset: 0x00011948
		private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
		{
			Type type = (value != null) ? value.GetType() : null;
			if (value != null && targetType.IsAssignableFrom(type))
			{
				return value;
			}
			if (value == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new Exception("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				(initialType != null) ? initialType.ToString() : "{null}",
				targetType
			}));
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000137B0 File Offset: 0x000119B0
		public static object ToValue(INullable nullableValue)
		{
			if (nullableValue == null)
			{
				return null;
			}
			if (nullableValue is SqlInt32)
			{
				return ConvertUtils.ToValue((SqlInt32)nullableValue);
			}
			if (nullableValue is SqlInt64)
			{
				return ConvertUtils.ToValue((SqlInt64)nullableValue);
			}
			if (nullableValue is SqlBoolean)
			{
				return ConvertUtils.ToValue((SqlBoolean)nullableValue);
			}
			if (nullableValue is SqlString)
			{
				return ConvertUtils.ToValue((SqlString)nullableValue);
			}
			if (nullableValue is SqlDateTime)
			{
				return ConvertUtils.ToValue((SqlDateTime)nullableValue);
			}
			throw new Exception("Unsupported INullable type: {0}".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				nullableValue.GetType()
			}));
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013864 File Offset: 0x00011A64
		public static bool IsInteger(object value)
		{
			switch ((int)System.Convert.GetTypeCode(value))
			{
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return true;
			default:
				return false;
			}
		}
	}
}
