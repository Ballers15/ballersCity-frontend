using System;
using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class MiniQuestStatistics
    {
        [SerializeField] 
        public List<MiniQuestEntity> entities = new List<MiniQuestEntity>();

        public DateTimeSerialized lastResetTimeSerialized = new DateTimeSerialized();

        public bool resetWasUsed = false;
    }
}