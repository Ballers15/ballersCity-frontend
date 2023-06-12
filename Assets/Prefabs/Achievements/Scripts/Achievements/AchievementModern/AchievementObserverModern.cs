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
    public class AchievementObserverModern : AchievementObserver
    {
        private ModernizationInteractor modernInteractor;
        private AchievementStateModern _achieveState;
        public AchievementObserverModern(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            modernInteractor = Game.GetInteractor<ModernizationInteractor>();
            _achieveState = achievement.GetState<AchievementStateModern>();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            modernInteractor.OnModernizationCompleteEvent += OnModernizationComplete;
        }

        protected override void UnsubscribeFromEvents()
        {
            modernInteractor.OnModernizationCompleteEvent -= OnModernizationComplete;
        }
        
        private void OnModernizationComplete(ModernizationInteractor interactor, object sender)
        {
            if (_achieveState.modernizationTimes != interactor.modernizationData.renovationIndex) return;
            AchievementCompleteAndSave();
        }
        
        protected override void CheckState()
        {
            if(modernInteractor.modernizationData.renovationIndex >= _achieveState.modernizationTimes)
                AchievementCompleteAndSave();
        }
    }
}