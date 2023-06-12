using SinSity.Domain;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    public sealed class QuestInspectorLaunchResearch : QuestInspector
    {
        private ResearchObjectTimerInteractor researchObjectTimerInteractor;

        private QuestStateLaunchResearch questState;

        private QuestInfoLaunchResearch questInfo;

        public QuestInspectorLaunchResearch(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            this.researchObjectTimerInteractor = Game.GetInteractor<ResearchObjectTimerInteractor>();
            this.researchObjectTimerInteractor.OnResearchObjectLaunchedEvent += this.OnResearchObjectLaunched;
            this.questState = this.quest.GetState<QuestStateLaunchResearch>();
            this.questInfo = this.quest.GetInfo<QuestInfoLaunchResearch>();
            this.CheckState();
        }

        public override void CheckState()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questInfo.requiredTimes;
            if (currentTimes >= requiredTimes)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            this.researchObjectTimerInteractor.OnResearchObjectLaunchedEvent -= this.OnResearchObjectLaunched;
        }

        protected override float GetProgressNormalized()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questInfo.requiredTimes;
            var percent = (float) currentTimes / requiredTimes;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questInfo.requiredTimes;
            if (currentTimes > requiredTimes)
            {
                currentTimes = requiredTimes;
            }

            return $"{currentTimes}/{requiredTimes}";
        }

        #region Event

        private void OnResearchObjectLaunched(ResearchObject obj)
        {
            this.questState.currentTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        #endregion
    }
}