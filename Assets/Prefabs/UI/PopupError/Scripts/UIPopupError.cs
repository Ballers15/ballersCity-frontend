using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupError : UIPopup<UIProperties, UIPopupArgs>
    {
        [SerializeField]
        private Button m_buttonOk;

        private void OnDestroy()
        {
            this.m_buttonOk.onClick.RemoveAllListeners();
        }

        private void OnEnable() {
            m_buttonOk.onClick.AddListener(OnOkBtnClick);
        }

        private void OnOkBtnClick() {
            Hide();
        }

        private void OnDisable() {
            m_buttonOk.onClick.RemoveListener(OnOkBtnClick);
        }
    }
}