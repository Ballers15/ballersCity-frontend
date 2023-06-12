using System;
using System.Collections;
using Prefabs.AirDrop.Scripts.Analytics;
using SinSity.Core;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

namespace SinSity.Domain {
    public sealed class AirDropInteractor : Interactor,
        IUpdateListenerInteractor,
        IModernizationAsyncListenerInteractor {
        #region Const

        private const string AIR_SHIP_CONTROLLER_PATH = "AirShipController";

        #endregion

        #region Event

        public event Action<object> OnAirDropUnlockedEvent;

        #endregion

        public bool isAirDropEnabled { get; private set; }

        private new AirDropRepository repository;

        public AirShipController airShipController { get; private set; }
        public AirDropAnalytics analytics { get; private set; }
        
        public override bool onCreateInstantly { get; } = true;


        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<AirDropRepository>();
            var airShipSpawnerAsset = Resources.Load<AirShipController>(AIR_SHIP_CONTROLLER_PATH);
            this.airShipController = Object.Instantiate(airShipSpawnerAsset);
            this.analytics = new AirDropAnalytics(this.airShipController);
        }

        public override void OnInitialized() {
            base.OnInitialized();
            this.LoadState();
        }

        private void LoadState() {
            var statistics = this.repository.GetStatistics();
            this.isAirDropEnabled = statistics.isAirDropEnabled;
            if (statistics.isLuckyModeEnabled) {
                var airShipParcelSupplier = this.airShipController.parcelSupplier;
                airShipParcelSupplier.isLuckyModeEnabled = true;
                airShipParcelSupplier.luckyIndex = statistics.luckyIndex;
                airShipParcelSupplier.OnLuckyIndexChangedEvent += this.OnLuckyIndexChanged;
                airShipParcelSupplier.OnLuckyPipelineFinishedEvent += this.OnLuckyPipelineFinished;
            }
        }

        #endregion

        public void OnUpdate(float unscaledDeltaTime) {
            if (!this.isAirDropEnabled) {
                return;
            }

            this.airShipController.OnUpdate(unscaledDeltaTime);
        }

        public IEnumerator OnStartModernizationAsync() {
            this.airShipController.DestroyAirShip();
            yield break;
        }

        public void UnlockAirDrop(object sender) {
            this.repository.SetAirDropEnabled(true);
            this.isAirDropEnabled = true;
            this.airShipController.DestroyAirShip();
            this.OnAirDropUnlockedEvent?.Invoke(sender);
        }

        #region Events

        private void OnLuckyIndexChanged(int newLuckyIndex) {
            this.repository.SetLuckyIndex(newLuckyIndex);
        }

        private void OnLuckyPipelineFinished() {
            var airShipParcelSupplier = this.airShipController.parcelSupplier;
            airShipParcelSupplier.OnLuckyIndexChangedEvent -= this.OnLuckyIndexChanged;
            airShipParcelSupplier.OnLuckyPipelineFinishedEvent -= this.OnLuckyPipelineFinished;
            this.repository.SetLuckyModeEnabled(false);
        }

        #endregion
    }
}