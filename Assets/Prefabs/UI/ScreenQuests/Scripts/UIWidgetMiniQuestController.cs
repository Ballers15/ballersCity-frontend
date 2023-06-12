using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.UI
{
    public sealed class UIWidgetMiniQuestController
    {
        private readonly UIWidgetMiniQuest[] widgetMiniQuests;

        private MiniQuestInteractor miniQuestInteractor;

        public UIWidgetMiniQuestController(UIWidgetMiniQuest[] miniQuests)
        {
            this.widgetMiniQuests = miniQuests;
        }

        public void OnEnable()
        {
            this.miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
            this.miniQuestInteractor.OnQuestChangedEvent += this.OnMiniQuestChanged;
        }

        public void OnDisable()
        {
            this.miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
            this.miniQuestInteractor.OnQuestChangedEvent -= this.OnMiniQuestChanged;
        }
        
        public void OnRefresh()
        {
            var activeQuests = this.miniQuestInteractor.GetActiveQuests();
            var activeQuestsCount = activeQuests.Count;
            for (var i = 0; i < activeQuestsCount; i++)
            {
                var widgetMiniQuest = this.widgetMiniQuests[i];
                var activeQuest = activeQuests[i];
                widgetMiniQuest.Setup(activeQuest);
            }
        }

        private void OnMiniQuestChanged(Quest quest)
        {
            this.OnRefresh();
        }
    }
}