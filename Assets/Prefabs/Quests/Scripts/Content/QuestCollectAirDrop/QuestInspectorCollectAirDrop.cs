using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    public sealed class QuestInspectorCollectAirDrop : QuestInspector
    {
        private AirShipController airShipController;

        private QuestStateCollectAirDrop questState;

        public QuestInspectorCollectAirDrop(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            var airDropInteractor = Game.GetInteractor<AirDropInteractor>();
            this.airShipController = airDropInteractor.airShipController;
            this.airShipController.OnAirDropRewardReceived += this.OnAirDropRewardReceived;
            this.questState = this.quest.GetState<QuestStateCollectAirDrop>();
            this.CheckState();
        }

        public override void CheckState()
        {
            var currentCollectTimes = this.questState.currentCollectTimes;
            var needToCollectTimes = questState.needToCollectTimes;
            if (currentCollectTimes >= needToCollectTimes)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            this.airShipController.OnAirDropRewardReceived -= this.OnAirDropRewardReceived;
        }

        protected override float GetProgressNormalized()
        {
            var currentCollectTimes = this.questState.currentCollectTimes;
            var needToCollectTimes = this.questState.needToCollectTimes;
            var percent = (float) currentCollectTimes / needToCollectTimes;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var currentCollectTimes = this.questState.currentCollectTimes;
            var needToCollectTimes = this.questState.needToCollectTimes;
            if (currentCollectTimes > needToCollectTimes)
            {
                currentCollectTimes = needToCollectTimes;
            }

            return $"{currentCollectTimes}/{needToCollectTimes}";
        }

        #region Event

        private void OnAirDropRewardReceived(AirShipBehaviour obj)
        {
            this.questState.currentCollectTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        #endregion
    }
}