using SinSity.Meta.Rewards;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace VavilichevGD.Meta.FortuneWheel.UI {
    public class UIWidgetFortuneWheelSector : UIWidget<UIWidgetFortuneWheelSectorProperties> {

        public Reward reward { get; private set; }

        public bool jackpotSector => this.properties.jackpotSector;

        public void Setup(Reward reward) {
            this.reward = reward;

            var rewardInfoEcoClicker = this.reward.GetRewardInfo<RewardInfoEcoClicker>();
            this.properties.imgIcon.sprite = rewardInfoEcoClicker.spriteIconOutlineBold;
            var countString = rewardInfoEcoClicker.GetCountToString();
            var countOne = countString == "1";
            this.properties.textCount.gameObject.SetActive(!countOne);
            if (!countOne)
                this.properties.textCount.text = $"x{countString}";
        }
        
    }
}