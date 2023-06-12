using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetPersCardContentReadyForPurchaseProps : UIProperties
    {
        [SerializeField]
        public Text textDescription;

        [SerializeField]
        public TextMeshProUGUI textNextIncomeSpeedMultiplicator;

        [SerializeField]
        public UIWidgetPriceSprite widgetPrice;
    }
}