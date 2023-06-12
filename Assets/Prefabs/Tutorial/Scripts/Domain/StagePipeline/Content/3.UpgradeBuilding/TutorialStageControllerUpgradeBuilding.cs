using SinSity.Core;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStageControllerUpgradeBuilding",
        menuName = "Domain/Tutorial/TutorialStageControllerUpgradeBuilding"
    )]
    public sealed class TutorialStageControllerUpgradeBuilding : TutorialStageController
    {
        public override void OnBeginListen()
        {
            IdleObject.OnIdleObjectLevelRisen += this.OnIdleObjectLevelRisen;
        }

        private void OnIdleObjectLevelRisen(IdleObject idleObject, int newLevel, bool arg3)
        {
            if (newLevel >= 3)
            {
                this.NotifyAboutTriggered();
            }
        }

        public override void OnContinueListen(string currentStageJson)
        {
            IdleObject.OnIdleObjectLevelRisen += this.OnIdleObjectLevelRisen;
        }

        public override void OnEndListen()
        {
            IdleObject.OnIdleObjectLevelRisen -= this.OnIdleObjectLevelRisen;
        }
    }
}