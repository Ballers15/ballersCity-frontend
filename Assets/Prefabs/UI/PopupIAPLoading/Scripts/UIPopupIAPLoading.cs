using System;
using System.Collections;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupIAPLoading : UIPopupAnim<UIPopupIAPLoading.Properties, UIPopupArgs> {
        
        [Serializable]
        public class Properties : UIProperties {
            public Button buttonClose;
            public int crossButtonEnableDelay = 3;
        }

        public override void Show() {
            this.properties.buttonClose.gameObject.SetActive(false);
            base.Show();
        }

        private void OnEnable() {
            this.properties.buttonClose.AddListener(this.OnCloseButtonClick);
            this.StartCoroutine("LifeRoutine");
        }

        private void OnDisable() {
            this.properties.buttonClose.RemoveListener(this.OnCloseButtonClick);
            this.StopCoroutine("LifeRoutine");
        }

        private IEnumerator LifeRoutine() {
            yield return new WaitForSecondsRealtime(this.properties.crossButtonEnableDelay);
            this.properties.buttonClose.gameObject.SetActive(true);
        }

        #region EVENTS

        private void OnCloseButtonClick() {
            SFX.PlayClosePopup();
            this.Hide();
            var args = new UIPopupArgs(this, UIPopupResult.Close);
            this.NotifyAboutResults(args);
        }
        
        #endregion
    }
}