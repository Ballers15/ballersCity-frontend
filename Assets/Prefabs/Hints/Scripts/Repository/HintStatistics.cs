using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class HintStatistics
    {
        [SerializeField]
        public List<string> hintJsonKeys;

        [SerializeField]
        public List<string> hintJsonValues;

        public HintStatistics Clone()
        {
            return new HintStatistics
            {
                hintJsonKeys = this.hintJsonKeys?.ToList(),
                hintJsonValues = this.hintJsonValues?.ToList()
            };
        }
    }
}