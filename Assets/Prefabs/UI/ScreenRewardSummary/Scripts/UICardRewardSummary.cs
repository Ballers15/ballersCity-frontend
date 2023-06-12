using SinSity.Meta.Rewards;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UICardRewardSummary : UIPanel<UICardRewardSummaryProperties>
    {
        public RewardInfoEcoClicker currentRewardInfo { get; private set; }

        public delegate void AppearingHandler(UICardRewardSummary cardRewardSummary);
        public event AppearingHandler OnAppearingOver;
        
        public void Setup(RewardInfoEcoClicker rewardInfo)
        {
            this.currentRewardInfo = rewardInfo;
            string localizedDescrition = rewardInfo.GetDescription();
            this.properties.textDescription.text = rewardInfo.GetDescription();
            this.properties.textCount.text = rewardInfo.GetCountToString();
            this.properties.imageIcon.sprite = rewardInfo.GetSpriteIcon();
            this.properties.animator.OnAnimationOver += OnAnimationOver;
        }

        private void OnAnimationOver() {
            properties.animator.OnAnimationOver -= OnAnimationOver;
            OnAppearingOver?.Invoke(this);
        }

        public void SpeedUp() {
            properties.animator.SpeedUp();
        }

        public void ShowCard() {
            SFX.PlaySFX(this.properties.audioClipShow);
            properties.animator.Show();
        }
    }
}