using SinSity.Tools;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Tools;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorCollectSoftCurrency : QuestInspector
    {
        public QuestInspectorCollectSoftCurrency(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            IdleObject.OnIdleObjectCurrencyCollected += this.OnIdleObjectCurrencyCollected;
            this.CheckState();
        }

        public override void CheckState()
        {
            var questState = this.quest.GetState<QuestStateCollectSoftCurrency>();
            var collectedSoftCurrency = questState.collectedSoftCurrency;
            var needToCollectSoftCurrency = questState.needToCollectSoftCurrency;
            if (collectedSoftCurrency >= needToCollectSoftCurrency)
            {
                this.quest.Complete();
            }
        }

        private void OnIdleObjectCurrencyCollected(object sender, BigNumber collectedCurrencyFromObject)
        {
            var state = this.quest.GetState<QuestStateCollectSoftCurrency>();
            var collectedSoftCurrencyBefore = state.collectedSoftCurrency;
            var collectedSoftCurrencyAfter = collectedSoftCurrencyBefore + collectedCurrencyFromObject;
            state.SetCollectedSoftCurrency(collectedSoftCurrencyAfter);
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectCurrencyCollected -= this.OnIdleObjectCurrencyCollected;
        }

        protected override float GetProgressNormalized()
        {
            var questState = this.quest.GetState<QuestStateCollectSoftCurrency>();
            var collectedSoftCurrency = questState.collectedSoftCurrency;
            var needToCollectSoftCurrency = questState.needToCollectSoftCurrency;
            var percent = (float) BigNumber.DivideToDouble(collectedSoftCurrency, needToCollectSoftCurrency);
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var questState = this.quest.GetState<QuestStateCollectSoftCurrency>();
            var collectedSoftCurrency = questState.collectedSoftCurrency;
            var needToCollectSoftCurrency = questState.needToCollectSoftCurrency;
            if (collectedSoftCurrency > needToCollectSoftCurrency)
            {
                collectedSoftCurrency = needToCollectSoftCurrency;
            }

            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            var collectedTranslated = collectedSoftCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            var needToCollectTranslated = needToCollectSoftCurrency.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
            return $"{collectedTranslated}/{needToCollectTranslated}";
        }
    }
}