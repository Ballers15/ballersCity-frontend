using System;
using System.Numerics;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "GemTreeObjectInfo",
        menuName = "Game/GemTree/New GemTreeObjectInfo"
    )]
    public sealed class GemTreeInfo : ScriptableObject
    {
        #region CONSTANTS

        private const float COEF = 1.09f;

        #endregion
        
        [SerializeField] 
        public TreeLevel[] treeLevels;

        [Serializable]
        public sealed class TreeLevel
        {
            [SerializeField]
            private BigNumber upgradePrice;
            
            [SerializeField] 
            public int maxProgress;

            [SerializeField] 
            public string[] openBranchesIdSet;

            public BigNumber GetPrice(int progress) {
                return this.upgradePrice * Math.Pow(COEF, progress);
            }

            public void SetPrice(BigNumber value)
            {
                this.upgradePrice = value;
            }

            public BigNumber GetBasePrice()
            {
                return this.upgradePrice;
            }
        }
    }
}