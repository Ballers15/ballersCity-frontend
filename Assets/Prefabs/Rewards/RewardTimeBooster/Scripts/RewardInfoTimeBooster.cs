using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards
{
    [CreateAssetMenu(fileName = "RewardInfoTimeBooster", menuName = "Meta/Rewards/RewardTimeBooster")]
    public sealed class RewardInfoTimeBooster : RewardInfoEcoClicker
    {
        [SerializeField]
        private ProductInfoTimeBooster m_timeBoosterInfo;

        [SerializeField]
        public int count = 1;

        public ProductInfoTimeBooster timeBoosterInfo
        {
            get { return m_timeBoosterInfo; }
        }

        public override RewardHandler CreateRewardHandler(Reward reward)
        {
            return new RewardHandlerTimeBooster(reward);
        }

        public override string GetCountToString() {
            return this.count.ToString();
        }

        public override string GetDescription()
        {
            var localizedDescription = Localization.GetTranslation(timeBoosterInfo.GetDesctiption());
            return string.Format(localizedDescription, timeBoosterInfo.timeHours);
        }

        public void Setup(int count)
        {
            this.count = count;
        }

        public override string GetTitle() {
            string localizedTitle = Localization.GetTranslation(this.m_timeBoosterInfo.GetTitle());
            return string.Format(localizedTitle, this.m_timeBoosterInfo.timeHours);
        }
    }
}