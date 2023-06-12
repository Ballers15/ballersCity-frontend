using System.Collections;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
    public abstract class UIPanelNotification<T> : UIPanelAnim<T> where T : UIProperties {
        [SerializeField] protected float lifetime = 3f;
        [SerializeField] protected Button btn;

        public override void Initialize() {
            base.Initialize();
            HideInstantly();
        }

        protected virtual void OnEnable() {
            StartLifeTime();
            
            btn.AddListener(OnClick);
        }

        protected void StartLifeTime() {
            StartCoroutine("LifeRoutine");
        }
        
        private IEnumerator LifeRoutine() {
            yield return new WaitForSecondsRealtime(lifetime);
            Hide();
        }

        protected void OnDisable() {
            btn.RemoveListener(OnClick);
        }

        protected void StopLifeTime() {
            StopCoroutine("LifeRoutine");
        }

        protected virtual void OnClick() {
            StopLifeTime();
            Hide();
        }
    }
}