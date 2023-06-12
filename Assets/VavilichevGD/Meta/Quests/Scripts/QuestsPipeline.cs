using UnityEngine;

namespace VavilichevGD.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestsPipeline", menuName = "Meta/Quests/QuestsPipeline")]
    public class QuestsPipeline : ScriptableObject
    {
        [SerializeField]
        protected QuestInfo[] quests;
    }
}