using System.Collections.Generic;
using SinSity.Domain.Utils;
using SinSity.Services;
using SinSity.Core;
using SinSity.Domain;
using SinSity.UI.Analytics;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIOfflineIncomeController : UIWidget<UIOfflineIncomeControllerProperties>
    {
        #region Const

        private const int MULTIPLIER_X2 = 2;

        private const int MULTIPLIER_X3 = 3;

        #endregion

        private RewindTimeInteractor rewindTimeInteractor;

        private TutorialPipelineInteractor tutorialStagePipeline;
        
        private IdleObjectsInteractor idleObjectsInteractor;

        private UIInteractor uiInteractor;

        private DailyRewardInteractor dailyRewardInteractor;

        private Dictionary<IdleObject, BigNumber> collectedOfflineCurrency;

        private OfflineCollectedCurrencyArgs collectedCurrencyArgs;
        private PopupOfflineCollectedAnalytics analytics;
        private string result;

        #region Awake

        protected override void Awake()
        {
            base.Awake();
            this.analytics = new PopupOfflineCollectedAnalytics();
            this.result = PopupOfflineCollectedAnalytics.RESULT_CANCELED;
            var popupOfflineCollected = this.properties.popup;
            popupOfflineCollected.gameObject.SetActive(false);
            popupOfflineCollected.OnDoubleForAdClickEvent.AddListener(this.OnDoubleForAdBtnClick);
            popupOfflineCollected.OnTripleForGemsClickEvent.AddListener(this.OnTripleForGemsBtnClick);
            popupOfflineCollected.OnCloseClickEvent.AddListener(this.OnCloseBtnClick);
            popupOfflineCollected.OnGetSoftCurrencyClickEvent.AddListener(this.OnGetSoftCurrencyClick);
        }
        
        #endregion

        #region OnGameInitialized

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.idleObjectsInteractor = this.GetInteractor<IdleObjectsInteractor>();
            this.rewindTimeInteractor = this.GetInteractor<RewindTimeInteractor>();
            this.rewindTimeInteractor.OnRewindTimeFinishedEvent += this.OnRewindTimeFinished;
            this.tutorialStagePipeline = this.GetInteractor<TutorialPipelineInteractor>();
            this.dailyRewardInteractor = this.GetInteractor<DailyRewardInteractor>();
            this.uiInteractor = this.GetInteractor<UIInteractor>();
        }

        #endregion

        #region OnDestroy

        private void OnDestroy()
        {
            this.rewindTimeInteractor.OnRewindTimeFinishedEvent -= this.OnRewindTimeFinished;
        }

        #endregion

        #region RewindTimeEvents

        private void OnRewindTimeFinished(RewindTimeIntent rewindTimeIntent) {
            if (!this.tutorialStagePipeline.isTutorialCompleted)
                return;

            if (!(rewindTimeIntent is RewindTimeIntentFocusChanged) &&
                !(rewindTimeIntent is RewindTimeIntentGameStarted))
                return;

            var passedSeconds = rewindTimeIntent.passedSeconds;
            Debug.Log($"Rewind time intent: {rewindTimeIntent.GetType().Name}, seconds passed: {passedSeconds}");
            if (passedSeconds < this.properties.offlineSecondsTrashold)
                return;

            var offlineCollectedCurrency = this.idleObjectsInteractor.GetAllCollectedIdleObjectCurrency();
            this.collectedCurrencyArgs = new OfflineCollectedCurrencyArgs {
                offlineSeconds = passedSeconds,
                offlineCollectedCurrency = offlineCollectedCurrency,
                x3ForGemsPrice = this.properties.x3ForGemsPrice
            };
            this.ShowPopup();
        }

        private void ShowPopup()
        {
            var offlinePopup = this.properties.popup;
            offlinePopup.Show();
            offlinePopup.Setup(this.collectedCurrencyArgs);
        }

        #endregion

        #region ClickEvents

        private void OnDoubleForAdBtnClick() {
            UIPopupADLoading popup = uiInteractor.ShowElement<UIPopupADLoading>();
            popup.OnADResultsReceived += OnAdResultsReceived;
            popup.ShowAD("double_income");
        }

        private void OnAdResultsReceived(UIPopupADLoading popup, bool success, string error) {
            popup.OnADResultsReceived -= OnAdResultsReceived;
            
            if (success)
            {
                this.result = PopupOfflineCollectedAnalytics.RESULT_DOUBLED_FOR_AD;
                this.MultiplyCollectedOfflineCurrencyForEachIdleObject(MULTIPLIER_X2);
                ResetupOfflineCollectedCurrencyValue(MULTIPLIER_X2);
            }
            else
            {
                //TODO: Show Error:
                ResetupOfflineCollectedCurrencyValue();
            }

        }

        private void ResetupOfflineCollectedCurrencyValue(int mul = 1) {
            var offlinePopup = this.properties.popup;
            offlinePopup.ResetupAsIncomeMultiplyed(this.collectedCurrencyArgs.offlineCollectedCurrency, mul);
        }

        private void OnTripleForGemsBtnClick()
        {
           
            UIPopupOfflineCollected popup = this.properties.popup;
            var x3ForGemsPrice = this.properties.x3ForGemsPrice;
            if (Bank.isEnoughtHardCurrency(x3ForGemsPrice))
            {
                this.result = PopupOfflineCollectedAnalytics.RESULT_TRIPPLED_FOR_GEMS;
                Bank.SpendHardCurrency(x3ForGemsPrice, this);
                this.MultiplyCollectedOfflineCurrencyForEachIdleObject(MULTIPLIER_X3);
                ResetupOfflineCollectedCurrencyValue(MULTIPLIER_X3);
                popup.PlaySuccessSFX();
            }
            else {
                popup.PlayTrippleForGemsError();
            }
            
        }

        private void OnCloseBtnClick()
        {
            this.analytics.LogOfflineCollectedResults(this.result);
            this.ClosePopup();
        }

        private void OnGetSoftCurrencyClick()
        {
            this.analytics.LogOfflineCollectedResults(this.result);
            this.ClosePopup();
        }

        #endregion

        private void MultiplyCollectedOfflineCurrencyForEachIdleObject(int times) {
            var builtIdleObjects = this.idleObjectsInteractor.GetBuiltIdleObjects();
            foreach (var builtIdleObject in builtIdleObjects) {
                builtIdleObject.state.collectedCurrency *= times;
                builtIdleObject.CollectCurrencyInstantly();
            }
        }

        private void ClosePopup()
        {
            this.properties.popup.Hide();
            if (this.dailyRewardInteractor.CanReceiveReward())
            {
                this.uiInteractor.ShowElement<UIPopupDailyRewards>();
            }
        }
    }
}