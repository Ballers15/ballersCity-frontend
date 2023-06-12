using SinSity.Meta.Quests;
using VavilichevGD.Audio;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPanelNotificationQuestComplete : UIPanelNotification<UIPanelNotificationQuestCompleteProperties> {
        
        protected override void OnClick() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            uiInteractor.ShowElement<UIScreenQuests>();
            base.OnClick();
        }

        public void Setup(Quest quest) {
            QuestStateEcoClicker state = quest.GetState<QuestStateEcoClicker>();
            properties.textQuestTitle.text = state.GetDescription(quest);
            SFX.PlaySFX(this.properties.audioClipShow);
        }
    }
}