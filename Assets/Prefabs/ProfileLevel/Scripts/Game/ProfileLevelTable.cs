using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ProfileLevelTable",
        menuName = "Game/ProfileLevel/ProfileLevelTable"
    )]
    public sealed class ProfileLevelTable : ScriptableObject
    {
        [SerializeField]
        private ulong[] relativeExperienceStages;

        private List<ulong> absExperienceStages;

        public int maxLevelNumber => absExperienceStages.Count;
        public int maxLevelIndex => absExperienceStages.Count - 1;

        private void Awake()
        {
            this.absExperienceStages = new List<ulong> {0};
            var length = this.relativeExperienceStages.Length;
            for (var i = 1; i < length; i++)
            {
                var previousExperience = this.absExperienceStages[i - 1];
                var stageExperice = this.relativeExperienceStages[i];
                var levelExperience = previousExperience + stageExperice;
                this.absExperienceStages.Add(levelExperience);
            }
        }

        public int GetCurrentLevel(ulong absExperience)
        {
            var length = this.absExperienceStages.Count;
            for (var level = 0; level < length; level++)
            {
                var experience = this.absExperienceStages[level];
                if (absExperience < experience)
                {
                    return level;
                }
            }

            var maxLevel = length;
            return maxLevel;
        }

        public ulong GetRequiredAbsExperience(int level)
        {
            ulong requiredAbsExperience;
            if (level < this.absExperienceStages.Count)
            {
                requiredAbsExperience = this.absExperienceStages[level];
            }
            else
            {
                requiredAbsExperience = this.absExperienceStages[level - 1];
            }

            return requiredAbsExperience;
        }

        public ulong GetRequiredRelativeExperience(int level)
        {
            ulong requiredRelativeExperience;
            if (level < this.absExperienceStages.Count)
            {
                requiredRelativeExperience = this.relativeExperienceStages[level];
            }
            else
            {
                requiredRelativeExperience = this.relativeExperienceStages[level - 1];
            }

            return requiredRelativeExperience;
        }

        public ulong GetCurrentRelativeExperience(int currentLevel, ulong currentAbsExperience)
        {
            if (currentLevel <= 1)
            {
                return currentAbsExperience;
            }

            return currentAbsExperience - this.absExperienceStages[currentLevel - 1];
        }
    }
}