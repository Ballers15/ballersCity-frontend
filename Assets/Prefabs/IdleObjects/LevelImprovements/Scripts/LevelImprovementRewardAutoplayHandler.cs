using System.Collections;
using SinSity.Core;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Meta
{
    public sealed class LevelImprovementRewardAutoplayHandler : RewardHandler
    {
        private readonly LevelImprovementReward levelImprovementReward;

        public LevelImprovementRewardAutoplayHandler(LevelImprovementReward reward) : base(reward)
        {
            this.levelImprovementReward = reward;
        }

        public override void ApplyReward(object sender, bool instantly) {
            IdleObject idleObject = levelImprovementReward.idleObject;
            idleObject.SetActiveAutoPlay(true);
            
           TryToShowNotification(idleObject);
        }
        
        private void TryToShowNotification(IdleObject idleObject) {
            Coroutines.StartRoutine(WaitForShowing(idleObject));
        }

        private IEnumerator WaitForShowing(IdleObject idleObject) {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIController uiController = uiInteractor.uiController;
            while (true) {
                yield return new WaitForSecondsRealtime(0.5f);
                if (!uiController.IsActiveUIElement<UIPanelNotificationLevelRaiseAutoPlay>()) {
                    UIPanelNotificationLevelRaiseAutoPlay panel = uiInteractor.ShowElement<UIPanelNotificationLevelRaiseAutoPlay>();
                    panel.Setup(idleObject);
                    break;
                }
            }
        }
    }
}