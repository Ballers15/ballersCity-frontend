using SinSity.Core;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStageControllerBuildTractor",
        menuName = "Domain/Tutorial/TutorialStageControllerBuildTractor"
    )]
    public sealed class TutorialStageControllerBuildTractor : TutorialStageController
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

        private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state)
        {
            if (idleobject.id == "io_2")
            {
                this.NotifyAboutTriggered();
            }
        }
    }
}