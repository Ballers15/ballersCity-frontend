using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateLevelUp : AchievementState
    {
        [SerializeField]
        public int upToLvl;
        
        public AchievementStateLevelUp(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateLevelUp(AchievementInfoLevelUp info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateLevelUp>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            upToLvl = state.upToLvl;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}