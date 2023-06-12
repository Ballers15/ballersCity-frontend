using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public static class UIExtentions {

        public static void Recalculate(this Transform transform, UnityAction callback = null) {
            RectTransform rectTransform = transform as RectTransform;
            if (rectTransform)
                rectTransform.Recalculate(callback);
        }

        public static void RecalculateWithHorizontalFitterInside(this RectTransform rectTransform, ContentSizeFitter.FitMode fitMode) {
            ContentSizeFitter fitter = rectTransform.GetComponentInChildren<ContentSizeFitter>(true);
            fitter.horizontalFit = fitMode;

            HorizontalLayoutGroup hlg = rectTransform.GetComponentInParent<HorizontalLayoutGroup>();
            if (hlg)
                hlg.enabled = true;
            
            rectTransform.Recalculate(() => {
                if (fitter)
                    fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                
                if (hlg)
                    hlg.enabled = false;
            });
        }
        
        public static void RecalculateWithVerticalFitterInside(this RectTransform rectTransform, ContentSizeFitter.FitMode fitMode) {
            ContentSizeFitter fitter = rectTransform.GetComponentInChildren<ContentSizeFitter>(true);
            fitter.verticalFit = fitMode;
            
            VerticalLayoutGroup vlg = rectTransform.GetComponentInParent<VerticalLayoutGroup>();
            if (vlg)
                vlg.enabled = true;
            
            rectTransform.Recalculate(() => {
                fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                if (vlg)
                    vlg.enabled = false;
            });
        }

        public static void Recalculate(this RectTransform rectTransform, UnityAction callback = null) {
            Coroutines.StartRoutine(RecalculateContainerRoutine(rectTransform, callback));
        }

        private static IEnumerator RecalculateContainerRoutine(RectTransform container, UnityAction callback = null) {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);
            callback?.Invoke();
        }
    }
}