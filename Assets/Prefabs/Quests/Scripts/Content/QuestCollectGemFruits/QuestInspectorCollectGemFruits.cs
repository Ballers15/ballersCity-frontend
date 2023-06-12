using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorCollectGemFruits : QuestInspector
    {
        private GemTreeTimerInteractor gemTreeTimerInteractor;

        private readonly QuestStateCollectGemFruits questState;

        private readonly QuestInfoCollectGemFruits questInfo;

        public QuestInspectorCollectGemFruits(Quest quest) : base(quest)
        {
            this.questState = quest.GetState<QuestStateCollectGemFruits>();
            this.questInfo = quest.GetInfo<QuestInfoCollectGemFruits>();
        }

        protected override void SubscribeOnEvents()
        {
            this.gemTreeTimerInteractor = Game.GetInteractor<GemTreeTimerInteractor>();
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent += this.OnGemReceived;
            this.CheckState();
        }

        private void OnGemReceived(object sender, GemBranchObject gemBranch, GemBranchReward arg3)
        {
            this.questState.currentTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        public override void CheckState()
        {
            if (this.questState.currentTimes >= this.questInfo.requiredTimes)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent -= this.OnGemReceived;
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
    }
}