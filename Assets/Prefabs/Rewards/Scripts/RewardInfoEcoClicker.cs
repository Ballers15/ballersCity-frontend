using UnityEngine;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Meta.Rewards {
    public abstract class RewardInfoEcoClicker : RewardInfo {
        [SerializeField] private Sprite m_spriteIconOutlineBold;

        public Sprite spriteIconOutlineBold => this.m_spriteIconOutlineBold;

    }
}