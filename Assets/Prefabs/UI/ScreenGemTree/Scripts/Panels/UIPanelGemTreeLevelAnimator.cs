using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIPanelGemTreeLevelAnimator : AnimObject {
        private static readonly int triggerLevelUp = Animator.StringToHash("level_up");

        public void PlayLevelUp() {
            this.SetTrigger(triggerLevelUp);
        }
        
    }
}