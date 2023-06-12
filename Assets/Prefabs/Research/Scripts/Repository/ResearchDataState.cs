using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class ResearchDataState : ICloneable<ResearchDataState>
    {
        [SerializeField]
        public int version;

        [SerializeField]
        public bool isUnlocked;

        public ResearchDataState Clone()
        {
            return new ResearchDataState
            {
                version = this.version,
                isUnlocked = this.isUnlocked
            };
        }
    }
}