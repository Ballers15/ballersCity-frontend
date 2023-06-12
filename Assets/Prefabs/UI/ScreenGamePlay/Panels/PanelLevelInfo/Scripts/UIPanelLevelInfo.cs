using System.Collections;
using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPanelLevelInfo : UIPanel<UIPanelLevelInfoProperties>
    {
        private ProfileLevelInteractor levelInteractor;

        private ProfileExperienceDataInteractor experienceDataInteractor;

        private LevelUp levelUp;
        private ProfileLevelTable levelTable;

        private IEnumerable<IExperienceVisualizer> experienceVisualizers;

        private int currentUILevel;

        private int currentUIExperience;

        private int requiredUIExperience;
        private bool isAnimStarted;

        private int restUIExperienceRange;


        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.levelInteractor = this.GetInteractor<ProfileLevelInteractor>();
            this.levelTable = this.levelInteractor.levelTable;
            this.experienceDataInteractor = this.GetInteractor<ProfileExperienceDataInteractor>();
            this.experienceVisualizers = this.GetInteractors<IExperienceVisualizer>();

            this.isAnimStarted = false;
            this.restUIExperienceRange = 0;
        }

        protected override void OnGameReady()
        {
            base.OnGameReady();
            foreach (var experienceVisualizer in this.experienceVisualizers)
            {
                experienceVisualizer.OnVisualizeAddedExperienceEvent += this.OnAddExperienceOnUI;
            }
        }

        protected override void OnGameStart()
        {
            base.OnGameStart();
            if (levelInteractor.isMaxLevelReached)
                this.SetVisualAsMax();
            else {
                this.UpdateLevelNumber(this.levelInteractor.currentLevel);
                this.SetupProgress(this.experienceDataInteractor.currentExperience);
            
                var profileLevelHandlers = this.GetInteractor<ProfileLevelInteractor>().GetLevelHandlers();
                foreach (var rewardReceiverInteractor in profileLevelHandlers)
                    rewardReceiverInteractor.OnLevelRewardReceivedEvent += OnLevelRewardReceivedEvent;
            }
        }

        public void SetupProgress(ulong newAbsExperience)
        {
            this.currentUILevel = this.levelInteractor.currentLevel;
            this.currentUIExperience = (int)this.levelTable.GetCurrentRelativeExperience(
                this.currentUILevel,
                newAbsExperience
            );
            this.requiredUIExperience = (int)this.levelTable.GetRequiredRelativeExperience(this.currentUILevel);
            this.UpdateProgress();
        }

        #region Update

        public void UpdateLevelNumber(int newLevel)
        {
            this.currentUILevel = newLevel;
            this.properties.SetLevelValue(newLevel);
        }

        private void UpdateProgress()
        {
            var progressTextValue = $"{this.currentUIExperience}/{this.requiredUIExperience}";
            var progressNormalized = (float) this.currentUIExperience / this.requiredUIExperience;
            this.properties.SetProgress(progressNormalized, progressTextValue);
        }

        #endregion

        #region InteractorEvents

        private void OnAddExperienceOnUI(object sender, Transform source, ulong range)
        {
            if (this.properties.visualSetuppedAsMax)
                return;
            
            WorldObjectEcoClicker objectEcoClicker = new WorldObjectEcoClicker(source.position);
            FXExperienceGenerator.MakeFX(objectEcoClicker, (int) range);
        }
        
        #endregion

        private void OnDestroy()
        {
            foreach (var experienceVisualizer in this.experienceVisualizers)
            {
                experienceVisualizer.OnVisualizeAddedExperienceEvent -= this.OnAddExperienceOnUI;
            }
            
            var profileLevelHandlers = this.GetInteractor<ProfileLevelInteractor>().GetLevelHandlers();
            foreach (var rewardReceiverInteractor in profileLevelHandlers)
                rewardReceiverInteractor.OnLevelRewardReceivedEvent -= OnLevelRewardReceivedEvent;
        }

        public void NotifyAboutExperienceAdded(ulong experienceRange)
        {
            if (this.isAnimStarted)
            {
                this.restUIExperienceRange += (int)experienceRange;
                return;
            }

            var range = (int)experienceRange;
            this.restUIExperienceRange = this.currentUIExperience + range - this.requiredUIExperience;

            if (this.restUIExperienceRange >= 0)
            {
                this.isAnimStarted = true;
                this.currentUIExperience = this.requiredUIExperience;
                this.UpdateProgress();
                this.StartAnimationNextLevel();
                return;
            }

            this.properties.PlayBounce();
            this.currentUIExperience += range;
            this.UpdateProgress();
        }

        

        private void StartAnimationNextLevel()
        {
            this.properties.animator.OnAnimationEndedEvent += OnAnimationEndedEvent;
            this.properties.animator.OnSwitchTextToNextLevelHandledEvent += OnSwitchTextToNextLevelHandledEvent;
            this.properties.PlayNextLevel();
        }

        private void OnAnimationEndedEvent() {
            this.properties.animator.OnAnimationEndedEvent -= OnAnimationEndedEvent;
            this.properties.animator.OnSwitchTextToNextLevelHandledEvent -= OnSwitchTextToNextLevelHandledEvent;

           
            this.isAnimStarted = false;

            
            if (this.levelInteractor.isMaxLevelReached)
                this.SetVisualAsMax();
            else
                this.StartCoroutine(FinishNextLevelAnimationRoutine());
        }
        
        private void SetVisualAsMax() {
            this.properties.SetVisualAsMax(levelInteractor.currentLevel);
        }

        private IEnumerator FinishNextLevelAnimationRoutine() {
            float duration = 0.75f;
            float timer = 0f;
            float startProgress = 0f;
            float targetProgress = (float) this.currentUIExperience / this.requiredUIExperience;
            while (timer < 1f) {
                timer = Mathf.Min(timer + Time.deltaTime / duration, 1f);
                float progressNormalized = Mathf.Lerp(startProgress, targetProgress, timer);
                string progressTextValue = $"{this.currentUIExperience}/{this.requiredUIExperience}";
                this.properties.SetProgress(progressNormalized, progressTextValue);
                yield return null;
            }
            this.UpdateLevelNumber(this.levelInteractor.currentLevel);
            this.SetupProgress(this.experienceDataInteractor.currentExperience);
        }
        
        private void OnSwitchTextToNextLevelHandledEvent() {
            this.UpdateLevelNumber(this.levelInteractor.currentLevel);

            var absExperience = experienceDataInteractor.currentExperience;
            this.currentUILevel = levelTable.GetCurrentLevel(absExperience);
            this.currentUIExperience = (int)levelTable.GetCurrentRelativeExperience(this.currentUILevel, absExperience);
            this.requiredUIExperience = (int) levelTable.GetRequiredRelativeExperience(this.currentUILevel);
            
            var progressTextValue = $"0/{this.requiredUIExperience}";
            var progressNormalized = 0f / this.requiredUIExperience;

            this.properties.SetProgress(progressNormalized, progressTextValue);
        }

        #region Events

        private void OnLevelRewardReceivedEvent(object sender, LevelUp _levelUp) {
            this.levelUp = _levelUp;
            
            UIInteractor uiInteractor = this.GetInteractor<UIInteractor>();
            var popup = uiInteractor.ShowElement<UIPopupNewLevel>();
            popup.Setup(this.levelUp);
        }

        #endregion
    }
}