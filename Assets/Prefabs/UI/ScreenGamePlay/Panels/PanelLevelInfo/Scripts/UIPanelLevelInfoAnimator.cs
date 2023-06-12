using UnityEngine;
using VavilichevGD.Audio;

namespace SinSity.UI {
    public class UIPanelLevelInfoAnimator : UIPanelTopScreenAnimator {

        [SerializeField] private AudioClip sfxNewLevel;
        
        private static readonly int triggerNaxtLevel = Animator.StringToHash("next_level");

        public delegate void AnimationHandler();
        public event AnimationHandler OnSwitchTextToNextLevelHandledEvent;
        public event AnimationHandler OnAnimationEndedEvent;
        
        public void PlayNextLevel() {
            this.SetTrigger(triggerNaxtLevel);
        }

        private void Handle_SwitchTextToNextLevel() {
            OnSwitchTextToNextLevelHandledEvent?.Invoke();
            PlaySFX();
        }

        private void Handle_AnimationEnded() {
            OnAnimationEndedEvent?.Invoke();
        }

        private void PlaySFX() {
            SFX.PlaySFX(sfxNewLevel);
        }
    }
}