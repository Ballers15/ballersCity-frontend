using System;
using SinSity.Meta;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    public class IdleObject : MonoBehaviour, IBankStateWithoutNotification, IIdleObject {

        #region CONSTANTS

        private static readonly object dummyObject = null;

        #endregion

        private const int MAX_LEVEL = 1000;
        
        [SerializeField]
        protected IdleObjectInfo m_info;

        [SerializeField]
        private Sounds m_sounds;

        #region Events

        public delegate void IdleObjectStateChangeHandler(IdleObjectState state);

        public event IdleObjectStateChangeHandler OnStateChangedEvent;

        public delegate void IdleObjectProgressChangeHandler(float progressNormalized);

        public event IdleObjectProgressChangeHandler OnProgressChanged;

        public delegate void IdleObjectBuildHandler(IdleObject idleObject, IdleObjectState state);

        public static event IdleObjectBuildHandler OnIdleObjectBuilt;
        public event IdleObjectBuildHandler OnBuiltEvent;


        public delegate void IdleObjectCollectCurrencyHandler(object sender, BigNumber collectedCurrency);

        public static event IdleObjectCollectCurrencyHandler OnIdleObjectCurrencyCollected;
        public event IdleObjectCollectCurrencyHandler OnCurrencyCollected;

        public delegate void IdleObjectInitializeHandler();

        public event IdleObjectInitializeHandler OnInitialized;

        public delegate void IdleObjectWorkHandler();

        public event IdleObjectWorkHandler OnWorkStarted;
        public event IdleObjectWorkHandler OnWorkOver;

        public delegate void LevelRisedHandler(int newLevel, bool success);

        public event LevelRisedHandler OnLevelRisenEvent;
        public static event Action<IdleObject, int, bool> OnIdleObjectLevelRisen;

        public delegate void ImprovementRewardHandler(IdleObject idleObject, LevelImprovementReward reward);

        public static event ImprovementRewardHandler OnIdleObjectLevelStageRisen;
        public event ImprovementRewardHandler OnImprovementRewarded;

        public event Action OnResetEvent;

        public static event Action<IdleObject> OnIdleObjectResetEvent;

        #endregion


        public string id => m_info.id;
        public bool isBuilt => state.isBuilded;
        public bool isBuildedAnyTime => state.isBuildedAnyTime;
        public LevelImprovementBlock levelImprovementBlock { get; protected set; }
        public IdleObjectState state { get; protected set; }
        public IdleObjectInfo info => m_info;
        public bool isInitialized => state != null;
        public bool autoplayEnabled => state.autoPlayEnabled;
        public BigNumber incomePerSec => incomeCurrent / state.incomePeriod;

        public bool isWorking { get; private set; }
        public float timerValue = 0f;

        public BigNumber incomeCurrent => state.fullIncome;

        private IOPeriodAllCoefficients ioPeriodAllCoefficients;


        private void Awake() {
            Game.OnGameInitialized += this.OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= this.OnGameInitialized;
            
            this.ioPeriodAllCoefficients = new IOPeriodAllCoefficients();
        }


        public void Initialize(string stateJson = null)
        {
            state = info.CreateState(stateJson);
            UpdateBuildAnyTimeState(state);
            UpdateLevelImprovementBlock(state.level);
            OnInitialized?.Invoke();
        }

        private void UpdateBuildAnyTimeState(IdleObjectState state) {
            if (state.isBuilded)
                state.isBuildedAnyTime = true;
        }

        private void UpdateLevelImprovementBlock(int newLevel)
        {
            var pipeline = LevelImprovementsPipeline.Load();
            this.levelImprovementBlock = pipeline.GetBlockByLevel(newLevel);
            Resources.UnloadUnusedAssets();
        }


        public void ForceUpdate(float unscaledDeltaTime)
        {
            if (!state.isBuilded)
                return;

            if (this.state.autoPlayEnabled) {
                this.TimerWork(unscaledDeltaTime);
                return;
            }

            if (Math.Abs(this.state.progressNomalized - 1f) < Mathf.Epsilon) {
                if (!this.isWorking)
                    return;
                
                this.StopWorking();
                return;
            }
            
            this.TimerWork(unscaledDeltaTime);
            
            
            if (!autoplayEnabled && state.hasAnyCollectedCurrency) {
                if (!isWorking)
                    return;
                
                this.StopWorking();
                return;
            }

            if (!isWorking)
            {
                isWorking = true;
                OnWorkStarted?.Invoke();
            }

            TimerWork(unscaledDeltaTime);
        }

        private void TimerWork(float unscaledDeltaTime)
        {
            var ioPeriodCoefficientTotal = this.ioPeriodAllCoefficients.GetTotalValue();
            timerValue += unscaledDeltaTime * ioPeriodCoefficientTotal;
            
            state.progressNomalized = Mathf.Clamp01(timerValue / this.state.incomePeriod);
            
            OnProgressChanged?.Invoke(state.progressNomalized);
            if (timerValue >= this.state.incomePeriod)
                this.StopWorking();
        }

        private void StopWorking() {
            this.IncreaseCollectedCurrency();
            this.timerValue = 0f;
            this.isWorking = false;
            this.state.progressNomalized = 1f;
            this.OnProgressChanged?.Invoke(this.state.progressNomalized);
            this.OnWorkOver?.Invoke();
        }

        private void IncreaseCollectedCurrency()
        {
            state.collectedCurrency += incomeCurrent;
            NotifyAboutStateChanged();
        }


        public void NotifyAboutStateChanged()
        {
            OnStateChangedEvent?.Invoke(state);
        }

        private void NotifyAboutBuilt()
        {
            OnBuiltEvent?.Invoke(this, state);
            OnIdleObjectBuilt?.Invoke(this, state);
            CommonAnalytics.LogObjectBuilt(this.id);
        }

        private void NotifyAboutCurrencyCollected(object sender, BigNumber value)
        {
            OnIdleObjectCurrencyCollected?.Invoke(sender, value);
            OnCurrencyCollected?.Invoke(sender, value);
        }

        private void NotifyAboutNextLevel(bool success)
        {
            var level = this.state.level;
            this.OnLevelRisenEvent?.Invoke(level, success);
            OnIdleObjectLevelRisen?.Invoke(this, level, success);
        }

        private void NotifyAboutImprovementRewarded(LevelImprovementReward reward)
        {
            OnImprovementRewarded?.Invoke(this, reward);
            OnIdleObjectLevelStageRisen?.Invoke(this, reward);
        }
        
        public void CollectCurrency() {
            this.CollectCurrency(this);
        }

        public void CollectCurrency(object sender) {
            if (!this.state.hasAnyCollectedCurrency)
                return;

            Bank.AddSoftCurrency(state.collectedCurrency, sender);
            this.NotifyAboutCurrencyCollected(this, state.collectedCurrency);
            SFX.PlaySFX(this.m_sounds.m_audioClipCollect);
            this.state.collectedCurrency = new BigNumber(0);
            if (!this.autoplayEnabled) {
                this.timerValue = 0f;
                this.state.progressNomalized = 0f;
            }

            this.NotifyAboutStateChanged();
        }

        public void CollectCurrencyInstantly() {
            if (!this.state.hasAnyCollectedCurrency)
                return;

            Bank.uiBank.AddSoftCurrency(this, this.state.collectedCurrency);
            Bank.AddSoftCurrency(this.state.collectedCurrency, this);
            this.NotifyAboutCurrencyCollected(null, this.state.collectedCurrency);
            this.state.collectedCurrency = new BigNumber(0);
            if (!this.autoplayEnabled) {
                this.timerValue = 0f;
                this.state.progressNomalized = 0f;
            }

            this.NotifyAboutStateChanged();
        }
        
        


        public void Build()
        {
            state.isBuilded = true;
            state.isBuildedAnyTime = true;
            state.level = 1;
            Bank.SpendSoftCurrency(info.priceToBuild, this);
            NotifyAboutStateChanged();
            NotifyAboutBuilt();
        }

        public void NextLevel()
        {
            if (this.state.level >= MAX_LEVEL)
                return;
            
            if (!Bank.isEnoughtSoftCurrency(this.state.priceImprovement) || 
                this.state.level >= IdleObjectInfo.MAX_LEVEL)
            {
                SFX.PlaySFX(this.m_sounds.m_audioClipError);
                this.NotifyAboutNextLevel(false);
                return;
            }

            Bank.SpendSoftCurrency(state.priceImprovement, this);
            this.state.level++;
            this.state.priceImprovement = this.info.priceImproveDefault * 
                                          Math.Pow(this.info.priceStep, this.state.level);
            this.CheckLevelImprovements(this.state.level);
            this.NotifyAboutStateChanged();
            this.NotifyAboutNextLevel(true);
            SFX.PlaySFX(this.m_sounds.m_audioClipNextLevel);
        }
        
        public void ForceNextLevel()
        {
            if (this.state.level >= MAX_LEVEL)
                return;
            
            this.state.level++;
            this.state.priceImprovement = this.info.priceImproveDefault * 
                                          Math.Pow(this.info.priceStep, this.state.level);
            this.CheckLevelImprovements(this.state.level);
            this.NotifyAboutStateChanged();
            this.NotifyAboutNextLevel(true);
            SFX.PlaySFX(this.m_sounds.m_audioClipNextLevel);
        }

        private void CheckLevelImprovements(int level)
        {
            if (this.levelImprovementBlock == null)
            {
                return;
            }

            if (this.levelImprovementBlock.lastLevel != level)
            {
                return;
            }

            var rewardInfo = this.levelImprovementBlock.rewardInfo;
            var reward = new LevelImprovementReward(rewardInfo);
            reward.Apply(this);
            this.UpdateLevelImprovementBlock(level);
            this.NotifyAboutImprovementRewarded(reward);

            CommonAnalytics.LogIdleObjectLevelRisen(this.state.id, this.state.level);
        }

        public void IncreaseSpeed(float speedBoost)
        {
            float newIncomePeriod = state.incomePeriod / speedBoost;
            state.SetIncomePeriod(newIncomePeriod);
            NotifyAboutStateChanged();
        }

        public void SetActiveAutoPlay(bool isActive)
        {
            state.autoPlayEnabled = isActive;
            NotifyAboutStateChanged();
        }

        public void IncreaseIncome(int incomeBoost)
        {
            state.localMultiplicatorDynamic *= incomeBoost;
        }

        public void NotifyAboutWorkStarted()
        {
            OnWorkStarted?.Invoke();
        }

        public void ResetObject() {
            this.isWorking = false;
            this.timerValue = 0f;
            var incomePeriod = this.state.incomePeriod;
            this.state.CleanExceptConstants(this.info);
            this.state.incomePeriod = incomePeriod;
            this.UpdateLevelImprovementBlock(this.state.level);
            this.OnResetEvent?.Invoke();
            OnIdleObjectResetEvent?.Invoke(this);
        }
        
        public void AddIncomeMultiplier(string id, Coefficient multiplier) {
            state.incomeConstMultiplicator.SetCoefficient(id, multiplier);
        }

        public void RemoveIncomeMultiplier(string id) {
            state.incomeConstMultiplicator.RemoveCoefficient(id);
        }

        [Serializable]
        public sealed class Sounds
        {
            [SerializeField]
            public AudioClip m_audioClipCollect;

            [SerializeField] 
            public AudioClip m_audioClipNextLevel;

            [SerializeField] 
            public AudioClip m_audioClipError;
        }
    }
}