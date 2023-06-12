using System;
using UnityEngine;

namespace VavilichevGD.Meta.Rewards
{
    [Serializable]
    public abstract class RewardInfo : ScriptableObject
    {
        [SerializeField] protected string m_id;
        [SerializeField] protected string m_title;
        [SerializeField] protected string m_description;
        [SerializeField] protected Sprite m_spriteIcon;

        public string id
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }

        public virtual string GetTitle()
        {
            return this.m_title;
        }

        public virtual string GetDescription()
        {
            return this.m_description;
        }

        public virtual Sprite GetSpriteIcon()
        {
            return this.m_spriteIcon;
        }

        public abstract RewardHandler CreateRewardHandler(Reward reward);
        public abstract string GetCountToString();

    }
}