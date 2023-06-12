using System;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIPopupModernizationInfo : UIPopupAnim<UIPopupModernizationInfoProperties, UIPopupArgs>
    {
        private string caption;
        private string descr;
        
        public void SetPopupType(UIPopupModernizationInfoProperties.PopupType type)
        {
            foreach (var popType in properties.popupContent)
            {
                if (popType.type != type) continue;
                SetTextFields(popType.content);
                break;
            }
        }

        private void SetTextFields(UIPopupModernizationInfoProperties.Content content)
        {
           properties.textFieldCaption.text = Localization.GetTranslation(content.captionTranslationKey);
           properties.textFieldDescription.text = Localization.GetTranslation(content.descrTranslationKey);
        }
        
        private void OnEnable() 
        {
            foreach (var btn in properties.buttonsClose)
            {
                btn.onClick.AddListener(OnCloseBtnClick);
            }
        }
        
        private void OnCloseBtnClick() 
        {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            Hide();
        }
        
        private void OnDisable() 
        {
            foreach (var btn in properties.buttonsClose)
            {
                btn.onClick.RemoveListener(OnCloseBtnClick);
            }
        }
    }
}