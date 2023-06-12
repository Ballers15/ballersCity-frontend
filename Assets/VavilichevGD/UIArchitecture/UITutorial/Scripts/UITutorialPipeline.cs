using UnityEngine;

namespace VavilichevGD.UI {
    [CreateAssetMenu(fileName = "UITutorialPipeline", menuName = "Tutorial/Pipeline")]
    public class UITutorialPipeline : ScriptableObject {
        [SerializeField] protected UITutorialStep[] steps;

        public bool IsValidIndex(int index) {
            return index < steps.Length;
        }

        public UITutorialStep GetStep(int index) {
            return steps[index];
        }
    }
}