using IdleClicker.Gameplay;
using SinSity.Core;
using SinSity.Meta.Quests;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests {
    [CreateAssetMenu(fileName = "QuestInfoCollectCar", menuName = "Meta/Quests/QuestInfoCollectCar")]
    public class QuestInfoCollectCar : DailyQuestInfo {
        [SerializeField] private int minTimes;
        [SerializeField] private int maxTimes;

        public override QuestInspector CreateInspector(Quest quest) {
            return new QuestInspectorCollectCar(quest);
        }

        public override QuestState CreateState(string stateJson) {
            return new QuestStateCollectCar(stateJson);
        }

        public override QuestState CreateStateDefault() {
            var questState = new QuestStateCollectCar(this) {
                requiredTimes = new System.Random().Next(this.minTimes, this.maxTimes + 1)
            };
            return questState;
        }

        public override string GetDescription() {
            return "Поймать машину";
        }
    }
}