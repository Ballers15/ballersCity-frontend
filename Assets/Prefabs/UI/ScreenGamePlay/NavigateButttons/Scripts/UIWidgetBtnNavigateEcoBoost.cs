using System;
using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.UI
{
    public sealed class UIWidgetBtnNavigateEcoBoost : UIWidgetBtnNavigate
    {
        #region Const

        private const int SECONDS_IN_HOUR = 3600;

        private const int SECONDS_IN_MINTES = 60;

        #endregion

        #region Event

        public static event Action OnPopupOpenedEvent;

        public static event Action OnPopupClosedEvent;

        #endregion

        [SerializeField] private TextMeshProUGUI textTimer;
        [SerializeField] private ProgressBarMasked progressBarTimer;

        [Space]
        [SerializeField] private Image imgBackground;
        [SerializeField] private GameObject goExclamationMark;
        [SerializeField] private Sprite spriteBackgroundActive;
        [SerializeField] private Sprite spriteBackgroundInactive;

        private EcoBoostInteractor ecoBoostInteractor;

        private RewindTimeInteractor rewindTimeInteractor;

        protected static readonly int boolEcoboostEnabledId = Animator.StringToHash("ecoboost_enabled");
        protected static readonly int triggerEcoboostEnabledStartedId = Animator.StringToHash("ecoboost_started");
        protected static readonly int triggerEcoboostEnabledEndedId = Animator.StringToHash("ecoboost_ended");


        protected override void Awake()
        {
            base.Awake();
            Game.OnGameStart += this.OnGameStart;
            this.SetInvisible();
        }

        private void OnGameStart(Game game)
        {
            this.rewindTimeInteractor = Game.GetInteractor<RewindTimeInteractor>();
            this.rewindTimeInteractor.OnRewindTimeFinishedEvent += this.OnRewindTimeFinished;
            this.ecoBoostInteractor = Game.GetInteractor<EcoBoostInteractor>();
            this.ecoBoostInteractor.OnEcoBoostLaunchedEvent += this.OnEcoBoostLaunched;
            this.ecoBoostInteractor.OnEcoBoostEnabledEvent += this.OnEcoBoostEnabled;
            this.ecoBoostInteractor.OnEcoBoostDisabledEvent += this.OnEcoBoostDisabled;
            this.ecoBoostInteractor.OnEcoBoostRemainingSecondsChangedEvent += this.OnEcoBoostRemainingSecondsChanged;
            if (this.ecoBoostInteractor.isEcoboostUnlocked)
            {
                this.SetVisible();
            }
            else
            {
                this.ecoBoostInteractor.OnEcoBoostUnlockedEvent += this.OnEcoBoostUnlocked;
            }
        }

        private void OnEcoBoostUnlocked(object obj)
        {
            this.ecoBoostInteractor.OnEcoBoostUnlockedEvent -= this.OnEcoBoostUnlocked;
            this.SetVisible();
        }

        private void OnRewindTimeFinished(RewindTimeIntent obj)
        {
            if (this.ecoBoostInteractor.isEcoBoostWorking)
            {
                this.ShowTimer();
            }
            else
            {
                this.HideTimer();
            }
        }

        public override bool IsPopupAlreadyOpened()
        {
            return uiInteractor.uiController.IsActiveUIElement<UIPopupEcoBoost>();
        }

        protected override void OpenPopup()
        {
            this.goExclamationMark.SetActive(false);
            this.uiInteractor.ShowElement<UIPopupEcoBoost>();
            CommonAnalytics.LogXtraManagementTabOpened();
            OnPopupOpenedEvent?.Invoke();
        }

        public override void ClosePopup()
        {
        }

        private void ShowTimer()
        {
            this.SetViewAsActive();
            this.animator.SetBool(boolEcoboostEnabledId, true);
            this.textTimer.gameObject.SetActive(true);
            var remainingSeconds = this.ecoBoostInteractor.remainingTimeSeconds;
            this.OnEcoBoostRemainingSecondsChanged((long) remainingSeconds);
        }

        private void HideTimer()
        {
            this.SetViewAsInactive();
            this.animator.SetBool(boolEcoboostEnabledId, false);
            this.textTimer.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            this.ecoBoostInteractor.OnEcoBoostLaunchedEvent -= this.OnEcoBoostLaunched;
            this.ecoBoostInteractor.OnEcoBoostDisabledEvent -= this.OnEcoBoostDisabled;
            this.ecoBoostInteractor.OnEcoBoostRemainingSecondsChangedEvent -= this.OnEcoBoostRemainingSecondsChanged;
            this.ecoBoostInteractor.OnEcoBoostEnabledEvent -= this.OnEcoBoostEnabled;
            this.rewindTimeInteractor.OnRewindTimeFinishedEvent -= this.OnRewindTimeFinished;
        }

        #region EcoBoostEvents

        private void OnEcoBoostRemainingSecondsChanged(long remainingSeconds)
        {
            var ecoBoostConfig = this.ecoBoostInteractor.config;
            var limitSeconds = (float) ecoBoostConfig.limitDurationTime;
            var restSeconds = Math.Min(remainingSeconds, limitSeconds);
            var percent = restSeconds / limitSeconds;
            this.progressBarTimer.SetValue(percent);

            string timeText = GameTime.ConvertToFormatHM((int) remainingSeconds);
            this.textTimer.text = timeText;
        }

        private void OnEcoBoostLaunched()
        {
            this.ShowTimer();
        }

        private void OnEcoBoostEnabled()
        {
            if (!animator.GetBool(boolEcoboostEnabledId))
                this.animator.SetTrigger(triggerEcoboostEnabledStartedId);
        }

        private void OnEcoBoostDisabled()
        {
            this.HideTimer();
            this.animator.SetTrigger(triggerEcoboostEnabledEndedId);
        }

        #endregion


        private void SetViewAsActive()
        {
            goExclamationMark.SetActive(false);
            imgBackground.sprite = spriteBackgroundActive;
        }

        private void SetViewAsInactive()
        {
            if (IsViewAlreadyInactive())
                return;

            goExclamationMark.SetActive(true);
            imgBackground.sprite = spriteBackgroundInactive;
        }

        private bool IsViewAlreadyInactive() {
            return imgBackground.sprite == spriteBackgroundInactive;
        }
    }
}