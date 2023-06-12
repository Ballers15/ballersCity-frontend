using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUseEcoBoost : QuestInspector
    {
        private EcoBoostInteractor ecoBoostInteractor;

        private QuestStateUseEcoBoost questState;

        public QuestInspectorUseEcoBoost(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            this.ecoBoostInteractor = Game.GetInteractor<EcoBoostInteractor>();
            this.ecoBoostInteractor.OnEcoBoostLaunchedEvent += this.OnEcoBoostLaunched;
            this.questState = this.quest.GetState<QuestStateUseEcoBoost>();
            this.CheckState();
        }

        private void OnEcoBoostLaunched()
        {
            this.questState.currentTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {
            this.ecoBoostInteractor.OnEcoBoostLaunchedEvent -= this.OnEcoBoostLaunched;
        }

        protected override float GetProgressNormalized()
        {
            var currentTimes = (float) this.questState.currentTimes;
            var requiredTimes = (float) this.questState.requiredTimes;
            return currentTimes / requiredTimes;
        }

        protected override string GetProgressDescription()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questState.requiredTimes;
            return $"{currentTimes}/{requiredTimes}";
        }

        public override void CheckState()
        {
            if (this.questState.currentTimes >= this.questState.requiredTimes)
            {
                this.quest.Complete();
            }
        }
    }
}