using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UICardRewardSummaryProperties : UIProperties
    {
        [SerializeField]
        private Text m_textDescription;

        [SerializeField]
        private Text m_textCount;

        [SerializeField]
        private Image m_imageIcon;

        [SerializeField] 
        private UICardRewardSummaryAnimator m_animator;

        [SerializeField]
        public AudioClip audioClipShow;

        public Text textDescription
        {
            get { return this.m_textDescription; }
        }

        public Text textCount
        {
            get { return this.m_textCount; }
        }

        public Image imageIcon
        {
            get { return this.m_imageIcon; }
        }

        public UICardRewardSummaryAnimator animator {
            get { return this.m_animator; }
        }
    }
}