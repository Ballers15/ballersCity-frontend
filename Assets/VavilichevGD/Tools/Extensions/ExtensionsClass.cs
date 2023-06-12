using System.Collections.Generic;
using Orego.Util;
using UnityEngine;

namespace VavilichevGD.Extensions
{
    public static class ExtensionsClass
    {
        public static T GetRandom<T>(this T[] array, T currentItem)
        {
            var availableList = new List<T>(array);
            availableList.Remove(currentItem);
            if (availableList.IsEmpty())
            {
                return currentItem;
            }

            var rIndex = Random.Range(0, availableList.Count);
            return availableList[rIndex];
        }
    }
}