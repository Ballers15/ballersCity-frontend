using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoUpgrIdle",
        menuName = "Achievements/AchievementInfoUpgrIdle"
    )]
    public sealed class AchievementInfoUpgrIdle : AchievementInfo
    {
        [SerializeField]
        public int lvlToUpgrade;

        [SerializeField]
        public int needToUpgradeCount;
        
        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateUpgrIdle(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateUpgrIdle(this);
            state.lvlToUpgrade = lvlToUpgrade;
            state.needToUpgradeCount = needToUpgradeCount;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverUpgrIdle(achievement);
        }
    }
}