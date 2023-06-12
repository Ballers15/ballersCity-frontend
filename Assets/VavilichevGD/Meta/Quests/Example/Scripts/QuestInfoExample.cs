using UnityEngine;

namespace VavilichevGD.Meta.Quests.Examples {
    [CreateAssetMenu(fileName = "QuestInfoExample", menuName = "Meta/Quests/Example")]
    public class QuestInfoExample : QuestInfo {

        public override QuestInspector CreateInspector(Quest quest) {
            return new QuestInspectorExample(quest);
        }

        public override QuestState CreateState(string stateJson) {
            return new QuestStateExample(stateJson);
        }

        public override QuestState CreateStateDefault() {
            return new QuestStateExample(this);
        }
    }
}