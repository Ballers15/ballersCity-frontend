using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoModern",
        menuName = "Achievements/AchievementInfoModern"
    )]
    public sealed class AchievementInfoModern : AchievementInfo
    {
        [SerializeField]
        public int modernizationTimes;

        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateModern(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateModern(this);
            state.modernizationTimes = modernizationTimes;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverModern(achievement);
        }
    }
}