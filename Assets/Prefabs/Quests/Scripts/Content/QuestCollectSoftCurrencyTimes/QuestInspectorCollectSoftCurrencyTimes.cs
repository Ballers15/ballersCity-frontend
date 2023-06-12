using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Tools;

namespace SinSity.Meta.Quests
{
    public sealed class QuestInspectorCollectSoftCurrencyTimes : QuestInspector
    {
        private readonly QuestInfoCollectSoftCurrencyTimes info;

        private readonly QuestStateCollectSoftCurrencyTimes state;

        public QuestInspectorCollectSoftCurrencyTimes(Quest quest) : base(quest)
        {
            this.info = quest.info as QuestInfoCollectSoftCurrencyTimes;
            this.state = quest.state as QuestStateCollectSoftCurrencyTimes;
        }

        protected override void SubscribeOnEvents()
        {
            IdleObject.OnIdleObjectCurrencyCollected += OnIdleObjectCurrencyCollected;
            this.CheckState();
        }

        private void OnIdleObjectCurrencyCollected(object sender, BigNumber collectedcurrency)
        {
            this.state.AddTime();
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        public override void CheckState()
        {
            if (this.state.timesCollected >= this.info.needToCollectTimes)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectCurrencyCollected -= OnIdleObjectCurrencyCollected;
        }

        protected override float GetProgressNormalized()
        {
            var timesCollected = (float) state.timesCollected;
            var infoNeedToCollectTimes = (float) info.needToCollectTimes;
            var percent = timesCollected / infoNeedToCollectTimes;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var timesCollected = this.state.timesCollected;
            var collectTimes = this.info.needToCollectTimes;
            var clampedValueCount = Mathf.Min(timesCollected, collectTimes);
            return $"{clampedValueCount}/{collectTimes}";
        }
    }
}