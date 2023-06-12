using System;
using System.Collections.Generic;

namespace Orego.Util
{
    public static class OregoArrayUtils
    {
        public static T GetRandom<T>(this T[] array)
        {
            if (array.Length == 0)
            {
                throw new Exception("Array is empty!");
            }
            
            var random = new Random();
            var randomIndex = random.Next(0, array.Length);
            return array[randomIndex];
        }

        public static T[] ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var e in array) action?.Invoke(e);

            return array;
        }

        public static bool IsEmpty<T>(this T[] array)
        {
            return array.Length == 0;
        }

        public static bool IsNotEmpty<T>(this T[] array)
        {
            return array.Length > 0;
        }
        
        public static T[] Foreach<T>(this T[] array, Action<T> action)
        {
            foreach (var e in array) action?.Invoke(e);

            return array;
        }

        public static Array<T> Clone<T>(this Array<T> array)
        {
            var count = array.count;
            var cloneArray = new Array<T>(count);
            for (var i = 0; i < count; i++)
            {
                var item = array[i];
                cloneArray[i] = item;
            }

            return cloneArray;
        }

        public static List<T> ToList<T>(this Array<T> array)
        {
            var list = new List<T>();
            var count = array.count;
            for (var i = 0; i < count; i++)
            {
                var item = array[i];
                list.Add(item);
            }

            return list;
        }

        public static Array<R> Map<T, R>(this Array<T> array, Func<T, R> transformFunc)
        {
            var count = array.count;
            var resultArray = new Array<R>(count);
            for (var i = 0; i < count; i++)
            {
                var item = array[i];
                var transformItem = transformFunc.Invoke(item);
                resultArray[i] = transformItem;
            }

            return resultArray;
        }
    }
}