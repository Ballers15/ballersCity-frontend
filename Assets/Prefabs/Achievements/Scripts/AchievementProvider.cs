using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    public abstract class AchievementProvider
    {
        #region CONSTANTS

        protected const bool SUCCESS = true;
        protected const bool FAIL = false;

        #endregion
        
        public AchievementProvider() {
            Initialize();
        }

        protected abstract void Initialize();

        public abstract void CompleteAchievement(AchievementEntity achievement);
        public abstract void ShowAchievement(AchievementEntity achievement);
        public abstract void ShowAchievementsList();
    }
}

