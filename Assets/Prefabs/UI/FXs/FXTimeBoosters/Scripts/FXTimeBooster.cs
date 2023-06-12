using SinSity.Monetization;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public class FXTimeBooster : FXUIFlyingReward<Product> {
        [SerializeField] private Image imgSpriteIcon;

        private int rewardCount;

        public void SetReward(Product productTimeBooster, int count) {
            base.SetReward(productTimeBooster);
            this.rewardCount = count;
        }

        public void SetIcon(Sprite spriteIcon) {
            imgSpriteIcon.sprite = spriteIcon;
        }

        protected override void ApplyReward() {
//            ProductStateTimeBooster state = rewardCurrent.GetState<ProductStateTimeBooster>();
//            state.AddBoosters(rewardCount);
        }
    }
}