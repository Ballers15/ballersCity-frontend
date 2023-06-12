using UnityEditor;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupExit : UIPopup<UIPopupExitProperties, UIPopupArgs> {
        
        private void OnEnable() {
            properties.btnYes.onClick.AddListener(OnYesBtnClick);
            properties.btnNo.onClick.AddListener(OnNoBtnClick);
        }

        private void OnYesBtnClick() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#endif
            Application.Quit();
            
            SFX.PlayBtnClick();
        }

        private void OnNoBtnClick() {
            Hide();
            
            SFX.PlayBtnClick();
        }

        private void OnDisable() {
            properties.btnYes.onClick.RemoveListener(OnYesBtnClick);
            properties.btnNo.onClick.RemoveListener(OnNoBtnClick);
        }
    }
}