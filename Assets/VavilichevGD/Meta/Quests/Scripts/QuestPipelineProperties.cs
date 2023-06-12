using UnityEngine;

namespace VavilichevGD.Meta.Quests {
    [CreateAssetMenu(fileName = "QuestPipelineProperties", menuName = "Meta/Quests/PipelineProperties")]
    public class QuestPipelineProperties : ScriptableObject {
        public int portionSize = 1;
        public bool onePortionPerDay = false;
    }
}