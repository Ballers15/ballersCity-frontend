using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class MainQuestStatisitcs : ICloneable<MainQuestStatisitcs>
    {
        [SerializeField] 
        public string currentQuestId;
        
        [SerializeField]
        public string currentQuestJson;

        [SerializeField]
        public bool isAllQuestsCompleted;


        public MainQuestStatisitcs Clone()
        {
            return new MainQuestStatisitcs
            {
                currentQuestId = this.currentQuestId,
                currentQuestJson = this.currentQuestJson,
                isAllQuestsCompleted = this.isAllQuestsCompleted
            };
        }
    }
}