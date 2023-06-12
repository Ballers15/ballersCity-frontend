using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;
using System;
using System.Collections;
using VavilichevGD.Monetization;
using SinSity.Services;
using SinSity.Core;
using SinSity.Extensions;
using SinSity.Repo;
using SinSity.UI;

namespace SinSity.Domain {
    public class ModernizationInteractor : Interactor {
        public override bool onCreateInstantly { get; } = true;

        #region CONSTANTS

        private const string PATH_CONFIG = "Config/ModernizationConfig";

        #endregion

        #region DELEGATES
        public delegate void ModernizationEventHandler();
        public event ModernizationEventHandler OnModernizationIsAvalible;

        public event Action<ModernizationInteractor> OnModernizationStartEvent;

        public delegate void ModernizationInteractorHandler(ModernizationInteractor interactor, object sender);
        public event ModernizationInteractorHandler OnModernizationDataStateChanged;
        
        public event ModernizationInteractorHandler OnModScoreChangedEvent;
        public event ModernizationInteractorHandler OnModernizationCompleteEvent;

        #endregion

        public ModernizationConfig config { get; private set; }
        public ModernizationAnalytics analytics { get; private set; }
        public ModernizationData modernizationData => this.repository.data;

        private ModernizationRepository repository;
        private List<IModernizationObserver> observers;

        private IdleObjectsInteractor idleInteractor;

        private IEnumerable<IModernizationAsyncListenerInteractor> modernizationListeners;
        private SaveGameInteractor saveGameInteractor;
        private CleanSlotsInteractor cleanSlotsInteractor;


        #region INITIALIZATION

        public override void OnCreate() {
            base.OnCreate();
            this.config = Resources.Load<ModernizationConfig>(PATH_CONFIG);
            this.repository = this.GetRepository<ModernizationRepository>();
            this.analytics = new ModernizationAnalytics(this, repository);
        }

        public override void OnInitialized() {
            base.OnInitialized();

            this.idleInteractor = GetInteractor<IdleObjectsInteractor>();
            IdleObject.OnIdleObjectLevelRisen += this.OnIdleObjectLevelRisen;
            this.modernizationListeners = this.GetInteractors<IModernizationAsyncListenerInteractor>();
            this.saveGameInteractor = this.GetInteractor<SaveGameInteractor>();
            this.cleanSlotsInteractor = this.GetInteractor<CleanSlotsInteractor>();

            observers = new List<IModernizationObserver> {
                new ModernizationObserverIdleBuild(config),
                new ModernizationObserverIdleUpgrd(config),
                new ModernizationObserverGameTime(config)
            };

            this.Subscribe(observers);
        }
        
        public override void OnReady() {
            base.OnReady();
            this.OnModernizationDataStateChanged?.Invoke(this,this);
        }

        #endregion

        #region MODERNIZATION

        public void StartModernization()
        {
            var uiInteractor = this.GetInteractor<UIInteractor>();
            UIScreenRenovationLoading screenRenovationLoading = uiInteractor.ShowElement<UIScreenRenovationLoading>();
            screenRenovationLoading.OnScreenReadyForRenovation += this.OnScreenReadyForModernization;
        }

        private void OnScreenReadyForModernization(UIScreenRenovationLoading screen)
        {
            screen.OnScreenReadyForRenovation -= this.OnScreenReadyForModernization;
            Coroutines.StartRoutine(this.StartModernizationRoutine());
        }

        public IEnumerator StartModernizationRoutine()
        {
            this.OnModernizationStartEvent?.Invoke(this);
            Debug.Log("DO MODERNIZATION!");
            Bank.SpendSoftCurrency(Bank.softCurrencyCount, this);

            this.repository.TransferScoresToMultiplier();
            this.repository.data.renovationIndex++;
            this.repository.Save();
           
            //var nextLevel = this.modernizationRepository.data.renovationIndex + 1;
            foreach (var renovationListener in this.modernizationListeners)
                yield return renovationListener.OnStartModernizationAsync();

            this.saveGameInteractor.SaveAll();
            this.cleanSlotsInteractor.Reset();

            this.OnModernizationCompleteEvent?.Invoke(this,this);
        }

        #endregion
        private void OnIdleObjectLevelRisen(IdleObject idleObject, int newlevel, bool success)
        {
            if (this.modernizationData.isAvailable)
                return;
            
            if(this.CanBeAvalible())
            {
                this.SetAvalible();
            }
        }

        private bool CanBeAvalible() {
            return false;
            var idleObjects = idleInteractor.GetBuildedIdleObjects();

            if (idleObjects.Length < config.idleObjectsCountToUnlock)
                return false;

            int idleCounter = 0;
            foreach (var idle in idleObjects)
            {
                if (idle.state.level >= config.updateIdleToLevelToUnlock)
                    idleCounter++;
            }

            return idleCounter >= config.idleObjectsCountToUnlock;
        }

        private void SetAvalible()
        {
            this.modernizationData.isAvailable = true;
            this.repository.Save();
            this.OnModernizationIsAvalible?.Invoke();
        }

        private void Subscribe(List<IModernizationObserver> observers)
        {
            foreach(var observer in observers)
            {
                observer.OnScoresReceivedEvent += this.OnModernizationsScoresReceived;
                observer.Subscribe();
            }
        }
        
        private void OnModernizationsScoresReceived(object sender, int scores) {
            this.repository.AddScores(scores);
            this.OnModScoreChangedEvent?.Invoke(this,sender);
            this.OnModernizationDataStateChanged?.Invoke(this,sender);
        }

        public void TransferScoresToMultiplier(object sender) {
            this.repository.TransferScoresToMultiplier();
            this.OnModernizationDataStateChanged?.Invoke(this,sender);
        }
    }
}