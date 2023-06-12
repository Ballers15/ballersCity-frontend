using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateGemTree : AchievementState
    {
        [SerializeField]
        public int levelToUpgrade;
        
        public AchievementStateGemTree(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateGemTree(AchievementInfoGemTree info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateGemTree>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            levelToUpgrade = state.levelToUpgrade;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
        
    }
}