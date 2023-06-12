using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIPopupDailyRewardsProperties : UIProperties
    {
        [SerializeField] 
        public UIWidgetRewardCard[] rewardCards;

        [SerializeField]
        public Button buttonReceiveRewards;

        [SerializeField] 
        public AudioClip audioClipShow;
        
        [SerializeField] 
        public AudioClip audioClipGetClick;
    }
}