using System;
using TMPro;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public class UIWidgetPanelResearchTimerProperties : UIWidgetPanelResearchProperties
    {
        [SerializeField]
        private TextMeshProUGUI textTimerValue;

        public void SetTimerValueText(string timerValueText)
        {
            this.textTimerValue.text = timerValueText;
        }
    }
}