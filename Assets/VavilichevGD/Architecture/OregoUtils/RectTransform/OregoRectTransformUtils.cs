using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Orego.Util
{
    public static class OregoRectTransformUtils
    {
        #region Const

        private const float HALF_COEFFICIENT = 0.5f;

        #endregion

        public static float GetWidth(this RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            var width = rect.width;
            return width;
        }

        public static float GetHalfWidth(this RectTransform rectTransform)
        {
            var width = rectTransform.GetWidth();
            return width * HALF_COEFFICIENT;
        }

        public static float GetHeight(this RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            var height = rect.height;
            return height;
        }

        public static float GetHalfHeight(this RectTransform rectTransform)
        {
            var height = rectTransform.GetHeight();
            return height * HALF_COEFFICIENT;
        }

        public static float GetLeftBorder(this RectTransform rectTransform)
        {
            return rectTransform.position.x - rectTransform.GetHalfWidth();
        }

        public static float GetRightBorder(this RectTransform rectTransform)
        {
            return rectTransform.position.x + rectTransform.GetHalfWidth();
        }

        public static float GetTopBorder(this RectTransform rectTransform)
        {
            return rectTransform.position.y + rectTransform.GetHalfHeight();
        }

        public static float GetBottomBorder(this RectTransform rectTransform)
        {
            return rectTransform.position.y - rectTransform.GetHalfHeight();
        }

        public static Vector2 GetTopLeftCorner(this RectTransform rectTransform)
        {
            return new Vector2(rectTransform.GetLeftBorder(), rectTransform.GetTopBorder());
        }
        
        public static Vector2 GetBottomLeftCorner(this RectTransform rectTransform)
        {
            return new Vector2(rectTransform.GetLeftBorder(), rectTransform.GetBottomBorder());
        }
        
        public static void RecalculateContainer(this MonoBehaviour behaviour, RectTransform container)
        {
            behaviour.StartCoroutine(RecalculateContainerRoutine(container));
        }

        public static IEnumerator RecalculateContainerRoutine(RectTransform container)
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);
        }
    }
}