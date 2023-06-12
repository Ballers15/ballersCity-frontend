using Orego.Util;
using SinSity.Analytics;
using UnityEngine;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIPopupADLoading : UIPopupAnim<UIPopupADLoadingProperties, UIPopupArgs>
    {
        private static readonly int triggerError = Animator.StringToHash("error");

        public delegate void ADResultsHandler(UIPopupADLoading popup, bool success, string error = "");

        public event ADResultsHandler OnADResultsReceived;

        public void ShowAD(string placementId) {
            ADS.ShowRewardedVideo(placementId, this.ADSCallback);
        }

        private void ADSCallback(bool success, ADResult result, string error) {
            if (!success)
                this.SetTrigger(triggerError);
            else
                this.HideInstantly();

            this.OnADResultsReceived?.Invoke(this, success);
        }

        private void OnEnable()
        {
            properties.btnOk.AddListener(OnOkBtnClick);
        }

        private void OnOkBtnClick()
        {
            Hide();
        }

        private void OnDisable()
        {
            properties.btnOk.RemoveListener(OnOkBtnClick);
            UIController.NotifyAboutStateChanged();
        }
    }
}