using SinSity.Domain;
using UnityEngine;

namespace SinSity.Core
{
    public abstract class ProfileLevelUnlockWithActivityConfig : ProfileLevelUnlockConfig
    {
        [Space]
        [Header("Reward")]
        [SerializeField]
        private Sprite activitySpriteIcon;

        [SerializeField]
        private string activityTitleCode;

        public sealed override LevelUp GenerateLevelUp(int reachLevel)
        {
            var levelUp = new LevelUpWithActivity
            {
                level = reachLevel,
                rewards = this.GenerateRewards(),
                activitySpriteIcon = this.activitySpriteIcon,
                activityTitleCode = this.activityTitleCode
            };
            return levelUp;
        }
    }
}