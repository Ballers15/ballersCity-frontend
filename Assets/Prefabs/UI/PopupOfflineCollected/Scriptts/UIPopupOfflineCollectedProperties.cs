using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupOfflineCollectedProperties : UIProperties {
        public Text textTimeValue;
        public UIPanelTextWithFitter panelCollectedCurrencyValue;
        public UIPanelPrice panelPriceTripleCurrency;
        [Space] 
        public Button btnDoubleForAd;
        public Button btnTripleForGems;
        public Button btnGet;
        public Animator animatorBtnTripple;
        public Button btnClose;
        [Space] 
        public Transform transformContainerIncreaseIncome;
        public AudioClip audioClipMultiplySoftCurrencyClick;
        public AudioClip audioClipCloseClick;
        public AudioClip audioClipGetClick;
        public AudioClip sfxError;
        
        private static readonly int riggerButtonError = Animator.StringToHash("error");

        public void PlayError_BtnTripleForGems() {
            animatorBtnTripple.SetTrigger(riggerButtonError);
            SFX.PlaySFX(sfxError);
        }
    }
}