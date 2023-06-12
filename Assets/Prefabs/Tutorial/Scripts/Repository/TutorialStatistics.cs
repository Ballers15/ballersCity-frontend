using System;
using UnityEngine;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class TutorialStatistics
    {
        [SerializeField]
        public bool isCompleted;

        [SerializeField]
        public string currentStageId;

        [SerializeField]
        public string currentStageJson;

        public TutorialStatistics Clone()
        {
            return new TutorialStatistics
            {
                isCompleted = this.isCompleted,
                currentStageId = this.currentStageId,
                currentStageJson = this.currentStageJson
            };
        }
    }
}