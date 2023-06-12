using System;
using System.Collections;
using Orego.Util;
using SinSity.Domain;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIScreenGemTree : UIScreen<UIScreenGemTreeProperties>
    {
        #region Event

        public AutoEvent OnAnimationStartEvent;

        public AutoEvent OnAnimationFinishedEvent;

        #endregion

        private GemTreeStateInteractor gemTreeStateInteractor;

        private readonly Routine animateRoutine;

        public UIScreenGemTree()
        {
            this.OnAnimationStartEvent = new AutoEvent();
            this.OnAnimationFinishedEvent = new AutoEvent();
            this.animateRoutine = new Routine(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.HideInstantly();
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.gemTreeStateInteractor = this.GetInteractor<GemTreeStateInteractor>();
        }

        public override void Show()
        {
            base.Show();
            this.gemTreeStateInteractor.OnCurrentLevelChangedEvent += this.OnGemTreeLevelChanged;
            this.gemTreeStateInteractor.OnCurrentProgressChangedEvent += this.OnGemTreeProgressChanged;
            this.properties.gemTree.OnShow();
            this.properties.panelGemTreeLevel.OnShow();
            this.properties.buttonGemTreeUpgrade.OnShow();
        }

        public override void Hide()
        {
            base.Hide();
            this.gemTreeStateInteractor.OnCurrentLevelChangedEvent -= this.OnGemTreeLevelChanged;
            this.gemTreeStateInteractor.OnCurrentProgressChangedEvent -= this.OnGemTreeProgressChanged;
            this.properties.gemTree.OnHide();
            this.properties.panelGemTreeLevel.OnHide();
            this.properties.buttonGemTreeUpgrade.OnHide();
        }

        #region InteractorEvents

        private void OnGemTreeProgressChanged(object sender, int newProgress) {
            this.OnAnimationStartEvent?.Invoke();
            this.ProgressUp(newProgress);
        }

        private void ProgressUp(int nextProgress) {
            this.properties.buttonGemTreeUpgrade.AnimateProgressUp(nextProgress);
            this.properties.panelGemTreeLevel.AnimateProgressUp(nextProgress);
            this.OnAnimationFinishedEvent?.Invoke();
        }

        private void OnGemTreeLevelChanged(object sender, int levelIndex)
        {
            this.OnAnimationStartEvent?.Invoke();
            int levelNumber = levelIndex + 1;
            this.animateRoutine.Start(this.AnimateLevelUpRoutine(levelNumber));
        }

        private IEnumerator AnimateLevelUpRoutine(int levelNumber)
        {
            this.properties.buttonGemTreeUpgrade.AnimateLevelUp(levelNumber);
            yield return this.properties.panelGemTreeLevel.AnimateLevelUp(levelNumber);
            yield return this.properties.gemTree.AnimateLevelUp(levelNumber);
            this.OnAnimationFinishedEvent?.Invoke();
        }

        #endregion

        private void OnDestroy()
        {
            this.OnAnimationStartEvent.Dispose();
            this.OnAnimationFinishedEvent.Dispose();
        }

        private void OnDisable() {
            this.animateRoutine.Cancel();
        }
    }
}