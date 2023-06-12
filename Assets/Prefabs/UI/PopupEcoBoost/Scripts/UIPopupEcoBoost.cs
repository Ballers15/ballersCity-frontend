using System;
using SinSity.Services;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupEcoBoost : UIPopupAnim<UIPopupEcoBoostProperties, UIPopupArgs>
    {
        #region Const

        private const int SECONDS_IN_HOUR = 3600;

        private const int SECONDS_IN_MINTES = 60;

        #endregion

        private EcoBoostInteractor ecoBoostInteractor;

        private UIInteractor uiInteractor;

        #region Awake

        protected override void Awake()
        {
            base.Awake();
            this.BindEcoBoostInteractor();
            InitUITimeElements();
        }

        private void BindEcoBoostInteractor()
        {
            this.uiInteractor = this.GetInteractor<UIInteractor>();
            this.ecoBoostInteractor = this.GetInteractor<EcoBoostInteractor>();
            this.ecoBoostInteractor.OnEcoBoostRemainingSecondsChangedEvent += this.OnRemainingSecondsChanged;
            this.ecoBoostInteractor.OnEcoBoostDisabledEvent += this.OnEcoBoostDisabled;
        }

        private void InitUITimeElements()
        {
            long remainingSeconds;
            if (this.ecoBoostInteractor.isEcoBoostWorking)
            {
                remainingSeconds = (long) this.ecoBoostInteractor.remainingTimeSeconds;
            }
            else
            {
                remainingSeconds = 0;
            }

            this.RefreshProgressBar(remainingSeconds);
            this.RefreshTimer(remainingSeconds);
        }

        #endregion

        private void OnEnable()
        {
            this.properties.buttonStartEcoBoost.onClick.AddListener(this.OnStartEcoBoostClick);
            this.properties.btnClose.onClick.AddListener(OnCloseBtnClick);
        }

        private void OnDisable()
        {
            this.properties.buttonStartEcoBoost.onClick.RemoveAllListeners();
            this.properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
        }

        private void OnDestroy()
        {
            this.ecoBoostInteractor.OnEcoBoostRemainingSecondsChangedEvent -= this.OnRemainingSecondsChanged;
            this.ecoBoostInteractor.OnEcoBoostDisabledEvent -= this.OnEcoBoostDisabled;
        }

        #region Refresh

        private void RefreshProgressBar(long remainingSeconds)
        {
            var ecoBoostConfig = this.ecoBoostInteractor.config;
            var limitSeconds = (float) ecoBoostConfig.limitDurationTime;
            var restSeconds = Math.Min(remainingSeconds, limitSeconds);
            var percent = restSeconds / limitSeconds;
            this.properties.progressBarMasked.SetValue(percent);
        }

        private void RefreshTimer(long remainingSeconds)
        {
            var hours = remainingSeconds / SECONDS_IN_HOUR;
            var minutes = (remainingSeconds - hours * SECONDS_IN_HOUR) / SECONDS_IN_MINTES;
            var localizationString = Localization.GetTranslation("ID_TIME");
            this.properties.textTimer.text = string.Format(localizationString, hours, minutes);
        }

        #endregion

        #region Events

        private void OnRemainingSecondsChanged(long remainingSeconds)
        {
            this.RefreshProgressBar(remainingSeconds);
            this.RefreshTimer(remainingSeconds);
        }

        private void OnEcoBoostDisabled()
        {
            this.RefreshProgressBar(0);
            this.RefreshTimer(0);
        }

        private void OnCloseBtnClick()
        {
            SFX.PlayClosePopup();
            Hide();
        }

        #endregion

        #region Click

        private void OnStartEcoBoostClick()
        {
            SFX.PlaySFX(this.properties.audioClipEcoBoostClick);
            var popupAdLoading = this.uiInteractor.ShowElement<UIPopupADLoading>();
            popupAdLoading.OnADResultsReceived += OnAdResultsReceived;
            popupAdLoading.ShowAD("ecoboost");
        }

        private void OnAdResultsReceived(UIPopupADLoading popup, bool success, string error)
        {
            popup.OnDialogueResults -= this.OnAdResultsReceived;
            if (success)
            {
                this.StartEcoBoost();
            }
        }

        private void OnAdResultsReceived(UIPopupArgs uiPopupArgs)
        {
        }

        private void StartEcoBoost()
        {
            this.ecoBoostInteractor.LaunchEcoBoost();
        }

        #endregion
    }
}