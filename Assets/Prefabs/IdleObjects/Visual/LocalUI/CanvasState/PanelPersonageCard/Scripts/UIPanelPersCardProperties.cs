using System;
using TMPro;
using UnityEngine;

namespace SinSity.UI {
    [Serializable]
    public sealed class UIPanelPersCardProperties : UIProperties {
        public TextMeshProUGUI textIncomeSpeedMultiplicatorCurrentValue;
        public UIWidgetPersCardContentPumping uiWidgetContentPumping;
        public UIWidgetPersCardContentReadyForPurchase uiWidgetContentReadyForPurchase;
        public Sprite spritePumpingMode;
        public Sprite spriteReadyForUpgradeMode;
        public UIPanelPersCardAnimator animator;
    }
}