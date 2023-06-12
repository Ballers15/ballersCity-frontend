using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoBuildIdle",
        menuName = "Achievements/AchievementInfoBuildIdle"
    )]
    public sealed class AchievementInfoBuildIdle : AchievementInfo
    {
        [SerializeField]
        public string needToBuildIdleId;

        [SerializeField]
        public int needToBuildCount;
        
        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateBuildIdle(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateBuildIdle(this);
            state.needToBuildIdleId = needToBuildIdleId;
            state.needToBuildCount = needToBuildCount;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverBuildIdle(achievement);
        }
    }
}