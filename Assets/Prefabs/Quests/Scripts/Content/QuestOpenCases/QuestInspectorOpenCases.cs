using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Tools;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorOpenCases : QuestInspector
    {
        public QuestInspectorOpenCases(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            ProductInfoCase.OnProductCaseOpenedEvent += this.OnCaseOpened;
            this.CheckState();
        }

        public override void CheckState()
        {
            var state = this.quest.GetState<QuestStateOpenCases>();
            var openedCases = state.openedCases;
            var needOpenCases = state.needOpenCases;
            if (openedCases >= needOpenCases)
            {
                this.quest.Complete();
            }
        }

        private void OnCaseOpened(ProductInfoCase productInfoCase)
        {
            var state = this.quest.GetState<QuestStateOpenCases>();
            state.openedCases++;
            this.quest.NotifyQuestStateChanged();
            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {
            ProductInfoCase.OnProductCaseOpenedEvent -= this.OnCaseOpened;
        }

        protected override float GetProgressNormalized()
        {
            var state = this.quest.GetState<QuestStateOpenCases>();
            var openedCases = (float) state.openedCases;
            var needOpenCases = (float) state.needOpenCases;
            var percent = openedCases / needOpenCases;
            return Mathf.Min(percent, 1);
        }

        protected override string GetProgressDescription()
        {
            var state = this.quest.GetState<QuestStateOpenCases>();
            var openedCases = (float) state.openedCases;
            var needOpenCases = (float) state.needOpenCases;
            openedCases = Mathf.Min(openedCases, needOpenCases);
            return $"{openedCases}/{needOpenCases}";
        }
    }
}