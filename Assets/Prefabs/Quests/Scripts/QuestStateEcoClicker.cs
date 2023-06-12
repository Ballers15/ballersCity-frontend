using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests {
    public abstract class QuestStateEcoClicker : QuestState {
        protected QuestStateEcoClicker(string stateJson) : base(stateJson) { }
        protected QuestStateEcoClicker(QuestInfo info) : base(info) { }

        public abstract string GetDescription(Quest quest);
    }
}