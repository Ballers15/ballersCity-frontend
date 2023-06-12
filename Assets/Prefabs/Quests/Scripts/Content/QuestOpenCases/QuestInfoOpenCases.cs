using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using SinSity.Quests.Meta;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    [CreateAssetMenu(
        fileName = "QuestInfoOpenCases",
        menuName = "Meta/Quests/QuestInfoOpenCases"
    )]
    public sealed class QuestInfoOpenCases : DailyQuestInfo
    {
        [SerializeField]
        private int m_open1CaseProb = 35;

        [SerializeField]
        private int m_open2CaseProb = 45;

        [SerializeField]
        private int m_open3CaseProb = 20;

        private readonly List<int> openCaseCountPool = new List<int>();

        public override QuestInspector CreateInspector(Quest quest)
        {
            return new QuestInspectorOpenCases(quest);
        }

        public override QuestState CreateState(string stateJson)
        {
            return new QuestStateOpenCases(stateJson);
        }

        public override QuestState CreateStateDefault()
        {
            this.m_open1CaseProb.Times(() => this.openCaseCountPool.Add(1));
            this.m_open2CaseProb.Times(() => this.openCaseCountPool.Add(2));
            this.m_open3CaseProb.Times(() => this.openCaseCountPool.Add(3));
            var needOpenCases = this.openCaseCountPool.GetRandom();
            var stateOpenCases = new QuestStateOpenCases(this)
            {
                needOpenCases = needOpenCases
            };
            return stateOpenCases;
        }

        public override string GetDescription()
        {
            return "Открыть любой кейс";
        }
    }
}