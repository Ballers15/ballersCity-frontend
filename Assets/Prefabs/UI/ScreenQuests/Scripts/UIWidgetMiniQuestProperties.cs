using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetMiniQuestProperties : UIProperties
    {
        [SerializeField]
        private ProgressBarFillerMasked m_progressBar;

        [SerializeField]
        private Text m_textTask;

        [SerializeField]
        private Text m_textProgress;

        [SerializeField]
        private Image m_rewardIcon;

        [SerializeField]
        private TextMeshProUGUI m_textCount;

        [SerializeField]
        private Button m_buttonTakeReward;

        [SerializeField]
        private Image m_imageTakeReward;

        [SerializeField] 
        private Image m_imageTakeRewardHighlight;

        [SerializeField]
        private Sprite m_spriteTakeRewardEnable;

        [SerializeField] 
        private Sprite m_spriteTakeRewardEnableHighlight;

        [SerializeField]
        private Sprite m_spriteTakeRewardDisable;

        [SerializeField] 
        private Sprite m_spriteTakeRewardDisableHighlight;

        [SerializeField] 
        private Color m_colorFillerEnable;

        [SerializeField] 
        private Color m_colorFillerDisable;

        [SerializeField] 
        private GameObject m_Complete;
        
        [SerializeField] 
        public AudioClip audioClipFinishMiniQuest;

        public ProgressBarFillerMasked progressBar
        {
            get { return this.m_progressBar; }
        }

        public Text textTask => this.m_textTask;

        public Text textProgress => this.m_textProgress;

        public Image rewardIcon
        {
            get { return this.m_rewardIcon; }
        }

        public TextMeshProUGUI textCount
        {
            get { return this.m_textCount; }
        }

        public Button buttonTakeReward
        {
            get { return this.m_buttonTakeReward; }
        }

        public Image imageTakeReward
        {
            get { return this.m_imageTakeReward; }
        }

        public Image imageTakeRewardHighlight => m_imageTakeRewardHighlight;

        public Sprite spriteTakeRewardEnable
        {
            get { return this.m_spriteTakeRewardEnable; }
        }

        public Sprite spriteTakeRewardEnableHighlight => m_spriteTakeRewardEnableHighlight;

        public Sprite spriteTakeRewardDisable
        {
            get { return this.m_spriteTakeRewardDisable; }
        }

        public Sprite spriteTakeRewardDisableHighlight => m_spriteTakeRewardDisableHighlight;

        public Color colorFillerEnable
        {
            get { return this.m_colorFillerEnable; }
        }
        
        public Color colorFillerDisable
        {
            get { return this.m_colorFillerDisable; }
        }

        public GameObject Complete
        {
            get { return this.m_Complete; }
        }
    }
}