using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public class FXExperience : FXUIFlyingReward<ulong> {
        [SerializeField] private AudioClip sfxAppear;
        
        private UIPanelLevelInfo panelLevelInfo;

        protected void Start() {
            this.panelLevelInfo = FindObjectOfType<UIPanelLevelInfo>();
        }

        protected override void ApplyReward() {
            this.panelLevelInfo.NotifyAboutExperienceAdded(this.rewardCurrent);
        }

        protected override void OnEnable() {
            base.OnEnable();
            PlaySFX();
        }

        private void PlaySFX() {
            if (Game.isInitialized)
                SFX.PlaySFXRandomPitch(sfxAppear);
        }
    }
}