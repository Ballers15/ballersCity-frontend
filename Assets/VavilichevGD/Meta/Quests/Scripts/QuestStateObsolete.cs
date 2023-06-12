using System;
using VavilichevGD.Meta.Quests;

namespace VavilichevGD.Meta {
    [Serializable]
    public abstract class QuestStateObsolete {

        public delegate void QuestStateChangedHandler(QuestStateObsolete state);
        public static event QuestStateChangedHandler OnQuestStateChanged;
        public event QuestStateChangedHandler OnStateChanged;
        
        public string id;
        public bool isViewed;
        public bool isCompleted;
        public bool isActive;
        public int completeTimes;

        public QuestStateObsolete(string stateJson) {
            SetState(stateJson);
        }

        public QuestStateObsolete(QuestInfoObsolete info) {
            id = info.id;
            isViewed = false;
            isCompleted = false;
            isActive = false;
            completeTimes = 0;
        }
        
        public abstract void SetState(string stateJson);
        public abstract string GetStateJson();

        public virtual void MarkAsCompleted()
        {
            completeTimes++;
            isActive = false;
            isCompleted = true;
            NotifyAboutStateChanged();
        }

        public virtual void MarkAsViewed() {
            isViewed = true;
            NotifyAboutStateChanged();
        }

        protected void NotifyAboutStateChanged() {
            OnQuestStateChanged?.Invoke(this);
            OnStateChanged?.Invoke(this);
        }

        public void Activate() {
            isActive = true;
            NotifyAboutStateChanged();
        }

        public void Deactivate() {
            isActive = false;
            NotifyAboutStateChanged();
        }
    }
}