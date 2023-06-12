using SinSity.Achievements;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupSettings : UIPopupAnim<UIPopupSettingProperties, UIPopupArgs>
    {
        [SerializeField] 
        private AudioClip audioClipClick;

        [SerializeField]
        private AudioClip audioClipCloseClick;
        
        private const string URL_PRIVACY_POLICY = "https://idle-ecoclicker-sav.flycricket.io/privacy.html";

        private void OnEnable()
        {
            this.properties.btnLanguage.onClick.AddListener(this.OnLanguageBtnClick);
            this.properties.btnGDPR.onClick.AddListener(this.OnGDPRBtnClick);
            this.properties.btnBack.onClick.AddListener(this.OnBackBtnClick);
            properties.btnAchievements.onClick.AddListener(OnAchievementBtnClick);
            Localization.OnLanguageChanged += this.OnLanguageChanged;
            this.isActive = true;
            
            this.UpdateLanguageTitle();
        }

        private void UpdateLanguageTitle() {
            this.properties.textLanguageTitle.text = Localization.GetLanguageTitle();
        }
        
        private void OnLanguageBtnClick()
        {
            SFX.PlaySFX(this.audioClipClick);
            Localization.SwitchToNextLanguage();
        }

        private void OnGDPRBtnClick()
        {
            SFX.PlaySFX(this.audioClipClick);
            Application.OpenURL(URL_PRIVACY_POLICY);
        }

        private void OnBackBtnClick()
        {
            SFX.PlaySFX(this.audioClipCloseClick);
            this.Hide();
        }

        private void OnAchievementBtnClick()
        {
            Achievement.ShowAchievementsList();
        }

        private void OnDisable()
        {
            this.properties.btnLanguage.onClick.RemoveListener(this.OnLanguageBtnClick);
            this.properties.btnGDPR.onClick.RemoveListener(this.OnGDPRBtnClick);
            this.properties.btnBack.onClick.RemoveListener(this.OnBackBtnClick);
            Localization.OnLanguageChanged -= this.OnLanguageChanged;
            this.isActive = false;
        }

        #region CALLBACKS

        private void OnLanguageChanged() {
            this.UpdateLanguageTitle();
        }

        #endregion
    }
}