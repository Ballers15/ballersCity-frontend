using System;
using UnityEngine;

namespace VavilichevGD.UI {
    [Serializable]
    public class UITutorialStep {
        [Tooltip("RootUIElementName/.../UIElementName")]
        public string uiElementPath;
        public string descriptionText;
        public bool shiftToFirstPlan = true;
        public bool pointerInebled = true;
    }
}