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
    public class AchievementObserverLevelUp : AchievementObserver
    {
        private ProfileLevelInteractor profileInteractor;
        private AchievementStateLevelUp _achieveState;
        public AchievementObserverLevelUp(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            profileInteractor = Game.GetInteractor<ProfileLevelInteractor>();
            _achieveState = achievement.GetState<AchievementStateLevelUp>();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            profileInteractor.OnLevelRisenEvent += OnLevelRisen;
        }

        protected override void UnsubscribeFromEvents()
        {
            profileInteractor.OnLevelRisenEvent -= OnLevelRisen;
        }
        
        private void OnLevelRisen(object obj, int level)
        {
            if (_achieveState.upToLvl > level) return;
            AchievementCompleteAndSave();
        }
        
        protected override void CheckState()
        {
            if(profileInteractor.currentLevel >= _achieveState.upToLvl) AchievementCompleteAndSave();
        }
    }
}
