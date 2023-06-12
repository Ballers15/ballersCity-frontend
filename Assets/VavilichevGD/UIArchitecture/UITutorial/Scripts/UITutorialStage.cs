using System;

namespace VavilichevGD.UI {
    [Serializable]
    public class UITutorialStage {
        public int lastIndexTutorialStep;
        public bool isComplete;

        public static UITutorialStage GetDefault() {
            UITutorialStage stageDefault = new UITutorialStage();
            stageDefault.isComplete = false;
            stageDefault.lastIndexTutorialStep = -1;
            return stageDefault;
        }
    }
}