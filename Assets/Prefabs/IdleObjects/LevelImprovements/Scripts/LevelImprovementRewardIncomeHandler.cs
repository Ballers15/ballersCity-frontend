using System.Collections;
using SinSity.Core;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Meta {
    public class LevelImprovementRewardIncomeHandler : RewardHandler {

        public LevelImprovementRewardIncomeHandler(LevelImprovementReward reward) : base(reward) { }

        public override void ApplyReward(object sender, bool instantly) {
            LevelImprovementReward levelImprovementReward = reward as LevelImprovementReward;
            IdleObject idleObject = levelImprovementReward.idleObject;
            LevelImprovementRewardIncomeInfo info = levelImprovementReward.info as LevelImprovementRewardIncomeInfo;;
            idleObject.IncreaseIncome(info.incomeBoost);

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
                if (!uiController.IsActiveUIElement<UIPanelNotificationLevelRaiseIncome>()) {
                    UIPanelNotificationLevelRaiseIncome panel = uiInteractor.ShowElement<UIPanelNotificationLevelRaiseIncome>();
                    panel.Setup(idleObject);
                    break;
                }
            }
        }
    }
}