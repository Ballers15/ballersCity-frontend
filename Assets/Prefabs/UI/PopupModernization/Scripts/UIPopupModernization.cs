using System;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupModernization : UIPopupAnim<UIPopupModernizationProperties, UIPopupArgs> {
        public static event Action OnHintClickedEvent;

        private void OnEnable() {
            properties.btnApply.onClick.AddListener(OnApplyBtnClick);
            properties.btnHint.onClick.AddListener(OnHintBtnClick);
            properties.btnClose.onClick.AddListener(OnCloseBtnClick);
        }

        private void OnApplyBtnClick() {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Apply));
            Hide();
            SFX.PlaySFX(this.properties.audioClipApply);
        }

        private void OnCloseBtnClick() {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            Hide();
            SFX.PlaySFX(this.properties.audioClipClose);
        }

        private void OnHintBtnClick()
        {
            var uiInteractor = GetInteractor<UIInteractor>();
            var popupHint = uiInteractor.GetUIElement<UIPopupModernizationInfo>();
            popupHint.SetPopupType(UIPopupModernizationInfoProperties.PopupType.Modernization);
            popupHint.Show();
        }

        private void OnDisable() {
            properties.btnApply.onClick.RemoveListener(OnApplyBtnClick);
            properties.btnHint.onClick.RemoveListener(OnHintBtnClick);
            properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
        }
    }
}