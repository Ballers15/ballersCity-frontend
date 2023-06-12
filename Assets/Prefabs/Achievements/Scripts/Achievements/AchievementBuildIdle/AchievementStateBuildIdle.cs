using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateBuildIdle : AchievementState
    {
        [SerializeField]
        public string needToBuildIdleId;
        
        [SerializeField]
        public int needToBuildCount;
        public AchievementStateBuildIdle(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateBuildIdle(AchievementInfoBuildIdle info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateBuildIdle>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            needToBuildIdleId = state.needToBuildIdleId;
            needToBuildCount = state.needToBuildCount;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }

        public bool isCountType()
        {
            return (this.needToBuildCount > 0);
        }
        
    }
}