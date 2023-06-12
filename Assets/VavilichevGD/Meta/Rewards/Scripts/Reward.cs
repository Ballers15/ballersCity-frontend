using SinSity.Services;
using UnityEngine;

namespace VavilichevGD.Meta.Rewards
{
    public class Reward
    {
        public delegate void RewardReceivedHandler(Reward reward, bool success);

        public static event RewardReceivedHandler OnRewardReceived;
        public event RewardReceivedHandler OnReceived;

        public RewardInfo info { get; }
        public string id => this.info.id;

        public T GetRewardInfo<T>() where T : RewardInfo {
            if (this.info is T castedInfo)
                return castedInfo;
            return null;
        }
        
        public Reward(RewardInfo info)
        {
            this.info = info;
        }

        public void NotifyAboutRewardReceived(bool success)
        {
            OnRewardReceived?.Invoke(this, success);
            OnReceived?.Invoke(this, success);
            CommonAnalytics.LogRewardReceived(this.info.id, success);
        }

        public virtual void Apply(object sender, bool instantly) {
            var handler = info.CreateRewardHandler(this);
            handler.ApplyReward(sender, instantly);
        }

        public string GetTitle()
        {
            return info.GetTitle();
        }

        public string GetDescription()
        {
            return info.GetDescription();
        }

        public Sprite GetSpriteIcon()
        {
            return info.GetSpriteIcon();
        }
    }
}