/*using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Tools;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Services;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;
using CardObjectExtensions = SinSity.Core.CardObjectExtensions;

namespace SinSity.UI
{
    public class UIPopupPersonageCard : UIPopupAnim<UIPopupPersonageCardProperties, UIPopupArgs>
    {
        private CardObjectUpgradeInteractor cardUpgradeInteractor;
        private IdleObjectsInteractor idleObjectsInteractor;
        private CardObject cardObjectCurrent;

        private bool wasPersonalUpgraded;

        #region Initializing

        public override void Initialize()
        {
            base.Initialize();

            this.cardUpgradeInteractor = Game.GetInteractor<CardObjectUpgradeInteractor>();
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();

            this.HideInstantly();
        }

        #endregion

        private void OnEnable()
        {
            this.properties.btnUpgrade.onClick.AddListener(OnUpgradeBtnClick);
            this.properties.btnClose.onClick.AddListener(OnCloseBtnClick);

            this.wasPersonalUpgraded = false;

            LogPopupOpened();
        }


        private void OnDisable()
        {
            this.properties.btnUpgrade.onClick.RemoveListener(OnUpgradeBtnClick);
            this.properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
        }


        #region Setup

        public void Setup(CardObject cardObject)
        {
            this.cardObjectCurrent = cardObject;
            IdleObject idleObject = idleObjectsInteractor.GetIdleObject(cardObject.info.targetIdleObjectId);

            SetupTitle(idleObject);
            SetupIcon(cardObject);

            UpdateView();
        }

        private void SetupTitle(IdleObject idleObject)
        {
            string ioTitleCode = idleObject.info.GetTitle();
            string ioTitleLocalized = Localization.GetTranslation(ioTitleCode);
            this.properties.SetObjectTitle(ioTitleLocalized);
        }

        private void SetupIcon(CardObject cardObject)
        {
            this.properties.SetIcon(cardObject.info.spriteIconThin);
        }

        #endregion


        #region UpdateView

        public void UpdateView()
        {
            if (this.cardObjectCurrent == null)
                return;

            IdleObject idleObject = idleObjectsInteractor.GetIdleObject(this.cardObjectCurrent.info.targetIdleObjectId);

            SetupIncomeValue(idleObject);
            SetupProgressBar(cardObjectCurrent);
            SetupPrice(cardObjectCurrent);
            SetupIncomeSpeed(cardObjectCurrent);
        }

        private void SetupIncomeValue(IdleObject idleObject)
        {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            string incomePerSecToString = idleObject.incomePerSec.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            string localizingString = $"{incomePerSecToString} /{Localization.GetTranslation("ID_SEC")}";
            this.properties.SetIncomeValue(localizingString);
        }

        private void SetupProgressBar(CardObject cardObject)
        {
            string progressValueText = this.GetProgressValueText(cardObject);
            float progressNormalized = 1f;

            if (cardObject.HasNextLevel())
                progressNormalized = cardObject.GetCollectedCardsForNextLevelPercent();

            this.properties.SetProgress(progressNormalized, progressValueText);
        }

        private string GetProgressValueText(CardObject cardObject)
        {
            if (cardObject.HasNextLevel())
            {
                var nextLevel = cardObject.GetNextLevel();
                int requiredCardCount = nextLevel.m_requiredCardCountForUpgrade;
                return $"{cardObject.currentCardCount}/{requiredCardCount}";
            }

            return CardObjectInfo.MAX_LEVEL_TEXT;
        }

        private void SetupPrice(CardObject cardObject)
        {
            int price = 0;
            bool hasNextLevel = cardObject.HasNextLevel();
            bool enoughCards = false;
            if (hasNextLevel)
            {
                enoughCards = cardObject.IsCollectionReadyForNextLevel();
                var nextLevel = cardObject.GetNextLevel();
                price = nextLevel.m_gemsPrice;
            }

            bool hasNextLevelAndEnoughCards = hasNextLevel && enoughCards;
            this.properties.SetPrice(price, hasNextLevelAndEnoughCards);
        }

        private void SetupIncomeSpeed(CardObject cardObject)
        {
            string levelMultiplierText = GetIncomeSpeedMultiplier(cardObject);
            this.properties.SetSpeedValue(levelMultiplierText);
        }

        private string GetIncomeSpeedMultiplier(CardObject cardObject)
        {
            int currentLevelIndex = cardObject.currentLevelIndex;
            double levelMultiplier = Math.Pow(CardObjectInfo.SPEED_BOOST_MULTIPLIER, currentLevelIndex);
            return $"x{levelMultiplier}";
        }

        #endregion


        #region Events

        private void OnUpgradeBtnClick()
        {
            UIPopupPersonageCardAnimator animator = this.properties.animator;

            if (!this.cardUpgradeInteractor.IsEnoughCards(this.cardObjectCurrent))
            {
                animator.PlayNotEnoughCards();
                SFX.PlaySFX(this.properties.sfxNotEnoughGems);
                return;
            }

            if (!Bank.isEnoughtHardCurrency(this.cardObjectCurrent.GetNextLevel().m_gemsPrice))
            {
                SFX.PlaySFX(this.properties.sfxNotEnoughGems);
                animator.PlayNotEnoughGems();
                return;
            }

            this.cardUpgradeInteractor.UpgradeCard(this.cardObjectCurrent);
            this.wasPersonalUpgraded = true;

            SetupPrice(cardObjectCurrent);

            IdleObject idleObject = idleObjectsInteractor.GetIdleObject(cardObjectCurrent.info.targetIdleObjectId);
            var animationData = new UIPopupPersonageCardAnimator.CardLevelUpgradeAnimationData();
            animationData.properties = this.properties;
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            animationData.newIncomeValueText = idleObject.incomePerSec.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            animationData.newSpeedValueText = this.GetIncomeSpeedMultiplier(cardObjectCurrent);

            animator.PlayUpgradeSuccessful(animationData);

            SFX.PlaySFX(this.properties.sfxCardUpgradedSuccessfully);
        }

        private void OnCloseBtnClick()
        {
            this.Hide();
            SFX.PlayClosePopup();

            this.LogPopupClosed();
        }

        #endregion


        #region Analytics

        private void LogPopupOpened()
        {
            if (this.cardObjectCurrent == null)
                return;

            bool readyForUpgrade = this.cardObjectCurrent.IsCollectionReadyForNextLevel();
            if (this.cardObjectCurrent.HasNextLevel()) {
                var nextLevel = this.cardObjectCurrent.GetNextLevel();
                bool enoughGems = Bank.isEnoughtHardCurrency(nextLevel.m_gemsPrice);
                CommonAnalytics.LogPopupPersonageCardOpened(readyForUpgrade, enoughGems);
            }
        }

        private void LogPopupClosed()
        {
            if (this.cardObjectCurrent == null)
                return;

            if (this.cardObjectCurrent.HasNextLevel())
            {
                var nextLevel = this.cardObjectCurrent.GetNextLevel();
                var enoughGems = Bank.isEnoughtHardCurrency(nextLevel.m_gemsPrice);
                CommonAnalytics.LogPopupPersonageCardClosed(enoughGems, this.wasPersonalUpgraded);
            }
        }

        #endregion
    }
}*/