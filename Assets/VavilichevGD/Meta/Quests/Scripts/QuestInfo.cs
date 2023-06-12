using UnityEngine;

namespace VavilichevGD.Meta.Quests
{
    public abstract class QuestInfo : ScriptableObject
    {
        [SerializeField] protected string m_id;
        [SerializeField] protected string m_titleCode;
        [SerializeField] protected string m_descriptionCode;

        public string id => m_id;

        public virtual string GetTitle()
        {
            return m_titleCode;
        }

        public virtual string GetDescription()
        {
            return m_descriptionCode;
        }

        public abstract QuestInspector CreateInspector(Quest quest);
        public abstract QuestState CreateState(string stateJson);
        public abstract QuestState CreateStateDefault();
    }
}