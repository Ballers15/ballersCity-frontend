using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class MiniQuestData : ICloneable<MiniQuestData>
    {
        [SerializeField]
        public int version;

        [SerializeField]
        public bool isUnlocked;

        public MiniQuestData Clone()
        {
            return new MiniQuestData
            {
                version = this.version,
                isUnlocked = this.isUnlocked
            };
        }
    }
}