using SinSity.Domain;
using SinSity.Monetization;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Monetization;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUseCertainTimeBooster : QuestInspector
    {
        private TimeBoosterActivationInteractor timeBoosterActivationInteractor;

        public QuestInspectorUseCertainTimeBooster(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            this.timeBoosterActivationInteractor = Game.GetInteractor<TimeBoosterActivationInteractor>();
            this.timeBoosterActivationInteractor.OnTimeBoosterActivatedEvent += this.OnTimeBoosterActivated;
        }

        private void OnTimeBoosterActivated(ProductInfoTimeBooster productInfoTimeBooster)
        {
            var timeBoosterId = productInfoTimeBooster.GetId();
            var questState = this.quest.GetState<QuestStateUseCertainTimeBooster>();
            if (questState.needUseCertainBoosterId == timeBoosterId)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            this.timeBoosterActivationInteractor.OnTimeBoosterActivatedEvent -= this.OnTimeBoosterActivated;
        }

        protected override float GetProgressNormalized()
        {
            return this.quest.isCompleted
                ? 1
                : 0;
        }

        protected override string GetProgressDescription()
        {
            var progress = this.quest.isCompleted
                ? "1/1"
                : "0/1";
            return $"{progress}";
        }

        public override void CheckState()
        {
            
        }
    }
}