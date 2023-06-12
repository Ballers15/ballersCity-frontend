using SinSity.Core;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Monetization;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorOpenCertainCase : QuestInspector
    {
        public QuestInspectorOpenCertainCase(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            ProductInfoCase.OnProductCaseOpenedEvent += this.OnCaseOpened;
        }

        private void OnCaseOpened(ProductInfoCase productInfoCase)
        {
            var caseId = productInfoCase.GetId();
            var state = this.quest.GetState<QuestStateOpenCertainCase>();
            if (state.needOpenCaseId == caseId)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            ProductInfoCase.OnProductCaseOpenedEvent -= this.OnCaseOpened;
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