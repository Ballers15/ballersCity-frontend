using System;
using System.Linq;
using UnityEngine;

namespace SinSity.Meta
{
    [Serializable]
    [CreateAssetMenu(fileName = "IdleObjectImprovementsPipeline", menuName = "IdleObject/ImprovementsPipeline")]
    public sealed class LevelImprovementsPipeline : ScriptableObject
    {
        #region Const

        private const string RESOURCES_NAME = "IdleObjectImprovementsPipeline";

        #endregion

        public LevelImprovementBlock[] improvementBlocks;
        
        public static LevelImprovementsPipeline Load()
        {
            return Resources.Load<LevelImprovementsPipeline>(RESOURCES_NAME);
        }
        
        public LevelImprovementBlock GetBlockByLevel(int level)
        {
            foreach (var levelImprovementBlock in this.improvementBlocks)
            {
                if (level >= levelImprovementBlock.lastLevel)
                {
                    continue;
                }

                return levelImprovementBlock;
            }

            return this.improvementBlocks.LastOrDefault();
        }
    }
}