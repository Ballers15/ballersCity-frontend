using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoGemTree",
        menuName = "Achievements/AchievementInfoGemTree"
    )]
    public sealed class AchievementInfoGemTree : AchievementInfo
    {
        [SerializeField]
        public int levelToUpgrade;

        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateGemTree(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateGemTree(this);
            state.levelToUpgrade = levelToUpgrade;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverGemTree(achievement);
        }
    }
}