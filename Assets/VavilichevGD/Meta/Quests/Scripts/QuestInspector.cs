namespace VavilichevGD.Meta.Quests {
    public abstract class QuestInspector {

        protected Quest quest;

        public float progressNormalized => GetProgressNormalized();
        public string progressDescription => GetProgressDescription();

        public QuestInspector(Quest quest) {
            this.quest = quest;
        }

        public virtual void StartQuest() {
            SubscribeOnEvents();
        }

        public virtual void Destroy() {
            UnsubscribeFromEvents();
        }
        
        protected abstract void SubscribeOnEvents();
        protected abstract void UnsubscribeFromEvents();
        protected abstract float GetProgressNormalized();
        protected abstract string GetProgressDescription();

        public abstract void CheckState();
    }
}