using System;
using UnityEngine;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class RenovationStatistics
    {
        [SerializeField] 
        public long level;

        [SerializeField] 
        public long passedQuestCount;

        [SerializeField] 
        public long targetQuestCount;
    }
}