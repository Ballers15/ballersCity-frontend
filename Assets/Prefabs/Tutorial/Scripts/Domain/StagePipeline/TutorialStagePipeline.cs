using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "TutorialStagePipeline",
        menuName = "Domain/Tutorial/New TutorialStagePipeline"
    )]
    public sealed class TutorialStagePipeline : ScriptableObject
    {
        [SerializeField]
        private Params m_params = new Params();
        
        private List<TutorialStageController> stageControllers;
        
        private Dictionary<string, int> stageIdVsIndexMap;

        private void Awake()
        {
            this.stageIdVsIndexMap = new Dictionary<string, int>();
            this.stageControllers = this.m_params.m_tutorialStageControllerAssets
                .Select(Instantiate)
                .ToList();
            var stageCount = this.stageControllers.Count;
            for (var stageIndex = 0; stageIndex < stageCount; stageIndex++)
            {
                var stageController = this.stageControllers[stageIndex];
                var stageId = stageController.id;
                this.stageIdVsIndexMap[stageId] = stageIndex;
            }
        }

        public IEnumerable<TutorialStageController> GetStageControllers()
        {
            return this.stageControllers.ToList();
        }

        public TutorialStageController GetStageController(string id)
        {
            var stageIndex = this.stageIdVsIndexMap[id];
            return this.GetStageController(stageIndex);
        }

        public TutorialStageController GetStageController(int index)
        {
            return this.stageControllers[index];
        }

        public bool HasNextStageController(string id, out TutorialStageController nextStageController)
        {
            var stageIndex = this.stageIdVsIndexMap[id];
            return this.HasNextStageController(stageIndex, out nextStageController);
        }

        public bool HasNextStageController(int currentIndex, out TutorialStageController nextStageController)
        {
            var nextIndex = currentIndex + 1;
            if (nextIndex >= this.stageControllers.Count)
            {
                nextStageController = null;
                return false;
            }

            nextStageController = this.GetStageController(nextIndex);
            return true;
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public TutorialStageController[] m_tutorialStageControllerAssets;
        }
    }
}