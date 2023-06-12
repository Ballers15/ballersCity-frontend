using System.Collections;
using Orego.Util;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPanelGemTreeLevel : UIWidget<UIPanelGemTreeLevelProperties> {
        [SerializeField] private AudioClip sfxLevelUp;
        [SerializeField] private AudioClip sfxUpgrade;
        
        private GemTreeStateInteractor gemTreeStateInteractor;

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.gemTreeStateInteractor = this.GetInteractor<GemTreeStateInteractor>();
        }

        public void OnShow()
        {
            this.UpdateState();
        }

        private void UpdateState()
        {
            string levelTranslated = Localization.GetTranslation("ID_LEVEL");

            if (this.gemTreeStateInteractor.IsLastLevel())
            {
                this.properties.textLevel.text = $"{levelTranslated} MAX";
                this.properties.progressBar.SetValue(1.0f);
                this.properties.textProgress.SetInvisible();
            }
            else
            {
                var currentLevel = this.gemTreeStateInteractor.currentLevelIndex + 1;
                var requiredLevel = this.gemTreeStateInteractor.GetMaxLevel() + 1;
                this.properties.textLevel.text = $"{levelTranslated} {currentLevel}/{requiredLevel}";
                this.properties.textProgress.SetVisible();
                this.UpdateProgress();
            }
        }

        private void UpdateProgress()
        {
            var currentProgress = this.gemTreeStateInteractor.currentProgress;
            var requiredProgress = this.gemTreeStateInteractor.GetMaxProgress();
            this.properties.progressBar.SetValue((float) currentProgress / requiredProgress);
            this.properties.textProgress.text = $"{currentProgress}/{requiredProgress}";
        }
        
        public void OnHide()
        {
        }

        public void AnimateProgressUp(int newProgress) {
            SFX.PlaySFX(this.sfxUpgrade);
            this.UpdateProgress();
        }

        public IEnumerator AnimateLevelUp(int newLevel) {
            SFX.PlaySFX(this.sfxLevelUp);
            this.properties.PlayLevelUpAnimation();
            this.UpdateState();
            if (!this.gemTreeStateInteractor.IsLastLevel()) {
                this.properties.progressBar.SetValue(1f);
                yield return new WaitForSeconds(0.5f);
                this.properties.progressBarAnimations.PlayProgress(1f, 0f);
                this.properties.progressBarAnimations.OnAnimationOver += OnAnimationOver;                
            }
            yield break;
        }

        private void OnAnimationOver(ProgressBarAnimations progressbaranimations) {
            this.properties.progressBarAnimations.OnAnimationOver -= OnAnimationOver;
            this.UpdateState();
        }
    }
}