using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public sealed class ProfileLevelInteractor : Interactor {
        #region Const

        private const string PROFILE_LEVEL_TABLE_PATH = "ProfileLevelTable";

        private const string PROFILE_LEVEL_LISTENER_TABLE_PATH = "ProfileLevelHandlerTable";

        #endregion

        #region Event

        public event Action<object, int> OnLevelRisenEvent;

        #endregion

        public int currentLevel { get; private set; }

        public bool isMaxLevelReached => currentLevel >= levelTable.maxLevelNumber;

        public ProfileLevelTable levelTable { get; private set; }

        public Dictionary<int, IProfileLevelHandler> levelHandlerMap { get; private set; }

        private ProfileExperienceDataInteractor dataInteractor;
        
        public override bool onCreateInstantly { get; } = true;


        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.levelTable = Resources.Load<ProfileLevelTable>(PROFILE_LEVEL_TABLE_PATH);
            this.levelHandlerMap = Resources
                .Load<ProfileLevelHandlerTable>(PROFILE_LEVEL_LISTENER_TABLE_PATH)
                .LoadMap();
        }

        #endregion

        #region OnInitialized

        public override void OnInitialized() {
            this.dataInteractor = this.GetInteractor<ProfileExperienceDataInteractor>();
            this.Setup();
        }

        private void Setup() {
            var experience = this.dataInteractor.currentExperience;
            this.currentLevel = this.levelTable.GetCurrentLevel(experience);
        }

        #endregion

        public IEnumerable<IProfileLevelHandler> GetLevelHandlers() {
            return this.levelHandlerMap.Values.ToList();
        }

        public bool NextLevelReady() {
            var exp = this.dataInteractor.currentExperience;
            var nextLevel = this.levelTable.GetCurrentLevel(exp);
            return nextLevel > this.currentLevel;
        }

        #region OnReady

        public override void OnReady() {
            this.dataInteractor.OnExperienceChangedEvent += this.OnProfileExperienceChanged;
        }


        #endregion

        #region InteractorsEvent

        private void OnProfileExperienceChanged(object sender, ulong newExperience) {
            var previousLevel = this.currentLevel;
            var nextLevel = this.levelTable.GetCurrentLevel(newExperience);
            if (nextLevel == previousLevel) {
                return;
            }

            if (nextLevel < previousLevel) {
                throw new Exception("Level can not decrease!");
            }

            this.currentLevel = nextLevel;
            this.levelHandlerMap[nextLevel].OnProfileLevelRisen();
            this.OnLevelRisenEvent?.Invoke(sender, nextLevel);
            ProfileLevelAnalytics.LogPlayerLevelUp(this.currentLevel);
        }

        #endregion
    }
}