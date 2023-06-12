using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using VavilichevGD.LocalizationFramework;

namespace SinSity.UI
{
    [Serializable]
    public class UIWidgetRewardCardProperties : UIProperties
    {
        [SerializeField] private Text textDayNumber;
        [SerializeField] private Text textCountValue; 
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBackground;
        [SerializeField] private Sprite spriteBackDefault;
        [SerializeField] private Sprite spriteBackToday;
        [SerializeField] private Image imgShadow;
        [SerializeField] private Image imgReceived;

        public void SetDayNumber(string dayNumberText) {
            string localizingString = Localization.GetTranslation("ID_DAY");
            string text = $"{localizingString} {dayNumberText}";
            this.textDayNumber.text = text;
        }

        public void SetCountValue(string countValueText)
        {
            this.textCountValue.text = countValueText;
        }

        public void SetIcon(Sprite spriteIcon)
        {
            this.imgIcon.sprite = spriteIcon;
        }

        public void SetupCard(bool today, bool received)
        {
            this.imgShadow.gameObject.SetActive(received);
            this.imgReceived.gameObject.SetActive(received);
            var sprite = today
                ? this.spriteBackToday
                : this.spriteBackDefault;
            this.imgBackground.sprite = sprite;
        }
    }
}