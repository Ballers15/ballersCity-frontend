#if UNITY_EDITOR
using Orego.Util;
using UnityEditor;
using UnityEngine;

namespace Orego.Editor
{
    public static class OregoSnapAnchors
    {
        [MenuItem("Orego/SnapAnchors")]
        private static void SnapParentMenuItem()
        {
            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject gameObject)
                {
                    Snap(gameObject);
                }
            }
        }

        internal static void Snap(GameObject gameObject)
        {
            var parent = gameObject.transform.parent;
            if (parent == null)
            {
                return;
            }

            if (!gameObject.Has(typeof(RectTransform)))
            {
                return;
            }

            var recTransform = gameObject.Get<RectTransform>();
            var parentTransform = parent.Get<RectTransform>();
            Undo.RecordObject(recTransform, "SnapAnchors");
            var offsetMin = recTransform.offsetMin;
            var offsetMax = recTransform.offsetMax;
            var anchorMin = recTransform.anchorMin;
            var anchorMax = recTransform.anchorMax;
            var parentScale = new Vector2(
                parentTransform.rect.width,
                parentTransform.rect.height
            );
            recTransform.anchorMin = new Vector2(
                anchorMin.x + offsetMin.x / parentScale.x,
                anchorMin.y + offsetMin.y / parentScale.y
            );
            recTransform.anchorMax = new Vector2(
                anchorMax.x + offsetMax.x / parentScale.x,
                anchorMax.y + offsetMax.y / parentScale.y
            );
            var zero = Vector2.zero;
            recTransform.offsetMin = zero;
            recTransform.offsetMax = zero;
        }
    }
}
#endif