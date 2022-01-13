using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000073 RID: 115
	internal static class CollectionUtils
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x00014AF3 File Offset: 0x00012CF3
		public static IEnumerable<T> CastValid<T>(this IEnumerable enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			return Enumerable.Cast<T>(Enumerable.Where<object>(Enumerable.Cast<object>(enumerable), (object o) => o is T));
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00014B1C File Offset: 0x00012D1C
		public static List<T> CreateList<T>(params T[] values)
		{
			return new List<T>(values);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014B24 File Offset: 0x00012D24
		public static bool IsNullOrEmpty(ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014B34 File Offset: 0x00012D34
		public static bool IsNullOrEmpty<T>(ICollection<T> collection)
		{
			return collection == null || collection.Count == 0;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00014B44 File Offset: 0x00012D44
		public static bool IsNullOrEmptyOrDefault<T>(IList<T> list)
		{
			return CollectionUtils.IsNullOrEmpty<T>(list) || ReflectionUtils.ItemsUnitializedValue<T>(list);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00014B58 File Offset: 0x00012D58
		public static IList<T> Slice<T>(IList<T> list, int? start, int? end)
		{
			return CollectionUtils.Slice<T>(list, start, end, default(int?));
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00014B78 File Offset: 0x00012D78
		public static IList<T> Slice<T>(IList<T> list, int? start, int? end, int? step)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (step == 0)
			{
				throw new ArgumentException("Step cannot be zero.", "step");
			}
			List<T> list2 = new List<T>();
			if (list.Count == 0)
			{
				return list2;
			}
			int num = step ?? 1;
			int num2 = start ?? 0;
			int num3 = end ?? list.Count;
			num2 = ((num2 < 0) ? (list.Count + num2) : num2);
			num3 = ((num3 < 0) ? (list.Count + num3) : num3);
			num2 = Math.Max(num2, 0);
			num3 = Math.Min(num3, list.Count - 1);
			for (int i = num2; i < num3; i += num)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00014C6C File Offset: 0x00012E6C
		public static Dictionary<K, List<V>> GroupBy<K, V>(ICollection<V> source, Func<V, K> keySelector)
		{
			if (keySelector == null)
			{
				throw new ArgumentNullException("keySelector");
			}
			Dictionary<K, List<V>> dictionary = new Dictionary<K, List<V>>();
			foreach (V v in source)
			{
				K k = keySelector.Invoke(v);
				List<V> list;
				if (!dictionary.TryGetValue(k, out list))
				{
					list = new List<V>();
					dictionary.Add(k, list);
				}
				list.Add(v);
			}
			return dictionary;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00014CF0 File Offset: 0x00012EF0
		public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
		{
			if (initial == null)
			{
				throw new ArgumentNullException("initial");
			}
			if (collection == null)
			{
				return;
			}
			foreach (T t in collection)
			{
				initial.Add(t);
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00014D4C File Offset: 0x00012F4C
		public static void AddRange(this IList initial, IEnumerable collection)
		{
			ValidationUtils.ArgumentNotNull(initial, "initial");
			ListWrapper<object> initial2 = new ListWrapper<object>(initial);
			initial2.AddRange(Enumerable.Cast<object>(collection));
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00014D78 File Offset: 0x00012F78
		public static List<T> Distinct<T>(List<T> collection)
		{
			List<T> list = new List<T>();
			foreach (T t in collection)
			{
				if (!list.Contains(t))
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00014DD8 File Offset: 0x00012FD8
		public static List<List<T>> Flatten<T>(params IList<T>[] lists)
		{
			List<List<T>> list = new List<List<T>>();
			Dictionary<int, T> currentSet = new Dictionary<int, T>();
			CollectionUtils.Recurse<T>(new List<IList<T>>(lists), 0, currentSet, list);
			return list;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014E00 File Offset: 0x00013000
		private static void Recurse<T>(IList<IList<T>> global, int current, Dictionary<int, T> currentSet, List<List<T>> flattenedResult)
		{
			IList<T> list = global[current];
			for (int i = 0; i < list.Count; i++)
			{
				currentSet[current] = list[i];
				if (current == global.Count - 1)
				{
					List<T> list2 = new List<T>();
					for (int j = 0; j < currentSet.Count; j++)
					{
						list2.Add(currentSet[j]);
					}
					flattenedResult.Add(list2);
				}
				else
				{
					CollectionUtils.Recurse<T>(global, current + 1, currentSet, flattenedResult);
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00014E78 File Offset: 0x00013078
		public static List<T> CreateList<T>(ICollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			T[] array = new T[collection.Count];
			collection.CopyTo(array, 0);
			return new List<T>(array);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00014EB0 File Offset: 0x000130B0
		public static bool ListEquals<T>(IList<T> a, IList<T> b)
		{
			if (a == null || b == null)
			{
				return a == null && b == null;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < a.Count; i++)
			{
				if (!@default.Equals(a[i], b[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00014F0D File Offset: 0x0001310D
		public static bool TryGetSingleItem<T>(IList<T> list, out T value)
		{
			return CollectionUtils.TryGetSingleItem<T>(list, false, out value);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014F34 File Offset: 0x00013134
		public static bool TryGetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty, out T value)
		{
			return MiscellaneousUtils.TryAction<T>(() => CollectionUtils.GetSingleItem<T>(list, returnDefaultIfEmpty), out value);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00014F67 File Offset: 0x00013167
		public static T GetSingleItem<T>(IList<T> list)
		{
			return CollectionUtils.GetSingleItem<T>(list, false);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00014F70 File Offset: 0x00013170
		public static T GetSingleItem<T>(IList<T> list, bool returnDefaultIfEmpty)
		{
			if (list.Count == 1)
			{
				return list[0];
			}
			if (returnDefaultIfEmpty && list.Count == 0)
			{
				return default(T);
			}
			throw new Exception("Expected single {0} in list but got {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				typeof(T),
				list.Count
			}));
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00014FDC File Offset: 0x000131DC
		public static IList<T> Minus<T>(IList<T> list, IList<T> minus)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			List<T> list2 = new List<T>(list.Count);
			foreach (T t in list)
			{
				if (minus == null || !minus.Contains(t))
				{
					list2.Add(t);
				}
			}
			return list2;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00015048 File Offset: 0x00013248
		public static IList CreateGenericList(Type listType)
		{
			ValidationUtils.ArgumentNotNull(listType, "listType");
			return (IList)ReflectionUtils.CreateGeneric(typeof(List<>), listType, new object[0]);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00015070 File Offset: 0x00013270
		public static IDictionary CreateGenericDictionary(Type keyType, Type valueType)
		{
			ValidationUtils.ArgumentNotNull(keyType, "keyType");
			ValidationUtils.ArgumentNotNull(valueType, "valueType");
			return (IDictionary)ReflectionUtils.CreateGeneric(typeof(Dictionary<,>), keyType, new object[]
			{
				valueType
			});
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000150B4 File Offset: 0x000132B4
		public static bool IsListType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return type.IsArray || typeof(IList).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IList));
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000150F4 File Offset: 0x000132F4
		public static bool IsCollectionType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return type.IsArray || typeof(ICollection).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(ICollection));
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00015134 File Offset: 0x00013334
		public static bool IsDictionaryType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			return typeof(IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof(IDictionary));
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000151B0 File Offset: 0x000133B0
		public static IWrappedCollection CreateCollectionWrapper(object list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type collectionDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(ICollection), out collectionDefinition))
			{
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(collectionDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						collectionDefinition
					});
					return constructor.Invoke(new object[]
					{
						list
					});
				};
				return (IWrappedCollection)ReflectionUtils.CreateGeneric(typeof(CollectionWrapper<>), new Type[]
				{
					collectionItemType
				}, instanceCreator, new object[]
				{
					list
				});
			}
			if (list is IList)
			{
				return new CollectionWrapper<object>((IList)list);
			}
			throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				list.GetType()
			}));
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000152E4 File Offset: 0x000134E4
		public static IWrappedList CreateListWrapper(object list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			Type listDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(list.GetType(), typeof(IList), out listDefinition))
			{
				Type collectionItemType = ReflectionUtils.GetCollectionItemType(listDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						listDefinition
					});
					return constructor.Invoke(new object[]
					{
						list
					});
				};
				return (IWrappedList)ReflectionUtils.CreateGeneric(typeof(ListWrapper<>), new Type[]
				{
					collectionItemType
				}, instanceCreator, new object[]
				{
					list
				});
			}
			if (list is IList)
			{
				return new ListWrapper<object>((IList)list);
			}
			throw new Exception("Can not create ListWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				list.GetType()
			}));
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00015418 File Offset: 0x00013618
		public static IWrappedDictionary CreateDictionaryWrapper(object dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			Type dictionaryDefinition;
			if (ReflectionUtils.ImplementsGenericDefinition(dictionary.GetType(), typeof(IDictionary), out dictionaryDefinition))
			{
				Type dictionaryKeyType = ReflectionUtils.GetDictionaryKeyType(dictionaryDefinition);
				Type dictionaryValueType = ReflectionUtils.GetDictionaryValueType(dictionaryDefinition);
				Func<Type, IList<object>, object> instanceCreator = delegate(Type t, IList<object> a)
				{
					ConstructorInfo constructor = t.GetConstructor(new Type[]
					{
						dictionaryDefinition
					});
					return constructor.Invoke(new object[]
					{
						dictionary
					});
				};
				return (IWrappedDictionary)ReflectionUtils.CreateGeneric(typeof(DictionaryWrapper<, >), new Type[]
				{
					dictionaryKeyType,
					dictionaryValueType
				}, instanceCreator, new object[]
				{
					dictionary
				});
			}
			if (dictionary is IDictionary)
			{
				return new DictionaryWrapper<object, object>((IDictionary)dictionary);
			}
			throw new Exception("Can not create DictionaryWrapper for type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
			{
				dictionary.GetType()
			}));
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00015528 File Offset: 0x00013728
		public static IList CreateAndPopulateList(Type listType, Action<IList, bool> populateList)
		{
			ValidationUtils.ArgumentNotNull(listType, "listType");
			ValidationUtils.ArgumentNotNull(populateList, "populateList");
			bool flag = false;
			IList list;
			Type type;
			if (listType.IsArray)
			{
				list = new List<object>();
				flag = true;
			}
			else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection<>), out type))
			{
				Type type2 = type.GetGenericArguments()[0];
				Type type3 = ReflectionUtils.MakeGenericType(typeof(IEnumerable), new Type[]
				{
					type2
				});
				bool flag2 = false;
				foreach (ConstructorInfo constructorInfo in listType.GetConstructors())
				{
					IList<ParameterInfo> parameters = constructorInfo.GetParameters();
					if (parameters.Count == 1 && type3.IsAssignableFrom(parameters[0].ParameterType))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					throw new Exception("Read-only type {0} does not have a public constructor that takes a type that implements {1}.".FormatWith(CultureInfo.InvariantCulture, new object[]
					{
						listType,
						type3
					}));
				}
				list = CollectionUtils.CreateGenericList(type2);
				flag = true;
			}
			else if (typeof(IList).IsAssignableFrom(listType))
			{
				if (ReflectionUtils.IsInstantiatableType(listType))
				{
					list = (IList)Activator.CreateInstance(listType);
				}
				else if (listType == typeof(IList))
				{
					list = new List<object>();
				}
				else
				{
					list = null;
				}
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(listType, typeof(IList)))
			{
				list = CollectionUtils.CreateGenericList(ReflectionUtils.GetCollectionItemType(listType));
			}
			else
			{
				list = null;
			}
			if (list == null)
			{
				throw new Exception("Cannot create and populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, new object[]
				{
					listType
				}));
			}
			populateList.Invoke(list, flag);
			if (flag)
			{
				if (listType.IsArray)
				{
					list = CollectionUtils.ToArray(((List<object>)list).ToArray(), ReflectionUtils.GetCollectionItemType(listType));
				}
				else if (ReflectionUtils.InheritsGenericDefinition(listType, typeof(ReadOnlyCollection<>)))
				{
					list = (IList)ReflectionUtils.CreateInstance(listType, new object[]
					{
						list
					});
				}
			}
			return list;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00015714 File Offset: 0x00013914
		public static Array ToArray(Array initial, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Array array = Array.CreateInstance(type, initial.Length);
			Array.Copy(initial, 0, array, 0, initial.Length);
			return array;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001574C File Offset: 0x0001394C
		public static bool AddDistinct<T>(this IList<T> list, T value)
		{
			return list.AddDistinct(value, EqualityComparer<T>.Default);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001575A File Offset: 0x0001395A
		public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
		{
			if (list.ContainsValue(value, comparer))
			{
				return false;
			}
			list.Add(value);
			return true;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00015770 File Offset: 0x00013970
		public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			foreach (TSource tsource in source)
			{
				if (comparer.Equals(tsource, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000157DC File Offset: 0x000139DC
		public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values)
		{
			return list.AddRangeDistinct(values, EqualityComparer<T>.Default);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000157EC File Offset: 0x000139EC
		public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
		{
			bool result = true;
			foreach (T value in values)
			{
				if (!list.AddDistinct(value, comparer))
				{
					result = false;
				}
			}
			return result;
		}
	}
}
