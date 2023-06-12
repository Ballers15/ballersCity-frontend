using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orego.Util
{
    public static class OregoDictionaryUtils
    {
        public static Dictionary<K, V> Clone<K, V>(this Dictionary<K, V> originMap)
        {
            return new Dictionary<K, V>(originMap);
        }
        
        public static void AddByType<T>(this Dictionary<Type, T> map, T item)
        {
            map[item.GetType()] = item;
        }

        public static void RemoveByType<T>(this Dictionary<Type, T> map, T item)
        {
            map.Remove(item.GetType());
        }

        public static void RemoveByType<T>(this Dictionary<Type, T> map)
        {
            map.Remove(typeof(T));
        }

        public static void AddRangeByType<T>(this Dictionary<Type, T> map, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                map.AddByType(item);
            }
        }

        public static void ForEachValue<K, V>(this Dictionary<K, V> map, Action<V> action)
        {
            var values = map.Values;
            foreach (var value in values)
            {
                action?.Invoke(value);
            }
        }

        public static R SmartGet<R, T>(this Dictionary<Type, T> map) where R : T
        {
            return (R) map.SmartGet(typeof(R));
        }

        public static T SmartGet<T>(this Dictionary<Type, T> map, Type requiredType)
        {
            if (map.ContainsKey(requiredType))
            {
                return map[requiredType];
            }
            
            var types = map.Keys;
            foreach (var type in types)
            {
                if (requiredType.IsAssignableFrom(type))
                {
                    var requiredItem = map[type];
                    map[requiredType] = requiredItem;
                    return requiredItem;
                }
            }

            throw new Exception($"Item of type {requiredType} not found!");
        }
    }
}