using System;
using VavilichevGD.Meta.Quests;

namespace VavilichevGD.Meta
{
    [Serializable]
    public class QuestState
    {

        public delegate void QuestStateChangedHandler(QuestState state);
        public static event QuestStateChangedHandler OnQuestStateChanged; 
        public event QuestStateChangedHandler OnStateChanged;

        public string id;
        public bool isViewed;
        public bool isCompleted;
        public bool isActive;
        public bool isRewardTaken;
        public int completeTimes;

        public QuestState(string stateJson)
        {
            SetState(stateJson);
        }

        public QuestState(QuestInfo info)
        {
            id = info.id;
            isViewed = false;
            isCompleted = false;
            isActive = false;
            isRewardTaken = false;
            completeTimes = 0;
        }

        public virtual void SetState(string stateJson) {
            throw new NotImplementedException();
        }
        public virtual string GetStateJson() {
            throw new NotImplementedException();
        }

        public virtual void MarkAsCompleted()
        {
            completeTimes++;
            isActive = false;
            isCompleted = true;
            NotifyAboutStateChanged();
        }

        public virtual void MarkAsViewed()
        {
            isViewed = true;
            NotifyAboutStateChanged();
        }

        public virtual void MarkThatRewardIsTaken()
        {
            isRewardTaken = true;
            NotifyAboutStateChanged();
        }

        protected void NotifyAboutStateChanged()
        {
            OnQuestStateChanged?.Invoke(this);
            OnStateChanged?.Invoke(this);
        }

        public void Activate()
        {
            isActive = true;
            NotifyAboutStateChanged();
        }

        public void Deactivate()
        {
            isActive = false;
            NotifyAboutStateChanged();
        }
    }
}