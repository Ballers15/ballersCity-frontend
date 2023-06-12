using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Meta.Quests {
    public class QuestInteractor : Interactor {

        protected const string QUESTS_FOLDER_PATH = "Quests";

        protected Dictionary<string, Quest> questsMap;


        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            QuestRepository questRepository = this.GetRepository<QuestRepository>();
            string[] stateJsons = questRepository.stateJsons;
            QuestInfo[] allQuestInfo = Resources.LoadAll<QuestInfo>(QUESTS_FOLDER_PATH);

            foreach (string stateJson in stateJsons) {
                QuestState state = JsonUtility.FromJson<QuestState>(stateJson);

                foreach (QuestInfo info in allQuestInfo) {
                    if (info.id == state.id) {
                        QuestState specialState = info.CreateState(stateJson);
                        Quest quest = new Quest(info, specialState);
                        questsMap.Add(info.id, quest);
                        break;
                    }
                }
            }
            Resources.UnloadUnusedAssets();
        }

        public Quest GetQuest(string questId) {
            return questsMap[questId];
        }

        public Quest[] GetActiveQuests() {
            List<Quest> activeQuests = new List<Quest>();
            foreach (Quest quest in questsMap.Values) {
                if (quest.isActive)
                    activeQuests.Add(quest);
            }
            return activeQuests.ToArray();
        }

        public void SaveAllQuests() {
            List<QuestState> states  = new List<QuestState>();
            foreach (Quest quest in questsMap.Values)
                states.Add(quest.state);

            QuestRepository questRepository = this.GetRepository<QuestRepository>();
            questRepository.Save(states.ToArray());
        }
    }
}