using System;
using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI {
    public abstract class UIWidgetLevelReward : UIWidget<UIWidgetLevelRewardProperties> {

        private Reward rewardCurrent;

        public void Setup(Reward reward) {
            this.rewardCurrent = reward;
            
            this.SetupCountText(reward);
            this.SetupIcon(reward);
        }

        private void SetupCountText(Reward reward) {
            RewardInfoEcoClicker rewardInfo = reward.GetRewardInfo<RewardInfoEcoClicker>();
            string rewardCount = rewardInfo.GetCountToString();
            this.properties.SetCountValueText(rewardCount);
        }

        private void SetupIcon(Reward reward) {
            this.properties.SetIcon(reward.GetSpriteIcon());
        }

        public void ApplyReward() {
            if (rewardCurrent == null)
                throw new Exception($"There is no reward setupped");
            
            MakeFX(rewardCurrent);
        }

        protected abstract void MakeFX(Reward reward);
    }
}