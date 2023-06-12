using System;
using Orego.Util;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class AirShip : AutoMonoBehaviour
    {
        #region Const

        private static readonly int DROP_BOX_ANIMATOR_KEY = Animator.StringToHash("drop_box");

        #endregion

        #region Event

        public delegate void AirShipClickHandler();
        public event AirShipClickHandler OnClickedEvent;

        public delegate void AirShipDroppingBoxHandler(AirShipDroppingBox droppingBox);
        public event AirShipDroppingBoxHandler OnDroppingBoxExploded;

        #endregion

        [SerializeField]
        private Params m_params;

        private Animator animator;

        private AirShipDroppingBox currentDroppingBoxHolder;

        private void Awake()
        {
            this.animator = this.m_params.m_animator;
        }

        #region OnMouseDown

        private void OnMouseDown() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var uiController = uiInteractor.uiController;
            var popupAirDrop = uiController.GetUIElement<UIPopupAirDrop>();
            var allowToClick = !popupAirDrop.isActive && uiController.OnlyGameplayScreenOpened() && !BluePrint.bluePrintModeEnabled;
            if (allowToClick)
                this.OnClickedEvent?.Invoke();
        }

        private bool IsValidMoment() {
            return !BluePrint.bluePrintModeEnabled;
        }

        public void DropBox() {
            if (animator == null)
                return;

            this.animator.SetTrigger(DROP_BOX_ANIMATOR_KEY);
            this.currentDroppingBoxHolder = Instantiate(
                this.m_params.m_droppingBoxHolderPrefab,
                this.transform.position,
                Quaternion.identity
            );
            currentDroppingBoxHolder.OnExplodeStart += OnExplodeStart;
        }

        private void OnExplodeStart(AirShipDroppingBox droppingbox) {
            droppingbox.OnExplodeStart -= OnExplodeStart;
            OnDroppingBoxExploded?.Invoke(droppingbox);
        }

        #endregion

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (this.currentDroppingBoxHolder != null)
            {
                Destroy(this.currentDroppingBoxHolder);
            }
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public Animator m_animator;

            [SerializeField]
            public AirShipDroppingBox m_droppingBoxHolderPrefab;
        }
    }
}