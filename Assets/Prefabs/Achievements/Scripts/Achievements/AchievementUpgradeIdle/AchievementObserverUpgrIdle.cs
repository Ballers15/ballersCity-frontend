using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Achievements;
using SinSity.Meta;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Achievements
{
    public class AchievementObserverUpgrIdle : AchievementObserver
    {
        private IdleObjectsInteractor idleInteractor;
        private AchievementStateUpgrIdle _achieveState;
        public AchievementObserverUpgrIdle(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            idleInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            _achieveState = this.achievement.GetState<AchievementStateUpgrIdle>();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            IdleObject.OnIdleObjectLevelRisen += OnIdleObjectLevelRisen;
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectLevelRisen -= OnIdleObjectLevelRisen;
        }

        private void OnIdleObjectLevelRisen(IdleObject idleobject, int level, bool success)
        {
            CheckAndComplete();
        }

        protected override void CheckState()
        {
            CheckAndComplete();
        }

        private void CheckAndComplete()
        {
            var upgradedIdlesCount = CountUpgradedIdles();
            if (upgradedIdlesCount >= _achieveState.needToUpgradeCount)
                AchievementCompleteAndSave();
        }
        
        private int CountUpgradedIdles()
        {
            var idleList = idleInteractor.GetBuildedIdleObjects();
            if (idleList.Length < _achieveState.needToUpgradeCount) return 0;
            var count = idleList.Count(idle => idle.state.level >= _achieveState.lvlToUpgrade);
            return count;
        }
    }
}

