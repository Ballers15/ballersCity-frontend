using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupPersonageCardProperties : UIProperties {
        [SerializeField] private Image imgIcon;
        [SerializeField] private ProgressBarMaskedWithTMPro progressBar;
        [SerializeField] private Text textObjectTitleValue;
        [SerializeField] private Text textSpeedValue;
        [SerializeField] private Text textIncomeValue;
        [SerializeField] private UIWidgetPrice panelPrice;
        [Space] 
        [SerializeField] private Button m_btnUpgrade;
        [SerializeField] private Button m_btnClose;
        [Space] 
        [SerializeField] private UIPopupPersonageCardAnimator m_animator;
        [Space] 
        [SerializeField] private AudioClip m_sfxNotEnoughGems;
        [SerializeField] private AudioClip m_sfxCardUpgradedSuccessfully;

        public Button btnUpgrade => m_btnUpgrade;
        public Button btnClose => m_btnClose;
        public UIPopupPersonageCardAnimator animator => m_animator;
        public AudioClip sfxNotEnoughGems => m_sfxNotEnoughGems;
        public AudioClip sfxCardUpgradedSuccessfully => m_sfxCardUpgradedSuccessfully;

        public void SetIcon(Sprite spriteIcon) {
            this.imgIcon.sprite = spriteIcon;
        }

        public void SetProgress(float valueNormalized, string valueText) {
            this.progressBar.SetValue(valueNormalized);
            this.progressBar.SetTextValue(valueText);
        }

        public void SetObjectTitle(string objectTitle) {
            textObjectTitleValue.text = objectTitle;
        }

        public void SetSpeedValue(string speedValue) {
            textSpeedValue.text = speedValue;
        }

        public void SetIncomeValue(string incomeValue) {
            textIncomeValue.text = incomeValue;
        }

        public void SetPrice(int priceGems, bool cardCountEnough) {
            this.panelPrice.SetPriceHard(priceGems);
            if (!cardCountEnough)
                this.panelPrice.SetVisualAsNotEnoughCurrency();
        }
    }
}