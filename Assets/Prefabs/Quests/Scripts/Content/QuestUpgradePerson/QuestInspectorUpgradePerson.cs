using SinSity.Core;
using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUpgradePerson : QuestInspector
    {
        //private CardObjectUpgradeInteractor upgradeCardInteractor;

        public QuestInspectorUpgradePerson(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {

        }

        protected override float GetProgressNormalized()
        {
            var state = this.quest.GetState<QuestStateUpgradePerson>();
            var currentTimes = (float) state.currentTimes;
            var requiredTimes = (float) state.requiredTimes;
            return currentTimes / requiredTimes;
        }

        protected override string GetProgressDescription()
        {
            var state = this.quest.GetState<QuestStateUpgradePerson>();
            return $"{state.currentTimes}/{state.requiredTimes}";
        }

        public override void CheckState()
        {
            this.quest.Complete();
            /*var questState = this.quest.GetState<QuestStateUpgradePerson>();
            if (questState.currentTimes >= questState.requiredTimes)
            {
                this.quest.Complete();
            }*/
        }
    }
}