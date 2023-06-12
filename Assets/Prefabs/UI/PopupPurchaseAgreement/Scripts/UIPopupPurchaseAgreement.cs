using Orego.Util;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupPurchaseAgreement : UIPopupAnim<UIPopupPurchaseAgreementProperties, UIPopupArgs> {

        public void Setup(string titleText) {
            this.properties.textTitle.text = titleText;
        }

        private void OnEnable() {
            this.properties.btnYes.AddListener(OnYesBtnClick);
            this.properties.btnNo.AddListener(OnNoBtnClick);
        }

        private void OnYesBtnClick() {
            UIPopupArgs args = new UIPopupArgs(this, UIPopupResult.Apply);
            NotifyAboutResults(args);
            this.Hide();

            SFX.PlayBtnClick();
        }

        private void OnNoBtnClick() {
            UIPopupArgs args = new UIPopupArgs(this, UIPopupResult.Close);
            NotifyAboutResults(args);
            this.Hide();
            
            SFX.PlayBtnClick();
        }

        private void OnDisable() {
            this.properties.btnYes.RemoveListener(OnYesBtnClick);
            this.properties.btnNo.RemoveListener(OnNoBtnClick);
        }
    }
}