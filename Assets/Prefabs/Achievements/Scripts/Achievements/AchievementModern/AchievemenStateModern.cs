using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateModern : AchievementState
    {
        [SerializeField]
        public int modernizationTimes;
        
        public AchievementStateModern(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateModern(AchievementInfoModern info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateModern>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            modernizationTimes = state.modernizationTimes;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
        
    }
}