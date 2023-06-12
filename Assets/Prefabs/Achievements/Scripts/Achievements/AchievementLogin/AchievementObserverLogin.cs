using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Achievements
{
    public class AchievementObserverLogin : AchievementObserver
    {
        private DailyRewardInteractor dailyRewardInteractor;
        public AchievementObserverLogin(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            dailyRewardInteractor = Game.GetInteractor<DailyRewardInteractor>();
        }

        protected override void SubscribeOnEvents()
        {
            dailyRewardInteractor.OnDailyBonusReceivedEvent += OnDailyBonusReceived;
        }

        protected override void UnsubscribeFromEvents()
        {
            dailyRewardInteractor.OnDailyBonusReceivedEvent -= OnDailyBonusReceived;
        }
        
        private void OnDailyBonusReceived(Reward reward)
        {
            var achieveState = this.achievement.GetState<AchievementStateLogin>();
            if (achieveState.loginDayTimes <= dailyRewardInteractor.day)
            {
                AchievementCompleteAndSave();
            }
        }
        
        protected override void CheckState()
        {
            throw new NotImplementedException();
        }
    }
}