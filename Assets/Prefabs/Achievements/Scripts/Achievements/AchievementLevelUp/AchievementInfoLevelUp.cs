using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoLevelUp",
        menuName = "Achievements/AchievementInfoLevelUp"
    )]
    public sealed class AchievementInfoLevelUp : AchievementInfo
    {
        [SerializeField]
        public int upToLvl;

        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateLevelUp(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateLevelUp(this);
            state.upToLvl = upToLvl;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverLevelUp(achievement);
        }
    }
}