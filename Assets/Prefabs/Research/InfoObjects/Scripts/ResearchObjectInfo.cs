using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    public abstract class ResearchObjectInfo : ScriptableObject
    {
        #region Const

        private const int SECONDS_IN_HOUR = 3600;

        #endregion

        [SerializeField]
        public string id;

        [SerializeField]
        private string titleCode;

        [SerializeField]
        private string descriptionCode;

        [SerializeField]
        public int durationSeconds;
        
        [SerializeField]
        public RewardInfoEcoClicker rewardInfo;

        public int durationHours
        {
            get { return this.durationSeconds / SECONDS_IN_HOUR; }
        }

        public abstract BigNumber LoadNextPrice();

        public string GetTitle()
        {
            var translation = Localization.GetTranslation(titleCode);
            return translation;
        }

        public string GetDescription()
        {
            var translatingLine = Localization.GetTranslation(descriptionCode);
            var finalLine = string.Format(translatingLine, durationHours);
            return finalLine;
        }

        public Sprite GetIcon()
        {
            return rewardInfo.GetSpriteIcon();
        }

        public string GetRewardCountToString()
        {
            return rewardInfo.GetCountToString();
        }
        
    }
}