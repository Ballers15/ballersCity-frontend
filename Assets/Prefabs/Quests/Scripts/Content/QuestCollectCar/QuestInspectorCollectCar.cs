using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.UI;

namespace SinSity.Meta.Quests {
    public class QuestInspectorCollectCar : QuestInspector {
        private UIControllerQuestCollectCar uiController;
        private readonly QuestStateCollectCar questState;
        
        public QuestInspectorCollectCar(Quest quest) : base(quest) {
            questState = quest.GetState<QuestStateCollectCar>();
        }
        
        protected override void SubscribeOnEvents() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            uiController = uiInteractor.GetUIController<UIControllerQuestCollectCar>();
            uiController.OnCarCollected += UpdateQuestState;
            uiController.Activate();
            CheckState();
        }

        private void UpdateQuestState() {
            questState.currentTimes++;
            quest.NotifyQuestStateChanged();
            CheckState();
        }

        protected override void UnsubscribeFromEvents() {
            uiController.OnCarCollected -= UpdateQuestState;
            uiController.Deactivate();
        }
        
        public override void CheckState() {
            if (questState.currentTimes >= this.questState.requiredTimes) {
                quest.Complete();
            }
        }

        protected override float GetProgressNormalized() {
            var currentTimes = questState.currentTimes;
            var requiredTimes = questState.requiredTimes;
            var percent = (float) currentTimes / requiredTimes;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription() {
            var currentTimes = questState.currentTimes;
            var requiredTimes = questState.requiredTimes;
            if (currentTimes > requiredTimes) {
                currentTimes = requiredTimes;
            }

            return $"{currentTimes}/{requiredTimes}";
        }
    }
}