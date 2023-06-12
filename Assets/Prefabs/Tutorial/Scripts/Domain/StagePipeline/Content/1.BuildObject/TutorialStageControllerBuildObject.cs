using SinSity.Core;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStageControllerBuildObject",
        menuName = "Domain/Tutorial/TutorialStageControllerBuildObject"
    )]
    public sealed class TutorialStageControllerBuildObject : TutorialStageController
    {
        public override void OnBeginListen()
        {
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
        }

        public override void OnContinueListen(string currentStageJson)
        {
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
        }

        public override void OnEndListen()
        {
            IdleObject.OnIdleObjectBuilt -= this.OnIdleObjectBuilt;
        }

        #region Event

        private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state)
        {
            this.NotifyAboutTriggered();
        }

        #endregion
    }
}