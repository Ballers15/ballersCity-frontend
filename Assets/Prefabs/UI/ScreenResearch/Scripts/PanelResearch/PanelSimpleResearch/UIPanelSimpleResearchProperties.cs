using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace SinSity.UI
{
    [Serializable]
    public class UIPanelSimpleResearchProperties : UIPanelResearchProperties
    {
        [SerializeField]
        private Image imgIcon;

        [SerializeField]
        private TextMeshProUGUI textCount;

        public void SetIcon(Sprite iconSprite)
        {
            this.imgIcon.sprite = iconSprite;
        }

        public void SetCountText(string countText)
        {
            this.textCount.text = countText;
        }
    }
}