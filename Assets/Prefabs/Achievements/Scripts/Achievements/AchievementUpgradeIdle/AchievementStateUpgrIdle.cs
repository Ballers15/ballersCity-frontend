using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateUpgrIdle : AchievementState
    {
        [SerializeField]
        public int lvlToUpgrade;
        
        [SerializeField]
        public int needToUpgradeCount;
        public AchievementStateUpgrIdle(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateUpgrIdle(AchievementInfoUpgrIdle info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateUpgrIdle>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            lvlToUpgrade = state.lvlToUpgrade;
            needToUpgradeCount = state.needToUpgradeCount;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}