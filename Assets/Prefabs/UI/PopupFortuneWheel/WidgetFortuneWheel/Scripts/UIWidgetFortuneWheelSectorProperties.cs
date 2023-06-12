using System;
using TMPro;
using UnityEngine.UI;

namespace VavilichevGD.Meta.FortuneWheel.UI {
    [Serializable]
    public class UIWidgetFortuneWheelSectorProperties : UIProperties {
        public Image imgIcon;
        public TextMeshProUGUI textCount;
        public bool jackpotSector;
    }
}