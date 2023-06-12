using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Achievements
{
    public class AchievementState
    {
        #region EVENTS
        
        public delegate void AchieveStateChangedHandler(AchievementState state);
        public static event AchieveStateChangedHandler OnAchieveStateChangedEvent; 
        
        #endregion

        #region VAR's

        public string id;
        public bool isActive;
        public bool isHidden;
        public bool isCompleted;
        public bool isShown;

        #endregion
        
        public AchievementState(string stateJson)
        {
            SetState(stateJson);
        }
        
        public AchievementState(AchievementInfo info)
        {
            id = info.id;
            isHidden = info.isHidden;
            isCompleted = false;
            isShown = false;
            isActive = true;
        }
        
        public virtual void SetState(string stateJson) {
            throw new NotImplementedException();
        }
        
        public virtual string GetStateJson() {
            throw new NotImplementedException();
        }
        
        public virtual void MarkAsCompleted()
        {
            isActive = false;
            isCompleted = true;
            NotifyAboutStateChanged();
        }

        public virtual void MarkAsHidden()
        {
            isHidden = true;
            NotifyAboutStateChanged();
        }

        public void Activate()
        {
            isActive = true;
            NotifyAboutStateChanged();
        }
        
        public void Show()
        {
            isShown = true;
            NotifyAboutStateChanged();
        }

        public void Deactivate()
        {
            isActive = false;
            NotifyAboutStateChanged();
        }
        
        protected void NotifyAboutStateChanged()
        {
            OnAchieveStateChangedEvent?.Invoke(this);
        }
    }
}