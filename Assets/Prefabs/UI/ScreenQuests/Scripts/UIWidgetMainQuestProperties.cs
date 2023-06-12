using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public sealed class UIWidgetMainQuestProperties : UIProperties {
        
        [SerializeField] private ProgressBarFillerMasked _progressBar;
        [SerializeField] private Text _textTask;
        [SerializeField] private TextMeshProUGUI _textProgress;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _textRewardCount;
        [SerializeField] private Button _receiveReward;
        [SerializeField] private AudioClip _audioClipFinishQuest;
        [SerializeField] private Image _imageTakeReward;
        [SerializeField] private Image _imageTakeRewardHighlight;
        [SerializeField] private Sprite _spriteTakeRewardEnable;
        [SerializeField] private Sprite _spriteTakeRewardEnableHighlight;
        [SerializeField] private Sprite _spriteTakeRewardDisable;
        [SerializeField] private Sprite _spriteTakeRewardDisableHighlight;
        [SerializeField] private Color m_colorFillerEnable;
        [SerializeField] private Color m_colorFillerDisable;
        

        public AudioClip audioClipFinishMQuest => this._audioClipFinishQuest;
        public ProgressBarFillerMasked progressBarMQuest => this._progressBar;
        public Text textTaskMQuest => this._textTask;
        public TextMeshProUGUI textProgressMQuest => this._textProgress;
        public Image rewardIconMQuest => this._rewardIcon;
        public TextMeshProUGUI textRewardCountMQuest => this._textRewardCount;
        public Button buttonReceiveRewardMQuest => this._receiveReward;

        public Sprite spriteTakeRewEnableMQuest => this._spriteTakeRewardEnable;

        public Sprite spriteTakeRewEnableHighlMQuest => this._spriteTakeRewardEnableHighlight;

        public Color colorFillerEnableMQuest => this.m_colorFillerEnable;

        public Sprite spriteTakeRewardDisableMQuest => this._spriteTakeRewardDisable;

        public Sprite spriteTakeRewDisableHighlMQuest => this._spriteTakeRewardDisableHighlight;

        public Color colorFillerDisableMQuest => this.m_colorFillerDisable;

        public Image imageTakeRewardMQuest => this._imageTakeReward;

        public Image imageTakeRewardHighlMQuest => this._imageTakeRewardHighlight;
    }
}