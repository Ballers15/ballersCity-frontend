using SinSity.Monetization;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public class FXCase : FXUIFlyingReward<Product> {

        [SerializeField] private Image imgSpriteIcon;

        private int rewardCount;

        public void SetReward(Product productCase, int count) {
            base.SetReward(productCase);
            this.rewardCount = count;
        }

        public void SetIcon(Sprite spriteIcon) {
            imgSpriteIcon.sprite = spriteIcon;
        }

        protected override void ApplyReward() {
//            ProductStateCase state = rewardCurrent.GetState<ProductStateCase>();
//            state.AddCases(rewardCount);
        }
    }
}