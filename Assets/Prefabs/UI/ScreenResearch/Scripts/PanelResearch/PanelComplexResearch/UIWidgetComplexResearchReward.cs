using System.Runtime.CompilerServices;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetComplexResearchReward : UIWidget<UIWidgetComplexResearchRewardProperties> {
        public void Setup(RewardInfoEcoClicker rewardInfo) {
            this.properties.SetIcon(rewardInfo.GetSpriteIcon());
            this.properties.SetDescription(rewardInfo.GetCountToString());
        }

        public Vector3 GetIconPosition() {
            return this.properties.GetIconPosition();
        }
    }
}