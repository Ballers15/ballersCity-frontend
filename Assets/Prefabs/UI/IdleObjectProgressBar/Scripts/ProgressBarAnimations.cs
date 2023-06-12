using System.Collections;
using UnityEngine;

namespace VavilichevGD.UI {
    public class ProgressBarAnimations : MonoBehaviour {

        #region Delegates

        public delegate void ProgressBarAnimationHandler(ProgressBarAnimations progressBarAnimations);

        public event ProgressBarAnimationHandler OnAnimationStart;
        public event ProgressBarAnimationHandler OnAnimationOver;

        #endregion
        
        [SerializeField] private ProgressBar m_progressBar;
        [SerializeField] protected float speed = 1f;

        public ProgressBar progressBar => m_progressBar;

        
        /// <summary>
        /// Play progress animation. From start value notmalized (0-1) to value normalized (0-1).
        /// </summary>
        /// <param name="startValueNormalized"></param>
        /// <param name="endValueNotmalized"></param>
        public virtual void PlayProgress(float startValueNormalized, float targetValueNotmalized) {
            this.StartCoroutine(AnimationRoutine(startValueNormalized, targetValueNotmalized));
        }

        protected virtual IEnumerator AnimationRoutine(float startValueNormalized, float targetValueNormalized) {
            OnAnimationStart?.Invoke(this);
            
            float progress = 0f;
            while (progress < 1f) {
                progress = Mathf.Min(progress + Time.deltaTime * speed, 1f);
                float newValue = Mathf.Lerp(startValueNormalized, targetValueNormalized, progress);
                this.progressBar.SetValue(newValue);
                yield return null;
            }
            
            OnAnimationOver?.Invoke(this);
        }
    }
}