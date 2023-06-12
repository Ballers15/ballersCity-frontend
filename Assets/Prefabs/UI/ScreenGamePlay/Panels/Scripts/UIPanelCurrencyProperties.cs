using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPanelCurrencyProperties : UIProperties {
        public Text textTotalCollected;
        public Button btn;
        [SerializeField] private UIPanelTopScreenAnimator animator;
        
        public void PlayBounce() {
            animator.PlayBounce();
        }
    }
}