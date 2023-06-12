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
    public class AchievementObserverGemTree : AchievementObserver
    {
        private GemTreeStateInteractor gemTreeInteractor;
        private AchievementStateGemTree _achieveState;
        public AchievementObserverGemTree(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            gemTreeInteractor = Game.GetInteractor<GemTreeStateInteractor>();
            _achieveState = achievement.GetState<AchievementStateGemTree>();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            gemTreeInteractor.OnCurrentLevelChangedEvent += OnCurrentLevelChanged;
        }

        protected override void UnsubscribeFromEvents()
        {
            gemTreeInteractor.OnCurrentLevelChangedEvent -= OnCurrentLevelChanged;
        }
        
        private void OnCurrentLevelChanged(object obj, int level)
        {
            if (_achieveState.levelToUpgrade == level)
            {
                AchievementCompleteAndSave();
            }
        }

        protected override void CheckState()
        {
            if(gemTreeInteractor.currentLevelIndex >= _achieveState.levelToUpgrade) AchievementCompleteAndSave();
        }
    }
}