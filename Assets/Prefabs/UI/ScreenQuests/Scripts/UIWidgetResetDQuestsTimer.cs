using System;
using SinSity.Core;
using UnityEngine.UI;
using UnityEngine;
using VavilichevGD.UI;
using SinSity.Scripts;
using SinSity.Domain;
using SinSity.Tools;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;
using VavilichevGD.Architecture;

namespace SinSity.UI
{
    public class UIWidgetResetDQuestsTimer : UIWidget<UIWidgetResetDQuestsTimer.Properties>
    {
        private MiniQuestInteractor interactor;
        private EcoClickerTimer timerResetQuestsNotAvailable;

        protected override void Awake()
        {
            base.Awake();
            this.interactor = this.GetInteractor<MiniQuestInteractor>();
            this.timerResetQuestsNotAvailable = new EcoClickerTimer();
        }

        private void OnEnable()
        {
            this.RestartTimer();
            this.UpdateState();
            this.interactor.OnResetTimeChangedEvent += onResetTimeChanged;
        }

        private void OnDisable()
        {
            this.StopTimer();
        }

        private void UpdateState()
        {
            if (!this.interactor.isInitialized)
                return;

            var resetAvailable = this.interactor.CanUseReset();

            if (!resetAvailable && !timerResetQuestsNotAvailable.isActive)
                this.ActivateTimer();
            if (resetAvailable)
            {
                var miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
                if (miniQuestInteractor.TryDisableQuests())
                {
                    miniQuestInteractor.UpdateLastResetTime(this);
                    miniQuestInteractor.TryInflateFreshDailyQuests();
                    this.ActivateTimer();
                }   
            }
        }

        public void ActivateTimer()
        {
            if (this.timerResetQuestsNotAvailable.isActive)
                return;

            var totalSeconds = this.interactor.GetTimeToNextReset();
            this.timerResetQuestsNotAvailable.timerValue = totalSeconds;
            this.timerResetQuestsNotAvailable.OnTimerValueChangedEvent += this.OnTimerValueChanged;
            this.timerResetQuestsNotAvailable.OnTimerCompletedEvent += this.OnTimerCompleted;
            this.UpdateTimerText();
            this.timerResetQuestsNotAvailable.Start();
        }

        private void StopTimer()
        {
            if (!this.timerResetQuestsNotAvailable.isActive) return;
            this.timerResetQuestsNotAvailable.OnTimerValueChangedEvent -= this.OnTimerValueChanged;
            this.timerResetQuestsNotAvailable.OnTimerCompletedEvent -= this.OnTimerCompleted;
            this.timerResetQuestsNotAvailable.Stop();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                StopTimer();
                UpdateState();
            }
        }

        private void RestartTimer()
        {
            this.StopTimer();
            this.ActivateTimer();
        }
        private void OnTimerCompleted()
        {
            this.timerResetQuestsNotAvailable.OnTimerValueChangedEvent -= this.OnTimerValueChanged;
            this.timerResetQuestsNotAvailable.OnTimerCompletedEvent -= this.OnTimerCompleted;
            this.UpdateState();
        }

        private void OnTimerValueChanged()
        {
            this.UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            var totalSeconds = this.timerResetQuestsNotAvailable.timerValue;
            string localizedString = Localization.GetTranslation("ID_RESET_QUESTS");
            var time = GameTime.ConvertToFormatHMS(totalSeconds);
            this.properties.textResetTimer.text = string.Format(localizedString, time);
        }

        private void onResetTimeChanged(object sender)
        {
            this.RestartTimer();
        }

        [Serializable]
        public class Properties : UIProperties
        {
            public Text textResetTimer;
        }
    }
}
