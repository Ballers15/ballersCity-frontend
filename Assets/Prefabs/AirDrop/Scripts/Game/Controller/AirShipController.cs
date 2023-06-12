using System;
using System.Runtime.CompilerServices;
using Orego.Util;
using SinSity.Domain;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using Random = UnityEngine.Random;

namespace SinSity.Core {
    [CreateAssetMenu(
        fileName = "AirShipController",
        menuName = "Game/AirDrop/New AirShipController"
    )]
    public sealed class AirShipController : ScriptableObject, IBankStateWithoutNotification {

        #region CONSTANTS

        private const int REGULAR_AIRSHIP_COUNT_MIN = 1;
        private const int REGULAR_AIRSHIP_COUNT_MAX = 2;

        #endregion
        
        #region Event

        public event Action<AirShipBehaviour> OnAirShipClickedEvent;
        public event Action<AirShipBehaviour> OnAirShipCreatedEvent; 

        public event Action<AirShipBehaviour> OnAirDropRewardReceived;

        #endregion

        public AirShipParcelSupplier parcelSupplier { get; private set; }
        private bool hasAirShip;
        [SerializeField] private Params m_params;
        private AirShipSpawnParams spawnParams;
        private Camera camera;
        private AirShipBehaviour currentAirShip;
        private float extraRemainingSpawnSeconds;
        private float standardRemainingSpawnSeconds;
        private MiniQuestInteractor miniQuestInteractor;

        #region Awake

        private void Awake() {
            if(!Application.isPlaying) return;
            
            camera = Camera.main;
            InputManager.OnInputTap += this.OnInputTap;
            parcelSupplier = m_params.parcelSupplierAssetRegular;
            spawnParams = m_params.spawnParamsRegular;
            standardRemainingSpawnSeconds = spawnParams.GetStandardRandomSpawnPeriod();
            extraRemainingSpawnSeconds = spawnParams.GetExtraRandomSpawnPeriod();
            
            m_params.parcelSupplierAssetRegular.Initialize();
            miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
        }

        #endregion

        public void OnUpdate(float unscaledDeltaTime) {
            if (hasAirShip || !HasActiveAirShipDailyQuest())
                return;

            standardRemainingSpawnSeconds -= unscaledDeltaTime;
            extraRemainingSpawnSeconds -= unscaledDeltaTime;
            if (standardRemainingSpawnSeconds > 0 && extraRemainingSpawnSeconds > 0) {
                return;
            }

            InstantiateAirShip();
        }

        private bool HasActiveAirShipDailyQuest() {
            return miniQuestInteractor.HasActiveQuestOfType<QuestInfoCollectAirDrop>();
        }

        private void InstantiateAirShip() {
            var prefab = GetPrefab();
            var newAirShip = Instantiate(prefab, null);
            var airShipTransform = newAirShip.transform;
            //Set relative position:
            var relativePosition = spawnParams.GetRandomRelativePosition();
            var centerPoint = camera.ViewportToWorldPoint(relativePosition);
            airShipTransform.position = centerPoint;
            
            //Set inversion:
            if (spawnParams.randomX) {
                var localScale = airShipTransform.localScale;
                airShipTransform.localScale = new Vector3(
                    localScale.x * OregoIntUtils.RandomSign(),
                    localScale.y,
                    localScale.z
                );
            }

            //Set parcel:
            var nextParcel = this.parcelSupplier.SupplyNextParcel();
            newAirShip.SetupParcel(nextParcel);

            //Assign air ship:
            this.currentAirShip = newAirShip;
            this.hasAirShip = true;
            this.currentAirShip.OnClickedEvent += this.OnAirShipClicked;
            this.currentAirShip.OnAnimationFinishedEvent += this.OnAirShipAnimationFinished;
            this.currentAirShip.OnParcelUsedEvent += this.OnParcelUsed;
            
            this.OnAirShipCreatedEvent?.Invoke(this.currentAirShip);
        }

        private AirShipBehaviour GetPrefab() {
            this.parcelSupplier = this.m_params.parcelSupplierAssetRegular;
            this.spawnParams = this.m_params.spawnParamsRegular;
            return this.m_params.airShipPrefabRegular;
        }
        
        #region Reset

        public void DestroyAirShip() {
            if (this.hasAirShip) {
                this.currentAirShip.OnClickedEvent -= this.OnAirShipClicked;
                this.currentAirShip.OnAnimationFinishedEvent -= this.OnAirShipAnimationFinished;
                this.currentAirShip.OnParcelUsedEvent -= this.OnParcelUsed;
                Destroy(this.currentAirShip.gameObject);
            }

            this.standardRemainingSpawnSeconds = this.spawnParams.GetStandardRandomSpawnPeriod();
            this.extraRemainingSpawnSeconds = this.spawnParams.GetExtraRandomSpawnPeriod();
            this.currentAirShip = null;
            this.hasAirShip = false;
        }

        #endregion

        public void ReceiveAirDropReward(AirShipBehaviour airShipBehaviour) {
            var rewardInfo = airShipBehaviour.rewardInfo;
            var reward = new Reward(rewardInfo);
            reward.Apply(this, false);
            this.OnAirDropRewardReceived?.Invoke(airShipBehaviour);
        }

        #region Event

        private void OnInputTap() {
            if (!this.hasAirShip)
                this.extraRemainingSpawnSeconds = this.spawnParams.GetExtraRandomSpawnPeriod();
        }

        private void OnAirShipClicked() {
            this.OnAirShipClickedEvent?.Invoke(this.currentAirShip);
        }

        private void OnAirShipAnimationFinished() {
            this.DestroyAirShip();
        }

        private void OnParcelUsed() {
            this.parcelSupplier.NotifyAboutParcelUsed();
        }

        #endregion

        [Serializable]
        public sealed class Params {
            public AirShipParcelSupplier parcelSupplierAssetRegular;
            public AirShipSpawnParams spawnParamsRegular;
            public AirShipBehaviour airShipPrefabRegular;
        }
    }
}