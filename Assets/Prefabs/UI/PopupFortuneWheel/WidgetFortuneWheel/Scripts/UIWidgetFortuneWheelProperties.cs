using System;
using SinSity.Meta;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIWidgetFortuneWheelProperties : UIProperties {
        public FortuneWheel fortuneWheel;
        public UIWidgetButtonFortuneWheelSpin btnRotateFree;
        public UIWidgetButtonFortuneWheelSpin btnRotateForAD;
        public UIWidgetButtonFortuneWheelSpin btnRotateForGems;
        public Text textPriceRotateGorGems;
        [Space] 
        public UIWidgetFortuneWheelLightsAnimator lightsAnimator;
        [Space] 
        public AudioClip sfxSpin;
    }
}