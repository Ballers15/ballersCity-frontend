using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "MainQuestsPipeline",
        menuName = "Meta/Quests/MainQuestsPipeline"
    )]
    public sealed class MainQuestsPipeline : QuestsPipeline
    {
        private Dictionary<string, int> questIdVsIndexMap;

        private void Awake()
        {
            this.questIdVsIndexMap = new Dictionary<string, int>();
            var questCount = this.quests.Length;
            for (var questIndex = 0; questIndex < questCount; questIndex++)
            {
                var questInfo = this.quests[questIndex];
                var questId = questInfo.id;
                this.questIdVsIndexMap[questId] = questIndex;
            }
        }

        public IEnumerable<QuestInfo> GetQuestInfos()
        {
            return this.quests.ToList();
        }

        public QuestInfo GetQuest(string id)
        {
            var questIndex = this.questIdVsIndexMap[id];
            return this.GetQuest(questIndex);
        }

        public bool HasQuest(string id)
        {
            return this.questIdVsIndexMap.ContainsKey(id);
        }

        public QuestInfo GetQuest(int index)
        {
            return this.quests[index];
        }

        public bool HasNextQuest(string id, out QuestInfo nextQuestInfo)
        {
            var questIndex = this.questIdVsIndexMap[id];
            return this.HasNextQuest(questIndex, out nextQuestInfo);
        }

        public bool HasNextQuest(int currentIndex, out QuestInfo nextQuestInfo)
        {
            var nextIndex = currentIndex + 1;
            if (nextIndex >= this.quests.Length)
            {
                nextQuestInfo = null;
                return false;
            }

            nextQuestInfo = this.GetQuest(nextIndex);
            return true;
        }
    }
}