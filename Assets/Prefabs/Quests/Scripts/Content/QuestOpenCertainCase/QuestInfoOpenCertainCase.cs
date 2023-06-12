using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    [CreateAssetMenu(
        fileName = "QuestInfoOpenCertainCase",
        menuName = "Meta/Quests/QuestInfoOpenCertainCase"
    )]
    public sealed class QuestInfoOpenCertainCase : DailyQuestInfo
    {
        [SerializeField]
        private string m_openSimpleCaseId;

        [SerializeField]
        private int m_openSimpleCaseProb = 70;

        [SerializeField]
        private int m_openSteelCaseProb = 30;

        [SerializeField]
        private string m_openSteelCaseId;

        [SerializeField]
        private string m_openGoldCaseId = "gold_case";

        [SerializeField]
        private int m_openGoldCaseProb = 0;

        private readonly List<string> openCaseCountPool = new List<string>();

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorOpenCertainCase(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateOpenCertainCase(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            this.m_openSimpleCaseProb.Times(() => this.openCaseCountPool.Add(this.m_openSimpleCaseId));
            this.m_openSteelCaseProb.Times(() => this.openCaseCountPool.Add(this.m_openSteelCaseId));
            this.m_openGoldCaseProb.Times(() => this.openCaseCountPool.Add(this.m_openGoldCaseId));
            var randomCaseId = this.openCaseCountPool.GetRandom();
            var state = new QuestStateOpenCertainCase(this)
            {
                needOpenCaseId = randomCaseId
            };
            return state;
        }

        public override string GetDescription()
        {
            return "Открыть кейс";
        }
    }
}