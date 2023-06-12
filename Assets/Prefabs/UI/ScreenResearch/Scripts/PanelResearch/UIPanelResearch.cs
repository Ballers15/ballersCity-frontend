using SinSity.Core;
using Orego.Util;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public abstract class UIPanelResearch : UIElement, IUIPanelResearch
    {
        #region Event

        public AutoEvent<IUIPanelResearch> OnStartBtnClickEvent { get; }

        public AutoEvent<IUIPanelResearch> OnGetRewardBtnClickEvent { get; }

        #endregion

        public string researchId
        {
            get { return this.properties.researchId; }
        }

        protected abstract UIPanelResearchProperties properties { get; }

        public ResearchObject researchObjectCurrent { get; private set; }

        private UIWidgetPanelResearchTimer widgetTimer;
        protected Camera cameraMain;

        protected override void Start() {
            base.Start();

            cameraMain = Camera.main;
        }

        protected UIPanelResearch()
        {
            this.OnStartBtnClickEvent = new AutoEvent<IUIPanelResearch>();
            this.OnGetRewardBtnClickEvent = new AutoEvent<IUIPanelResearch>();
        }

        protected virtual void OnEnable() {
            Localization.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged() {
            UpdateCommonText();
        }

        protected virtual void OnDisable() {
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }

        private void UpdateCommonText() {
            if (this.researchObjectCurrent == null)
                return;
            
            var researchObjectInfo = this.researchObjectCurrent.info;
            this.properties.SetTitle(researchObjectInfo.GetTitle());
            this.properties.SetDescription(researchObjectInfo.GetDescription());
        }

        public virtual void Setup(ResearchObject researchObject)
        {
            this.researchObjectCurrent = researchObject;
            var researchObjectState = researchObject.state;
            if (researchObjectState.isEnabled)
            {
                this.SetStateTimerWork();
            }
            else if (researchObjectState.isRewardReady)
            {
                this.SetStateAwaitingReward();
            }
            else
            {
                this.SetStateCanStart();
            }

            UpdateCommonText();
        }

        public virtual void SetStateCanStart()
        {
            var widgetButton = this.properties.CreateWidgetButtonStart();
            widgetButton.AddListener(this.OnStartBtnClick);
        }

        public void SetStateTimerWork()
        {
            this.widgetTimer = this.properties.CreateWidgetTimer();
            this.UpdateTimerValue();
        }

        public void UpdateTimerValue()
        {
            var researchObjectState = this.researchObjectCurrent.state;
            int remainingTimeSec = researchObjectState.remainingTimeSec;
            string timerFormattedValue = GameTime.ConvertToFormatHMS(remainingTimeSec);
            this.widgetTimer.SetTimerValue(timerFormattedValue);
        }

        public void SetStateAwaitingReward()
        {
            var widgetButton = this.properties.CreateWidgetButtonGetReward();
            widgetButton.AddListener(this.OnGetRewardBtnClick);
        }

        #region ClickEvents

        protected virtual void OnStartBtnClick()
        {
            var widgetButton = this.properties.GetWidgetButton();
            if (widgetButton.AllowToUse()) {
                this.OnStartBtnClickEvent?.Invoke(this);
                SFX.PlayBtnClick();
            }
            else {
                widgetButton.PlaySFX_Error();                
            }
        }

        protected virtual void OnGetRewardBtnClick()
        {
            SFX.PlaySFX(this.properties.sfxGetReward);
            ResearchAnalytics.LogResearchRewardCollected(this.researchObjectCurrent);
            this.OnGetRewardBtnClickEvent?.Invoke(this);
        }

        #endregion
        
        
        protected void MakeFX(RewardInfoEcoClicker rewardInfoEcoClicker, Vector3 position) {

            if (rewardInfoEcoClicker is RewardInfoCase) {
                RewardInfoCase rewardInfoCase = rewardInfoEcoClicker as RewardInfoCase;
                MakeFXCases(rewardInfoCase, position);
            }
            else if (rewardInfoEcoClicker is RewardInfoHardCurrency){
                 RewardInfoHardCurrency rewardInfoHardCurrency = rewardInfoEcoClicker as RewardInfoHardCurrency;
                 MakeFXGems(rewardInfoHardCurrency, position);
            }
            else if (rewardInfoEcoClicker is RewardInfoTimeBooster) {
                RewardInfoTimeBooster rewardInfoTimeBooster = rewardInfoEcoClicker as RewardInfoTimeBooster;
                MakeFXTimeBooster(rewardInfoTimeBooster, position);
            }
        }

        protected void MakeFXCases(RewardInfoCase rewardInfoCase, Vector3 spawnPosition) {
            string caseId = rewardInfoCase.caseInfo.GetId();
            Product productCase = Shop.GetProduct(caseId);
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(spawnPosition);
            FXCasesGenerator.MakeFX(objectEcoClicker, productCase, rewardInfoCase.count);
        }

        protected void MakeFXGems(RewardInfoHardCurrency rewardInfoHardCurrency, Vector3 spawnPosition) {
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(spawnPosition);
            FXGemsGenerator.MakeFX(objectEcoClicker, rewardInfoHardCurrency.value);
        }

        protected void MakeFXTimeBooster(RewardInfoTimeBooster rewardInfoTimeBooster, Vector3 spawnPosition) {
            string timeBoosterId = rewardInfoTimeBooster.timeBoosterInfo.GetId();
            Product productTimeBooster = Shop.GetProduct(timeBoosterId);
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(spawnPosition);
            FXTimeBoosterGenerator.MakeFX(objectEcoClicker, productTimeBooster, rewardInfoTimeBooster.count);
        }
        

        private void OnDestroy()
        {
            this.OnStartBtnClickEvent.Dispose();
            this.OnGetRewardBtnClickEvent.Dispose();
        }
    }
}