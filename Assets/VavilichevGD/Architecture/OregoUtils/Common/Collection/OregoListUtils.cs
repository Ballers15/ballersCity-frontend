using System;
using System.Collections.Generic;

namespace Orego.Util
{
    public static class OregoListUtils
    {
        #region Const

        private const int FIRST_INDEX = 0;

        #endregion

        public static T GetFirst<T>(this List<T> collection)
        {
            return collection[FIRST_INDEX];
        }


        public static int GetRandomIndex<T>(this List<T> collection)
        {
            if (collection.IsEmpty())
            {
                throw new Exception("List is empty!");
            }

            var randomIndex = new Random().Next(0, collection.Count);
            return randomIndex;
        }

        public static T GetRandom<T>(this List<T> collection)
        {
            var randomIndex = collection.GetRandomIndex();
            return collection[randomIndex];
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        public static bool IsNotEmpty<T>(this List<T> list)
        {
            return list.Count > 0;
        }
    }
}