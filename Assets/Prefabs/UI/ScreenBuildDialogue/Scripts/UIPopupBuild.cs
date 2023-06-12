using SinSity.Core;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIPopupBuild : UIDialogue<UIDialogueBuildProperties, UIPopupArgs> {

        public IdleObject lastBuildedObject { get; private set; }
        private IdleObject currentIdleObject;
        
        public override void Initialize()
        {
            base.Initialize();
            HideInstantly();
        }

        public void Setup(IdleObject idleObject) {
            this.currentIdleObject = idleObject;
            SetHeader(idleObject);
            SetIcon(idleObject);
            SetDescription(idleObject);
            SetPrice(idleObject);
            SFX.PlaySFX(this.properties.audioClipShow);
        }

        private void SetHeader(IdleObject idleObject)
        {
            var headerText = Localization.GetTranslation(idleObject.info.GetTitle());
            properties.textHeader.text = headerText;
        }

        private void SetIcon(IdleObject idleObject)
        {
            properties.imgIcon.sprite = idleObject.info.spriteIcon;
        }

        private void SetDescription(IdleObject idleObject)
        {
            properties.textDescription.text = Localization.GetTranslation(idleObject.info.GetDescription());
        }

        private void SetPrice(IdleObject idleObject)
        {
            properties.panelPrice.SetPrice(idleObject.info.priceToBuild);
        }

        private void OnEnable()
        {
            properties.btnClose.onClick.AddListener(OnCloseBtnClick);
            properties.btnBuild.onClick.AddListener(OnBuildBtnClick);
        }

        private void OnCloseBtnClick()
        {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            Hide();
            SFX.PlaySFX(this.properties.audioClipClose);
        }

        public void OnBuildBtnClick() {
            this.lastBuildedObject = this.currentIdleObject;
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Apply));
            Hide();
        }

        private void OnDisable()
        {
            properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
            properties.btnBuild.onClick.RemoveListener(OnBuildBtnClick);
        }
    }
}