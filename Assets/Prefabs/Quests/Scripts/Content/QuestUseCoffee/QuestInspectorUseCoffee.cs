using IdleClicker.Gameplay;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUseCoffee : QuestInspector
    {
        private CoffeeBoost coffeeBoost;

        private readonly QuestStateUseCoffee questState;

        #region Lifecycle

        public QuestInspectorUseCoffee(Quest quest) : base(quest)
        {
            this.questState = quest.GetState<QuestStateUseCoffee>();
        }

        protected override void SubscribeOnEvents()
        {
            var interactor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.coffeeBoost = interactor.coffeeBoost;
            this.coffeeBoost.OnStartedEvent += this.OnCoffeeBoostStarted;
            this.CheckState();
        }


        protected override void UnsubscribeFromEvents()
        {
            this.coffeeBoost.OnStartedEvent -= this.OnCoffeeBoostStarted;
        }

        #endregion

        public override void CheckState()
        {
            if (this.questState.currentTimes >= this.questState.requiredTimes)
            {
                this.quest.Complete();
            }
        }

        protected override float GetProgressNormalized()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questState.requiredTimes;
            var percent = (float) currentTimes / requiredTimes;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var currentTimes = this.questState.currentTimes;
            var requiredTimes = this.questState.requiredTimes;
            if (currentTimes > requiredTimes)
            {
                currentTimes = requiredTimes;
            }

            return $"{currentTimes}/{requiredTimes}";
        }

        #region InteractorCallbacks

        private void OnCoffeeBoostStarted(CoffeeBoost coffeeboost)
        {
            this.questState.currentTimes++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        #endregion
    }
}