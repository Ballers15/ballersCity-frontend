using System;
using Orego.Util;
using UnityEngine.Events;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetButtonFortuneWheelSpin : UIWidgetAnim<UIWidgetButtonFortuneWheelSpin.Properties> {

        public override void Show() {
            this.gameObject.SetActive(true);
        }

        public void AddListener(UnityAction callback) {
            this.properties.button.AddListener(callback);
        }

        public void RemoveListener(UnityAction callback) {
            this.properties.button.RemoveListener(callback);
        }

        public void SetInteractable(bool isInteractable) {
            this.properties.button.interactable = isInteractable;
        }


        private void OnEnable() {
            this.properties.button.AddListener(this.OnClick);
        }

        private void OnDisable() {
            this.properties.button.RemoveListener(this.OnClick);
        }

        #region EVENTS

        private void OnClick() {
            SFX.PlayBtnClick();
        }

        #endregion


        [System.Serializable]
        public class Properties : UIProperties {
            public Button button;
        }
    }
}