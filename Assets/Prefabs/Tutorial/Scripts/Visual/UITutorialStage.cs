using System;
using Orego.Util;
using SinSity.Domain;
using UnityEngine;

namespace SinSity.Core
{
    public abstract class UITutorialStage : MonoBehaviour
    {
        #region Event

        public AutoEvent OnStageFinishedEvent { get; } = new AutoEvent();

        #endregion

        public abstract Type GetRequiredControllerType();

        public abstract void SetController(TutorialStageController tutorialStageController);

        public virtual void OnStageFinished()
        {
            this.OnStageFinishedEvent?.Invoke();
        }

        private void OnDestroy()
        {
            this.OnStageFinishedEvent.Dispose();
        }
    }
}