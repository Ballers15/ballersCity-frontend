using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementStatePersonLvlUp : AchievementState
    {
        [SerializeField]
        public string personIdToUpgrade;
        
        [SerializeField]
        public int levelToUpgrade;
        public AchievementStatePersonLvlUp(string stateJson) : base(stateJson)
        {
        }

        public AchievementStatePersonLvlUp(AchievementInfoPersonLvlUp info) : base(info)
        {
            
        }
        
        public override void SetState(string stateJson)
        {
            var state = JsonUtility.FromJson<AchievementStatePersonLvlUp>(stateJson);
            id = state.id;
            isActive = state.isActive;
            isCompleted = state.isCompleted;
            isHidden = state.isHidden;
            isShown = state.isShown;
            personIdToUpgrade = state.personIdToUpgrade;
            levelToUpgrade = state.levelToUpgrade;
        }
        
        public override string GetStateJson()
        {
            return JsonUtility.ToJson(this);
        }
        
    }
}