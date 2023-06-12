using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Tools;

namespace SinSity.Achievements
{
    public class AchievementRepository : Repository
    {
        protected AchievementStates states;
        public string[] stateJsons => states.listOfStates.ToArray();
        
        public AchievementRepository() {
            states = AchievementStates.empty;
        }
        
        protected const string PREF_KEY = "ACHIEVEMENT_STATES";
        
        public void Save(AchievementState[] statesArray) {
            states = new AchievementStates(statesArray);
            this.Save();
        }
        
        protected override void Initialize() {
            base.Initialize();
            this.LoadFromStorage();
        }

        private void LoadFromStorage() {
            states = Storage.GetCustom(PREF_KEY, AchievementStates.empty);
            Logging.Log("ACHIEVEMENT REPOSITORY: Loaded from the Storage");
        }
        
        public override void Save() {
            Storage.SetCustom(PREF_KEY, states);
            Logging.Log("ACHIEVEMENT REPOSITORY: Saved to the Storage");
        }
    }
}