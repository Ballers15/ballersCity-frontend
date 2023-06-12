using System.Collections.Generic;
using UnityEngine;

namespace Orego.Util
{
    public static class OregoFindComponentUtils
    {
        public static IEnumerable<T> FindChildrenRecursively<T>(this Component component)
            where T : Component
        {
            var requieredComponents = new List<T>();
            var transform = component.transform;
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                var childTransform = transform.GetChild(i);
                requieredComponents.AddRange(childTransform.Children<T>());
            }

            return requieredComponents;
        }
    }
}