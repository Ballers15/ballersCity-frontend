using System;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPanelPersCard : UIPanel<UIPanelPersCardProperties>
    {
        /*public CardObject cardObject { get; private set; }

        private CardObjectDataInteractor cardDataInteractor;

        private CardObjectUpgradeInteractor cardUpgradeInteractor;

        private CardObjectCollectInteractor cardCollectInteractor;

        private Image imageBackground;

        private Button button;

        protected override void Awake()
        {
            base.Awake();
            this.imageBackground = this.GetComponent<Image>();
            this.button = this.GetComponent<Button>();
        }

        #region Initialize

        protected override void OnGameInitialized()
        {
            this.cardDataInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            this.cardCollectInteractor = Game.GetInteractor<CardObjectCollectInteractor>();
            this.cardCollectInteractor.OnCardCountInCollectionChangedEvent += this.OnCardCountInCollectionChanged;
            this.cardUpgradeInteractor = Game.GetInteractor<CardObjectUpgradeInteractor>();
            this.cardUpgradeInteractor.OnCardUpgradedEvent += this.OnCardUpgraded;
            this.BindCardObject();
        }

        private void BindCardObject()
        {
            var idleObject = this.GetComponentInParent<IdleObject>();
            var idleObjectId = idleObject.id;
            this.cardObject = this.cardDataInteractor.GetCardObjectByIdleObjectId(idleObjectId);
            this.Setup();
        }

        #endregion

        #region Setup

        private void Setup()
        {
            var currentLevelIndex = this.cardObject.currentLevelIndex;
            var levelMultiplier = Math.Pow(CardObjectInfo.SPEED_BOOST_MULTIPLIER, currentLevelIndex);
            this.properties.textIncomeSpeedMultiplicatorCurrentValue.text = $"x{levelMultiplier}";
            if (this.cardObject.IsCollectionReadyForNextLevel())
            {
                this.SetReadyForUpgradeMode();
            }
            else
            {
                this.SetPumpingMode();
            }
        }

        private void SetReadyForUpgradeMode()
        {
            var uiWidgetPersCardContentReadyForPurchase = this.properties.uiWidgetContentReadyForPurchase;
            uiWidgetPersCardContentReadyForPurchase.SetActive(true);
            uiWidgetPersCardContentReadyForPurchase.Setup(this.cardObject);
            this.properties.uiWidgetContentPumping.SetActive(false);
            this.imageBackground.sprite = this.properties.spriteReadyForUpgradeMode;
        }

        private void SetPumpingMode()
        {
            this.properties.uiWidgetContentReadyForPurchase.SetActive(false);
            var uiWidgetPersCardContentPumping = this.properties.uiWidgetContentPumping;
            uiWidgetPersCardContentPumping.SetActive(true);
            uiWidgetPersCardContentPumping.Setup(this.cardObject);
            this.imageBackground.sprite = this.properties.spritePumpingMode;
        }

        #endregion

        private void OnDestroy()
        {
            if (this.cardCollectInteractor != null)
            {
                this.cardCollectInteractor.OnCardCountInCollectionChangedEvent -= this.OnCardCountInCollectionChanged;
            }

            if (this.cardUpgradeInteractor != null)
            {
                this.cardUpgradeInteractor.OnCardUpgradedEvent -= this.OnCardUpgraded;
            }
        }

        #region Events

        private void OnCardUpgraded(CardObject otherCardObject)
        {
            if (otherCardObject == this.cardObject)
            {
                this.Setup();
            }
        }

        private void OnCardCountInCollectionChanged(object o, CardObject otherCardObject)
        {
            if (otherCardObject == this.cardObject)
            {
                this.Setup();
            }
        }

        #endregion

        private void OnEnable()
        {
            if (cardObject == null)
                return;

            if (cardObject.IsCollectionReadyForNextLevel())
                properties.animator.PlayReadyForPurchase();
            else
                properties.animator.StopReadyForPurchase();
            
            this.button.onClick.AddListener(OnBtnClick);
        }

        private void OnBtnClick() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIPopupPersonageCard popup = uiInteractor.ShowElement<UIPopupPersonageCard>();
            popup.Setup(this.cardObject);
            
            SFX.PlayOpenPopup();
        }

        private void OnDisable() {
            this.button.onClick.RemoveListener(OnBtnClick);
        }*/
    }
}