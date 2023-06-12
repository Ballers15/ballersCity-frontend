using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStateLogin : AchievementState
    {
        [SerializeField]
        public int loginDayTimes;
        
        public AchievementStateLogin(string stateJson) : base(stateJson)
        {
        }

        public AchievementStateLogin(AchievementInfoLogin info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStateLogin>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            loginDayTimes = state.loginDayTimes;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
        
    }
}