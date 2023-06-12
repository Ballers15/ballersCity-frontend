using System;
using System.Collections;
using SinSity.Core;
using SinSity.Meta;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class IdleObjectExperienceInteractor : AbstractExperienceInteractor
    {
        #region Const

        private const string EXPERIENCE_CONFIG_PATH = "IdleObjectExperienceConfig";

        #endregion

        #region Event

        public override event Action<object, Transform, ulong> OnVisualizeAddedExperienceEvent;

        #endregion

        private IdleObjectExperienceConfig config;
        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.config = Resources.Load<IdleObjectExperienceConfig>(EXPERIENCE_CONFIG_PATH);
        }

        public override void OnReady()
        {
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
            IdleObject.OnIdleObjectLevelStageRisen += this.OnIdleObjectImproved;
        }


        #region IdleObjectsEvents

        public void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state)
        {
            var experienceRange = this.config.experienceForBuild;
            this.AddExperience(this, experienceRange);
            this.OnVisualizeAddedExperienceEvent?.Invoke(this, idleobject.transform, experienceRange);
            ProfileLevelAnalytics.LogPlayerExpAdded(
                Game.GetInteractor<ProfileLevelInteractor>().currentLevel,
                "idle_object_built"
            );
        }

        private void OnIdleObjectImproved(IdleObject idleObject, LevelImprovementReward reward)
        {
            var experienceRange = this.config.experienceForImprovement;
            this.AddExperience(this, experienceRange);
            this.OnVisualizeAddedExperienceEvent?.Invoke(this, idleObject.transform, experienceRange);
            ProfileLevelAnalytics.LogPlayerExpAdded(
                Game.GetInteractor<ProfileLevelInteractor>().currentLevel,
                "idle_object_improved"
            );
        }

        #endregion
    }
}