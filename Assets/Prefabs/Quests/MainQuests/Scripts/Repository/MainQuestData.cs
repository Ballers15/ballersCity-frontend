using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class MainQuestData : ICloneable<MainQuestData>
    {
        [SerializeField] 
        public int version;

        [SerializeField]
        public bool isUnlocked;

        public MainQuestData Clone()
        {
            return new MainQuestData
            {
                version = this.version,
                isUnlocked = this.isUnlocked
            };
        }
    }
}