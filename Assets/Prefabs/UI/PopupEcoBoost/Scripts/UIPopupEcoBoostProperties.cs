using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIPopupEcoBoostProperties : UIProperties
    {
        [SerializeField]
        private ProgressBarMasked m_progressBar;

        [SerializeField]
        private Button m_buttonStartEcoBoost;

        [SerializeField]
        private Text m_textTimer;

        [SerializeField]
        private AudioClip m_audioClipEcoBoostClick;

        [SerializeField] private Button m_btnClose;

        public ProgressBarMasked progressBarMasked
        {
            get { return this.m_progressBar; }
        }

        public Button buttonStartEcoBoost
        {
            get { return this.m_buttonStartEcoBoost; }
        }

        public Text textTimer
        {
            get { return this.m_textTimer; }
        }

        public AudioClip audioClipEcoBoostClick
        {
            get { return this.m_audioClipEcoBoostClick; }
        }

        public Button btnClose => m_btnClose;
    }
}