using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetModerizationPopProperties : UIProperties
    {
        [SerializeField]
        private TextMeshProUGUI m_textIncome;

        [SerializeField]
        private TextMeshProUGUI m_textScore;

        [SerializeField]
        private TextMeshProUGUI m_textFutureIncome;

        public TextMeshProUGUI textIncome
        {
            get { return this.m_textIncome; }
        }

        public TextMeshProUGUI textScore
        {
            get { return this.m_textScore; }
        }

        public TextMeshProUGUI textFutureIncome
        {
            get { return this.m_textFutureIncome; }
        }

    }
}