using System;
using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
    [Serializable]
    public sealed class UIPanelGemTreeLevelProperties : UIProperties {
        public Text textLevel;
        public ProgressBarMasked progressBar;
        public ProgressBarAnimations progressBarAnimations;
        public UIPanelGemTreeLevelAnimator animator;
        public Text textProgress;


        public void PlayLevelUpAnimation() {
            this.animator.PlayLevelUp();
        }
    }
}