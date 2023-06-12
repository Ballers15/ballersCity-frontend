using System;

namespace VavilichevGD.UI {
    [Serializable]
    public class UITutorialState {
        public int lastIndexTutorialStep;
        public bool isComplete;

        public static UITutorialState GetDefault() {
            UITutorialState stateDefault = new UITutorialState();
            stateDefault.isComplete = false;
            stateDefault.lastIndexTutorialStep = -1;
            return stateDefault;
        }
    }
}