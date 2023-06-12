using SinSity.Domain;
using SinSity.Monetization;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUseTimeBooster : QuestInspector
    {
        private TimeBoosterActivationInteractor timeBoosterActivationInteractor;

        public QuestInspectorUseTimeBooster(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            this.timeBoosterActivationInteractor = Game.GetInteractor<TimeBoosterActivationInteractor>();
            this.timeBoosterActivationInteractor.OnTimeBoosterActivatedEvent += this.OnTimeBoosterUsed;
            this.CheckState();
        }

        private void OnTimeBoosterUsed(ProductInfoTimeBooster obj)
        {
            var questState = this.quest.GetState<QuestStateUseTimeBooster>();
            questState.currentTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {
            this.timeBoosterActivationInteractor.OnTimeBoosterActivatedEvent -= this.OnTimeBoosterUsed;
        }

        protected override float GetProgressNormalized()
        {
            var state = this.quest.GetState<QuestStateUseTimeBooster>();
            var currentTimes = (float) state.currentTimes;
            var requieredTimes = (float) state.requiredTimes;
            return currentTimes / requieredTimes;
        }

        protected override string GetProgressDescription()
        {
            var state = this.quest.GetState<QuestStateUseTimeBooster>();
            var currentTimes = state.currentTimes;
            var requieredTimes = state.requiredTimes;
            return $"{currentTimes}/{requieredTimes}";
        }

        public override void CheckState()
        {
            var questState = this.quest.GetState<QuestStateUseTimeBooster>();
            if (questState.currentTimes >= questState.requiredTimes)
            {
                this.quest.Complete();
            }
        }
    }
}