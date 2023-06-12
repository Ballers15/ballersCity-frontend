using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoLogin",
        menuName = "Achievements/AchievementInfoLogin"
    )]
    public sealed class AchievementInfoLogin : AchievementInfo
    {
        [SerializeField]
        public int loginDayTimes;

        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStateLogin(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStateLogin(this);
            state.loginDayTimes = loginDayTimes;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverLogin(achievement);
        }
    }
}