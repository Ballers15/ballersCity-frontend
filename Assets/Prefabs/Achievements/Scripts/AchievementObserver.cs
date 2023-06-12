using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    public abstract class AchievementObserver
    {
        protected AchievementEntity achievement;
        
        public AchievementObserver(AchievementEntity achievement) {
            this.achievement = achievement;
        }
        
        public virtual void StartObserve()
        {
            if (achievement.isShown) return;
            Initialize();
            if (achievement.isCompleted) return;
            SubscribeOnEvents();
        }

        public virtual void AchievementCompleteAndSave()
        {
            if (!achievement.isCompleted)
            {
                Achievement.CompleteAchievement(achievement);
            }
            else
            {
                if(!achievement.isShown) Achievement.ShowAchievement(achievement);
            }
            Achievement.Save();
            UnsubscribeFromEvents();
        }
        
        protected abstract void Initialize();
        
        protected abstract void SubscribeOnEvents();
        
        protected abstract void UnsubscribeFromEvents();

        protected abstract void CheckState();
    }
}
