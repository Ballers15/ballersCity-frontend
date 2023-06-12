using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;

namespace SinSity.Achievements
{
    [CreateAssetMenu(
        fileName = "AchievementInfoPersonLvlUp",
        menuName = "Achievements/AchievementInfoPersonLvlUp"
    )]
    public sealed class AchievementInfoPersonLvlUp : AchievementInfo
    {
        [SerializeField]
        public int levelToUpgrade;

        [SerializeField]
        public string personIdToUpgrade;
        
        public override AchievementState CreateState(string stateJson)
        {
            return new AchievementStatePersonLvlUp(stateJson);
        }

        public override AchievementState CreateStateDefault()
        {
            var state = new AchievementStatePersonLvlUp(this);
            state.levelToUpgrade = levelToUpgrade;
            state.personIdToUpgrade = personIdToUpgrade;
            return state;
        }

        public override AchievementObserver CreateObserver(AchievementEntity achievement)
        {
            return new AchievementObserverPersonLvlUp(achievement);
        }
    }
}