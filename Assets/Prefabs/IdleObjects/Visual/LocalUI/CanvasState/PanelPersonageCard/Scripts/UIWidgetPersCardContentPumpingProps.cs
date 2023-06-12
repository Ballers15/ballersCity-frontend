using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetPersCardContentPumpingProps : UIProperties
    {
        [SerializeField] 
        public TextMeshProUGUI textPersCardLevelValue;

        [SerializeField] 
        public ProgressBarMasked progressBar;

        [SerializeField] 
        public Image imageIcon;
    }
}