using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoUseCertainTimeBooster",
        menuName = "Meta/Quests/QuestInfoUseCertainTimeBooster"
    )]
    public sealed class QuestInfoUseCertainTimeBooster : DailyQuestInfo
    {
        [SerializeField]
        private string m_useBooster4Id;

        [SerializeField]
        private int m_useBooster4 = 35;

        [SerializeField]
        private string m_useBooster12Id;

        [SerializeField]
        private int m_useBooster12 = 45;

        private readonly List<string> useBoosterPool = new List<string>();

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorUseCertainTimeBooster(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateUseCertainTimeBooster(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            this.m_useBooster4.Times(() => this.useBoosterPool.Add(this.m_useBooster4Id));
            this.m_useBooster12.Times(() => this.useBoosterPool.Add(this.m_useBooster12Id));
            var randomTimeBoosterId = this.useBoosterPool.GetRandom();
            var questState = new QuestStateUseCertainTimeBooster(this)
            {
                needUseCertainBoosterId = randomTimeBoosterId
            };
            return questState;
        }

        public override string GetDescription()
        {
            return "Использовать тайм-бустер";
        }
    }
}