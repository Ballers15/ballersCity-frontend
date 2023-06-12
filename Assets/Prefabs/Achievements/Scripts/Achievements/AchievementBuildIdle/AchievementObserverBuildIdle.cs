using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Achievements;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Achievements
{
    public class AchievementObserverBuildIdle : AchievementObserver
    {
        private IdleObjectsInteractor idleInteractor;
        public AchievementObserverBuildIdle(AchievementEntity achievement) : base(achievement)
        {
            
        }

        protected override void Initialize()
        {
            idleInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            CheckState();
        }

        protected override void SubscribeOnEvents()
        {
            IdleObject.OnIdleObjectBuilt += OnOnIdleObjectBuilt;
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectBuilt -= OnOnIdleObjectBuilt;
        }
        
        private void OnOnIdleObjectBuilt(IdleObject idleobject, IdleObjectState newstate)
        {
            
            var achieveState = this.achievement.GetState<AchievementStateBuildIdle>();
            if (!achieveState.isCountType())
            {
                if (idleobject.id != achieveState.needToBuildIdleId) return;
                AchievementCompleteAndSave();
            }
            else
            {
               if(isBuildedEnough()) AchievementCompleteAndSave();
            }
        }

        protected override void CheckState()
        {
             var achieveState = this.achievement.GetState<AchievementStateBuildIdle>();
             if (!achieveState.isCountType())
             {
                 var idleObj = idleInteractor.GetIdleObject(achieveState.needToBuildIdleId);
                 if (!idleObj.isBuilt) return;
                 AchievementCompleteAndSave();
             }
             else
             {
                 if(isBuildedEnough()) AchievementCompleteAndSave();
             }
        }

         private bool isBuildedEnough()
         {
             var achieveState = this.achievement.GetState<AchievementStateBuildIdle>();
             var idleList = idleInteractor.GetBuildedIdleObjects();

             return (idleList.Length >= achieveState.needToBuildCount);
         }
    }
}

