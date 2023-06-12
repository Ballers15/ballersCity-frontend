using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementEntity
    {
        private readonly AchievementObserver observer; 
        public AchievementInfo info { get; protected set; }
        
        public T GetInfo<T>() where T : AchievementInfo
        {
            return (T)this.info;
        }
        
        public AchievementState state { get; protected set; }

        public T GetState<T>() where T : AchievementState
        {
            return (T)this.state;
        }
        
        public bool isCompleted => state.isCompleted;
        public bool isActive => state.isActive;
        public bool isHidden => state.isHidden;
        public bool isShown => state.isShown;
        public string id => info.id;
        
        public AchievementEntity(AchievementInfo info, AchievementState state)
        {
            this.info = info;
            this.state = state;
            observer = info.CreateObserver(this);
        }

        public void StartObserve()
        {
            observer.StartObserve();
        }

        public void CompleteAchievement()
        {
            state.MarkAsCompleted();
        }

        public void ShowAchievement()
        {
            state.Show();
        }
    }
}