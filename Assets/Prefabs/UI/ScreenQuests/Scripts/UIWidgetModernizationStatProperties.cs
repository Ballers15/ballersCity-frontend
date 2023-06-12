using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public sealed class UIWidgetModernizationStatProperties : UIProperties {
        [SerializeField] private Text m_textIncome;
        [SerializeField] private Text m_textScore;

        public Text textIncome => this.m_textIncome;
        public Text textScore => this.m_textScore;
    }
}