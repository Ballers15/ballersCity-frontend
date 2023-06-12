using Orego.Util;
using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    public abstract class AirShipBehaviour : AutoMonoBehaviour {
        
        #region EVENTS

        public delegate void AirShipBehaviourHandler();
        public event AirShipBehaviourHandler OnClickedEvent;
        public event AirShipBehaviourHandler OnAnimationFinishedEvent;
        public event AirShipBehaviourHandler OnParcelUsedEvent;

        public delegate void AirShipDroppingBoxHandler(AirShipDroppingBox airShipDroppingBox);
        public event AirShipDroppingBoxHandler OnDroppingBoxExploded;

        #endregion
        
        
        [SerializeField] protected AirShip airShip;

        
        public RewardInfo rewardInfo { get; private set; }
        public bool needToWatchAds { get; private set; }

        protected bool isParcelDropped;
        protected Animator animator;


        public void SetupParcel(AirDropParcel parcel) {
            this.rewardInfo = parcel.rewardInfoBuilder.Build();
            this.needToWatchAds = parcel.needWatchAds;
        }

        public void DropParcel() {
            if (this.isParcelDropped)
                return;

            this.isParcelDropped = true;
            this.airShip.DropBox();
            this.airShip.OnDroppingBoxExploded += OnBoxExploded;
        }

        private void OnBoxExploded(AirShipDroppingBox droppingBox) {
            this.airShip.OnDroppingBoxExploded -= OnBoxExploded;
            Destroy(this.rewardInfo);
            this.OnParcelUsedEvent?.Invoke();
            this.OnDroppingBoxExploded?.Invoke(droppingBox);
        }

        protected virtual void Awake() {
            this.animator = this.Get<Animator>();
            this.airShip.OnClickedEvent += this.OnAirShipClicked;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            this.airShip.OnClickedEvent -= this.OnAirShipClicked;
        }

        public void Enable() {
            this.animator.enabled = true;
        }

        public void Disable() {
            this.animator.enabled = false;
        }


        protected void OnAirShipClicked() {
            if (!this.isParcelDropped) 
                this.OnClickedEvent?.Invoke();
        }

        public void OnAnimationFinished() {
            this.OnAnimationFinishedEvent?.Invoke();
        }
    }
}