using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Achievements
{
    public class AchievementObserverPersonLvlUp : AchievementObserver
    {
        //private CardObjectUpgradeInteractor upgradeCardInteractor;
        private AchievementStatePersonLvlUp _achieveState;
        public AchievementObserverPersonLvlUp(AchievementEntity achievement) : base(achievement)
        {
           
        }

        protected override void Initialize()
        {
            //upgradeCardInteractor = Game.GetInteractor<CardObjectUpgradeInteractor>();
            _achieveState = this.achievement.GetState<AchievementStatePersonLvlUp>();
            AchievementCompleteAndSave();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            //upgradeCardInteractor.OnCardUpgradedEvent += OnCardUpgraded;
        }

        protected override void UnsubscribeFromEvents()
        {
            //upgradeCardInteractor.OnCardUpgradedEvent -= OnCardUpgraded;
        }
        
        private void OnCardUpgraded(ICard card)
        {
            //var upgradedCardLevel = card.currentLevelIndex + 1;
            /*if (_achieveState.levelToUpgrade == upgradedCardLevel)
            {
                AchievementCompleteAndSave();
            }*/
        }
        
        protected override void CheckState()
        {
            /*var cardInteractor = Game.GetInteractor<CardObjectDataInteractor>();
            var cardList = cardInteractor.GetCardObjects();
            foreach (var card in cardList)
            {
                if(card.currentLevelIndex < _achieveState.levelToUpgrade) continue;
                AchievementCompleteAndSave();
                return;
            }*/
        }
    }
}

