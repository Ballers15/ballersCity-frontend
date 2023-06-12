using Orego.Util;
using SinSity.Domain;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupNewLevel : UIPopupAnim<UIPopupNewLevelProperties, UIPopupArgs>
    {
        #region CONSTANTS

        private const string PREF_NAME_WIDGET_REWARD_CASE = "UIWidgetLevelReward_Case";
        private const string PREF_NAME_WIDGET_REWARD_GEMS = "UIWidgetLevelReward_Gems";
        private const string PREF_NAME_WIDGET_REWARD_CLEAN_ENERGY = "UIWidgetLevelReward_CleanEnergy";
        private const string PREF_NAME_WIDGET_REWARD_TIME_BOOSTER = "UIWidgetLevelReward_TimeBooster";
        private const string PREF_NAME_WIDGET_REWARD_PERS_CARD = "UIWidgetLevelReward_PersonageCard";
        private const string PATH_PREFIX = "UIWidgetsLevelReward/";
        
        private static readonly int boolExpand = Animator.StringToHash("expand");

        #endregion

        private bool isRewardReceived;

        private LevelUp levelUp;

        public override void Initialize() {
            base.Initialize();
            this.HideInstantly();
        }
        
        public override void Show()
        {
            if (preCached)
            {
                transform.SetAsLastSibling();
                gameObject.SetActive(true);
//                canvas.enabled = true;
            }
        }

        #region Setup

        public void Setup(LevelUp levelUp)
        {
            this.levelUp = levelUp;
            this.isRewardReceived = false;
            this.SetLevelNumber(levelUp.level);
            this.SetRewards(levelUp);
            if (levelUp is LevelUpWithActivity levelUpWithActivity)
            {
                this.SetActionReward(levelUpWithActivity);
                this.SetBoolTrue(boolExpand);
            }
        }
        
        private void SetLevelNumber(int newLevelNumber)
        {
            this.properties.SetLevelNumber(newLevelNumber);
        }

        private void SetRewards(LevelUp levelUp)
        {
            this.properties.CleanRewards();

            var rewardRewards = levelUp.rewards;
            foreach (Reward reward in rewardRewards)
            {
                UIWidgetLevelReward prefWidget = this.GetWidgetPrefab(reward);
                this.properties.AddReward(prefWidget, reward);
            }
        }

        private UIWidgetLevelReward GetWidgetPrefab(Reward reward)
        {
            var path = $"{PATH_PREFIX}";
            var rewardInfo = reward.info;
            if (rewardInfo is RewardInfoSoftCurrency)
                path += $"{PREF_NAME_WIDGET_REWARD_CLEAN_ENERGY}";
            else if (rewardInfo is RewardInfoHardCurrency || rewardInfo is RewardInfoSetupHardCurrency)
                path += $"{PREF_NAME_WIDGET_REWARD_GEMS}";
            else if (rewardInfo is RewardInfoCase)
                path += $"{PREF_NAME_WIDGET_REWARD_CASE}";
            else if (rewardInfo is RewardInfoTimeBooster)
                path += $"{PREF_NAME_WIDGET_REWARD_TIME_BOOSTER}";
            else if (rewardInfo is RewardInfoCard || rewardInfo is RewardInfoSetupCard)
                path += $"{PREF_NAME_WIDGET_REWARD_PERS_CARD}";
            return Resources.Load<UIWidgetLevelReward>(path);
        }

        private void SetActionReward(LevelUpWithActivity levelUp)
        {
            var activityTitleCode = Localization.GetTranslation(levelUp.activityTitleCode);
            var activitySpriteIcon = levelUp.activitySpriteIcon;
            this.properties.SetActionRewardInfo(activityTitleCode, activitySpriteIcon);
        }

        #endregion

        private void OnEnable()
        {
            this.properties.btnApply.AddListener(OnApplyBtnClick);
        }

        private void OnDisable()
        {
            this.properties.btnApply.RemoveListener(OnApplyBtnClick);
            this.SetBoolFalse(boolExpand);
        }

        #region Events

        private void OnApplyBtnClick()
        {
            SFX.PlaySFX(this.properties.sfxApply);
            this.properties.ApplyRewards();
            this.isRewardReceived = true;
            this.Hide();
        }

        #endregion

        private void Handle_NotifyAboutUIStateChanged() {
            this.NotifyAboutStateChanged(ACTIVATED);
        }
    }
}