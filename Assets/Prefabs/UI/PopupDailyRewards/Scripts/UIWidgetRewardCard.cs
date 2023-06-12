using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetRewardCard : UIWidgetAnim<UIWidgetRewardCardProperties>
    {
        private static readonly int BOOL_TODAY = Animator.StringToHash("today");

        public void Setup(Args args)
        {
            var rewardInfo = args.rewardInfo;
            this.properties.SetIcon(rewardInfo.GetSpriteIcon());
            this.properties.SetupCard(args.isToday, args.isReceived);
            this.properties.SetCountValue(rewardInfo.GetCountToString());
            this.properties.SetDayNumber(args.dayNumber.ToString());
            
            animator.SetBool(BOOL_TODAY, args.isToday);
        }

        public sealed class Args
        {
            public bool isToday { get; set; }

            public bool isReceived { get; set; }

            public int dayNumber { get; set; }

            public RewardInfoEcoClicker rewardInfo { get; set; }
        }
    }
}