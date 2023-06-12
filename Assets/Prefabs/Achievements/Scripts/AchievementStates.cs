using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    [Serializable]
    public class AchievementStates
    {
        public List<string> listOfStates;
        
        public AchievementStates() {
            listOfStates = new List<string>();
        }
    
        public static AchievementStates empty {
            get {
                var states = new AchievementStates();
                return states;
            }
        }
        
        public AchievementStates(AchievementState[] statesArray) {
            listOfStates = new List<string>();
            foreach (var state in statesArray) {
                var stateJson = state.GetStateJson();
                listOfStates.Add(stateJson);
            }
        }
    }
}