﻿using VavilichevGD.Tools;

namespace VavilichevGD.Meta.Quests
{
    public class Quest
    {
        #region Events

        public delegate void QuestStateChangeHandler(Quest quest, QuestState newState);

        public static event QuestStateChangeHandler OnQuestStateChanged;
        public event QuestStateChangeHandler OnStateChanged;

        #endregion

        public QuestInfo info { get; protected set; }

        public T GetInfo<T>() where T : QuestInfo
        {
            return (T)this.info;
        }

        public QuestState state { get; protected set; }

        public T GetState<T>() where T : QuestState
        {
            return (T)this.state;
        }

        public float progressNormalized => inspector.progressNormalized;
        public string progressDescription => inspector.progressDescription;
        public bool isCompleted => state.isCompleted;
        public bool isViewed => state.isViewed;
        public bool isActive => state.isActive;
        public bool isRewardTaken => state.isRewardTaken;
        public string id => info.id;

        private readonly QuestInspector inspector;

        public Quest(QuestInfo info, QuestState state)
        {
            this.info = info;
            this.state = state;
            this.inspector = info.CreateInspector(this);
        }

        public void CheckState()
        {
            this.inspector.CheckState();
        }

        public void StartQuest()
        {
            state.Activate();
            inspector.StartQuest();
        }

        public void NotifyQuestStateChanged()
        {
            OnQuestStateChanged?.Invoke(this, state);
            OnStateChanged?.Invoke(this, state);
        }

        public void Complete()
        {
            state.MarkAsCompleted();
            inspector.Destroy();
            NotifyQuestStateChanged();
        }
    }
}