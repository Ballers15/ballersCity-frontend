using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetRenovationQuestProperties : UIProperties
    {
        [SerializeField]
        private TextMeshProUGUI m_textTask;

        [SerializeField]
        private TextMeshProUGUI m_textProgress;

        [SerializeField]
        private ProgressBarMasked m_progressBarMasked;

        [SerializeField]
        private Button mButtonStartRenovation;

        [SerializeField]
        public AudioClip audioClipStartRenovation;
        
        public TextMeshProUGUI textTask
        {
            get { return this.m_textTask; }
        }

        public TextMeshProUGUI textProgress
        {
            get { return this.m_textProgress; }
        }

        public ProgressBarMasked progressBarMasked
        {
            get { return this.m_progressBarMasked; }
        }

        public Button buttonStartRenovation
        {
            get { return this.mButtonStartRenovation; }
        }
    }
}