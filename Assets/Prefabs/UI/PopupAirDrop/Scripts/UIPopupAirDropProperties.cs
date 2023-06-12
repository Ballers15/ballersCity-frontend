using System;
using SinSity.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.Tools;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupAirDropProperties : UIProperties {

        public Button btnGet;
        public Button btnClose;
        
        [Space]
        [SerializeField] private Text textValue;
        [SerializeField] private Image imgValueIcon;
        
        [Space] 
        [SerializeField] public Image imgAirShip;
        [SerializeField] public Sprite iconRegularAirShip;
        [SerializeField] public Sprite iconAdAirShip;

        [Space] 
        [SerializeField] private Sprite iconCleanEnergy;
        [SerializeField] private Sprite iconGems;

        [Space] 
        [SerializeField] private AudioClip sfxClick;
        [SerializeField] private AudioClip sfxClose;
        
        public void SetPackageValue(int gemsValue) {
            textValue.text = gemsValue.ToString();
            imgValueIcon.sprite = iconGems;
            RecalculateValueField();
        }

        private void RecalculateValueField() {
            RectTransform parent = textValue.transform.parent as RectTransform;
            parent.RecalculateWithHorizontalFitterInside(ContentSizeFitter.FitMode.PreferredSize);
        }

        public void SetPackageValue(BigNumber cleanEnergyValue) {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            textValue.text = cleanEnergyValue.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            imgValueIcon.sprite = iconCleanEnergy;
            RecalculateValueField();
        }

        public void PlayClick() {
            SFX.PlaySFX(sfxClick);
        }

        public void PlayClose() {
            SFX.PlaySFX(sfxClose);
        }
    }
}