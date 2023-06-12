using SinSity.Core;
using SinSity.Domain.Utils;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupUpgrade : UIDialogue<UIPopupUpgradeProperties, UIPopupArgs> {
        private IdleObject currentIdleObject;
        
        public override void Initialize() {
            base.Initialize();
            HideInstantly();
        }

        public void Setup(IdleObject idleObject) {
            currentIdleObject = idleObject;
            SetHeader();
            SetIcons();
            SetDescription();
            properties.widgetLevelUp.Setup(idleObject);
            SFX.PlaySFX(properties.audioClipShow);
        }

        private void SetHeader() {
            var headerText = Localization.GetTranslation(currentIdleObject.info.GetTitle());
            properties.textHeader.text = headerText;
        }

        private void SetIcons() {
            properties.imgIcon.sprite = currentIdleObject.info.spriteIcon;
            properties.imgProductIcon.sprite = currentIdleObject.info.productIcon;
        }

        private void SetDescription() {
            properties.textDescription.text = Localization.GetTranslation(currentIdleObject.info.GetDescription());
        }

        private void OnEnable() {
            properties.btnClose.onClick.AddListener(OnCloseBtnClick);
            properties.btnPrev.onClick.AddListener(OnPrevClicked);
            properties.btnNext.onClick.AddListener(OnNextClicked);
        }

        private void OnCloseBtnClick() {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            Hide();
            SFX.PlaySFX(this.properties.audioClipClose);
        }
        
        private void OnPrevClicked() {
            var idleObjectsInteractor = GetInteractor<IdleObjectsInteractor>();
            var prevIdleObject = idleObjectsInteractor.GetPreviousBuiltObject(currentIdleObject);
            SwitchToNewIdle(prevIdleObject);
        }

        private void SwitchToNewIdle(IdleObject newIdle) {
            properties.contentContainer.gameObject.SetActive(false);
            Setup(newIdle);
            properties.contentContainer.gameObject.SetActive(true);
        }

        private void OnNextClicked() {
            var idleObjectsInteractor = GetInteractor<IdleObjectsInteractor>();
            var prevIdleObject = idleObjectsInteractor.GetNextBuiltObject(currentIdleObject);
            SwitchToNewIdle(prevIdleObject);
        }

        private void OnDisable() {
            properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
            properties.btnPrev.onClick.RemoveListener(OnPrevClicked);
            properties.btnNext.onClick.RemoveListener(OnNextClicked);
        }
    }
}