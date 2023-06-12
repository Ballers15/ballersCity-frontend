using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    public static class Achievement
    {
        private static AchievementInteractor _interactor;
        
        public static void Initialize(AchievementInteractor interactor) {
            _interactor = interactor;
        }

        public static void ShowAchievement(AchievementEntity achievement)
        {
            _interactor.ShowAchievement(achievement);
        }
        
        public static void CompleteAchievement(AchievementEntity achievement)
        {
            _interactor.CompleteAchievement(achievement);
        }

        public static void Save()
        {
            _interactor.SaveAllAchievements();
        }

        public static void ShowAchievementsList()
        {
            _interactor?.ShowAchievementsList();
        }
    }
}

