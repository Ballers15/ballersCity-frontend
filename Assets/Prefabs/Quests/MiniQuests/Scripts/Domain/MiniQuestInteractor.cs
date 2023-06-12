using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Core;
using SinSity.Repo;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Domain
{
    public sealed class MiniQuestInteractor : Interactor, IModernizationAsyncListenerInteractor,
        IBankStateWithoutNotification
    {
        #region Const

        private const string CONFIG_PATH = "Config/MiniQuestConfig";

        private const string QUESTS_FOLDER_PATH = "MiniQuests";

        private const int MAX_ACTIVE_QUEST_COUNT = 3;

        #endregion

        #region Event

        public event Action<object> OnQuestUnlockedEvent;

        public event Action<Quest> OnQuestChangedEvent;

        public event Action<Quest> OnQuestFinishedEvent;

        public event Action<object> OnResetTimeChangedEvent;

        #endregion

        public bool isQuestsUnlocked { get; private set; }

        private MiniQuestConfig config;

        private Dictionary<string, Quest> activeQuestMap;

        private Dictionary<string, QuestInfoEcoClicker> questInfoMap;

        private Dictionary<string, DailyQuestInfo> dailyQuestInfoMap;

        private new MiniQuestsRepository repository;

        private MiniQuestStatistics statistic;

        public override bool onCreateInstantly { get; } = true;


        public List<Quest> GetActiveQuests()
        {
            var activeQuests = this.activeQuestMap.Values.ToList();
            var counter = 1;
//            foreach (var activeQuest in activeQuests)
//                Debug.Log($"{counter++} INTERACCCCTTTTOORRRR        AAAAAAAAAAAAACTIVE QUEST: " + activeQuest.id);

            return activeQuests;
        }

        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.activeQuestMap = new Dictionary<string, Quest>();
            this.config = Resources.Load<MiniQuestConfig>(CONFIG_PATH);
            this.repository = this.GetRepository<MiniQuestsRepository>();
            this.statistic = this.repository.statistic;
            var questInfoSet = Resources.LoadAll<QuestInfoEcoClicker>(QUESTS_FOLDER_PATH);
            var dailyQuestInfoSet = Resources.LoadAll<DailyQuestInfo>(QUESTS_FOLDER_PATH);
            this.questInfoMap = questInfoSet.ToDictionary(it => it.id);
            this.dailyQuestInfoMap = dailyQuestInfoSet.ToDictionary(it => it.id);
            Resources.UnloadUnusedAssets();
            this.Setup();
        }

        private void Setup()
        {
            this.isQuestsUnlocked = this.repository.GetIsUnlocked();
        }
        #endregion

        #region OnReady

        public override void OnReady()
        {
            base.OnReady();
            Debug.Log("MINI QUESTS UNLOCKED: " + this.isQuestsUnlocked);
            
            if (this.isQuestsUnlocked)
            {
                this.InflateActiveQuests();
            }
        }


        private void InflateActiveQuests()
        {
            this.InflateSavedQuests();
            this.TryInflateFreshQuests();
        }

        #endregion

        #region InflateSavedQuests

        private void InflateSavedQuests()
        {
            var entities = this.repository.GetEntities();
            var counter = 0;
            
            Debug.Log("SAVED MINI QUEST COUNT: " + entities.Count);
            
            foreach (var entity in entities)
            {
                var entityId = entity.id;
                if (!this.questInfoMap.ContainsKey(entityId))
                {
                    continue;
                }

                var questInfo = this.questInfoMap[entityId];
                var questDataJson = entity.json;
                this.InflateSavedQuest(questInfo, questDataJson);
                counter++;
                if (counter >= MAX_ACTIVE_QUEST_COUNT)
                {
                    break;
                }
            }
        }

        private void InflateSavedQuest(QuestInfoEcoClicker savedQuestInfo, string questDataJson)
        {
            QuestState questState;
            Debug.Log("INFLATE SAVED QUEST: " + savedQuestInfo.id);
            
            try
            {
                var questStateBase = JsonUtility.FromJson<QuestState>(questDataJson);
                questState = savedQuestInfo.CreateState(questDataJson);
            }
            catch(Exception e)
            {
                questState = savedQuestInfo.CreateStateDefault();
            }

            //var questSavedState = savedQuestInfo.CreateState(questDataJson);
            var savedQuest = new Quest(savedQuestInfo, questState);

            this.EnableQuest(savedQuest);
        }

        #endregion

        #region InflateFreshQuests

        public void TryInflateFreshQuests()
        {
            var absentQuestCount = this.config.maxActiveQuestCount - this.activeQuestMap.Count;
            if (absentQuestCount <= 0)
            {
                return;
            }

            var availableFreshQuests = this.GetAvailableFreshQuests();
            var freshQuestInfoSet = this.GetRandomSet(availableFreshQuests, absentQuestCount);
            foreach (var freshQuestInfo in freshQuestInfoSet)
            {
                this.InflateFreshQuest(freshQuestInfo);
            }
        }
        
        public void TryInflateFreshDailyQuests()
        {
            var availableFreshQuests = this.GetAvailableDailyQuests();
            var freshQuestInfoSet = this.GetRandomDailyQuests(availableFreshQuests);
            foreach (var freshQuestInfo in freshQuestInfoSet)
            {
                this.InflateFreshQuest(freshQuestInfo);
            }
        }

        private List<QuestInfoEcoClicker> GetAvailableFreshQuests()
        {
            var activeQuests = this.activeQuestMap.Values.ToList();
            var availableFreshQuests = this.questInfoMap.Values.ToSet();
            foreach (var activeQuest in activeQuests)
            {
                var activeQuestTitle = activeQuest.info.GetTitle();
                var again = true;
                while (again)
                {
                    again = false;
                    foreach (var availableFreshQuest in availableFreshQuests)
                    {
                        if (availableFreshQuest.GetTitle() == activeQuestTitle)
                        {
                            again = true;
                            availableFreshQuests.Remove(availableFreshQuest);
                            break;
                        }
                    }
                }
            }

            var freshQuests = availableFreshQuests.ToSet();
            foreach (var freshQuest in freshQuests)
            {
                if (!freshQuest.CanCreateQuest())
                {
                    availableFreshQuests.Remove(freshQuest);
                }
            }

            if (availableFreshQuests.IsEmpty())
            {
                throw new Exception("Available quests are absent!");
            }

            return availableFreshQuests.ToList();
        }

        private List<DailyQuestInfo> GetAvailableDailyQuests()
        {
            var activeQuests = this.activeQuestMap.Values.ToList();
            var availableFreshQuests = this.dailyQuestInfoMap.Values.ToSet();
            foreach (var activeQuest in activeQuests)
            {
                var activeQuestTitle = activeQuest.info.GetTitle();
                var again = true;
                while (again)
                {
                    again = false;
                    foreach (var availableFreshQuest in availableFreshQuests)
                    {
                        if (availableFreshQuest.GetTitle() == activeQuestTitle)
                        {
                            again = true;
                            availableFreshQuests.Remove(availableFreshQuest);
                            break;
                        }
                    }
                }
            }

            var freshQuests = availableFreshQuests.ToSet();
            foreach (var freshQuest in freshQuests)
            {
                if (!freshQuest.CanCreateQuest())
                {
                    availableFreshQuests.Remove(freshQuest);
                }
            }

            if (availableFreshQuests.IsEmpty())
            {
                throw new Exception("Available quests are absent!");
            }

            return availableFreshQuests.ToList();
        }

        public List<QuestInfoEcoClicker> GetRandomSet(List<QuestInfoEcoClicker> infoList, int count)
        {
            if (infoList.Count < count)
            {
                throw new Exception("Required count more than enumerable size!");
            }

            var restItems = infoList.ToList();
            var resultRandomList = new List<QuestInfoEcoClicker>();
            for (var i = 0; i < count; i++)
            {
                var randomItem = restItems.GetRandom();
                resultRandomList.Add(randomItem);

                var again = true;
                while (again)
                {
                    again = false;
                    foreach (var item in restItems)
                    {
                        if (item.GetTitle() == randomItem.GetTitle())
                        {
                            restItems.Remove(item);
                            again = true;
                            break;
                        }
                    }
                }
            }

            return resultRandomList;
        }
        
        public List<QuestInfoEcoClicker> GetRandomDailyQuests(List<DailyQuestInfo> infoList)
        {
            if (infoList.Count < MAX_ACTIVE_QUEST_COUNT)
            {
                throw new Exception("Required count more than enumerable size!");
            }

            var restItems = infoList.ToList();

            var hardQuestList = restItems.FindAll(delegate (DailyQuestInfo qinfo) { return qinfo.difficulty == DailyQuestInfo.Difficulty.hard; });
            var normalQuestList = restItems.FindAll(delegate (DailyQuestInfo qinfo) { return qinfo.difficulty == DailyQuestInfo.Difficulty.normal; });
            var easyQuestList = restItems.FindAll(delegate (DailyQuestInfo qinfo) { return qinfo.difficulty == DailyQuestInfo.Difficulty.easy; });

            var easyQuest = easyQuestList.GetRandom();
            normalQuestList = RemoveSameQuestType(easyQuest, normalQuestList);
            hardQuestList = RemoveSameQuestType(easyQuest, hardQuestList);
            var normalQuest = normalQuestList.GetRandom();
            hardQuestList = RemoveSameQuestType(normalQuest, hardQuestList);
            var hardQuest = hardQuestList.GetRandom();

            var resultRandomList = new List<QuestInfoEcoClicker>()
            {
                hardQuest,
                normalQuest,
                easyQuest,
            };

            return resultRandomList;
        }

        private List<DailyQuestInfo> RemoveSameQuestType(DailyQuestInfo quest, List<DailyQuestInfo> questsList)
        {
            var newList = questsList.ToList();
            foreach (var q in questsList)
            {
                if(q.GetTitle() == quest.GetTitle())
                    newList.Remove(q);
            }

            return newList;
        }

        #region QUEST RESET

        public bool CanUseReset()
        {
            var timeLastReset = this.statistic.lastResetTimeSerialized.GetDateTime();
            var now = DateTime.Now;
            var secondsDifferent = (timeLastReset - now).TotalSeconds;
            return secondsDifferent <= 0;
        }

        public void UpdateLastResetTime(object sender)
        {
            var now = DateTime.Now;
            var targetDate = new DateTime(now.Year,now.Month,now.Day,config.resetQuestsHour,0,0);
            var dateDiff = (targetDate - now).TotalSeconds;
            
            if (dateDiff <= 0)
            {
                targetDate = targetDate.AddDays(1);
            }
            
            this.statistic.lastResetTimeSerialized = new DateTimeSerialized(targetDate);
            this.OnResetTimeChangedEvent?.Invoke(sender);
        }

        public int GetTimeToNextReset()
        {
            var lastResetTime = this.statistic.lastResetTimeSerialized.GetDateTime();

            if (lastResetTime == new DateTime()) return 0;
            
            var now = DateTime.Now;
            var diff = lastResetTime - now;
            var diffInt = Convert.ToInt32(diff.TotalSeconds);
            var totalSeconds = Mathf.Max(diffInt, 0);
            return totalSeconds;
        }

        public void UpdateResetButtonState(bool state)
        {
            repository.statistic.resetWasUsed = state;
        }

        #endregion

        private void InflateFreshQuest(QuestInfoEcoClicker freshQuestInfo)
        {
            Debug.Log("INFALTE FRESH MINI QUEST " + freshQuestInfo.id);
            
            var freshQuestState = freshQuestInfo.CreateStateDefault();
            var freshQuest = new Quest(freshQuestInfo, freshQuestState);
            this.SaveQuestIntoRepository(freshQuest);
            this.EnableQuest(freshQuest);
            CommonAnalytics.LogDailyQuestStarted(freshQuest.id);
            this.OnQuestChangedEvent?.Invoke(freshQuest);
        }

        #endregion

        private void EnableQuest(Quest quest)
        {
            quest.OnStateChanged += this.OnQuestStateChanged;
            this.activeQuestMap[quest.id] = quest;
            quest.StartQuest();
        }

        private void DisableQuest(Quest quest)
        {
            quest.OnStateChanged -= this.OnQuestStateChanged;
            //this.DeleteQuestFromRepository(quest);
            //this.activeQuestMap.Remove(quest.id);
        }

        private void DeleteQuest(Quest quest)
        {
            this.DeleteQuestFromRepository(quest);
            this.activeQuestMap.Remove(quest.id);
        }

        #region QuestEvent

        private void OnQuestStateChanged(Quest quest, QuestState newstate)
        {
            this.SaveQuestIntoRepository(quest);
            this.OnQuestChangedEvent?.Invoke(quest);

            if (newstate.isCompleted && !newstate.isRewardTaken)
            {
                TryToShowNotification(quest);
                if (newstate.completeTimes == 1)
                {
                    CommonAnalytics.LogDailyQuestEnd(quest.id);
                }
            }
        }

        private void TryToShowNotification(Quest quest)
        {
            Coroutines.StartRoutine(WaitForShowing(quest));
        }

        private IEnumerator WaitForShowing(Quest quest)
        {
            UIInteractor uiInteractor = GetInteractor<UIInteractor>();
            UIController uiController = uiInteractor.uiController;
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.5f);
                if (!uiController.IsActiveUIElement<UIPanelNotificationQuestComplete>())
                {
                    UIPanelNotificationQuestComplete panel =
                        uiInteractor.ShowElement<UIPanelNotificationQuestComplete>();
                    panel.Setup(quest);
                    break;
                }
            }
        }

        public void FinishQuest(Quest quest)
        {
            if (!quest.isCompleted)
            {
                throw new Exception("Cannot finish quest");
            }

            CommonAnalytics.LogDailyQuestRewardReceived(quest);
            this.GiveRewardForComplete(quest);
            this.DisableQuest(quest);
            this.SaveQuestIntoRepository(quest);
            this.OnQuestFinishedEvent?.Invoke(quest);
            //this.TryInflateFreshQuests();
        }

        private void GiveRewardForComplete(Quest quest)
        {
            var miniQuestInfo = (QuestInfoEcoClicker) quest.info;
            var rewardInfo = miniQuestInfo.rewardInfo;
            var reward = new Reward(rewardInfo);
            reward.Apply(this, false);
        }

        #endregion

        private void SaveQuestIntoRepository(Quest quest)
        {
            var entity = new MiniQuestEntity
            {
                id = quest.id,
                json = quest.state.GetStateJson()
            };
            this.repository.UpdateEntity(entity);
        }

        private void DeleteQuestFromRepository(Quest quest)
        {
            this.repository.DeleteEntity(quest.id);
        }

        public IEnumerator OnStartModernizationAsync()
        {
            this.ReinflateQuests();
            yield break;
        }

        public List<QuestInfoEcoClicker> GetNewQuests()
        {
            var absentQuestCount = this.config.maxActiveQuestCount - this.activeQuestMap.Count;

            var availableFreshQuests = this.GetAvailableFreshQuests();
            return this.GetRandomSet(availableFreshQuests, absentQuestCount);
        }

        public void ReinflateQuests()
        {
            var quests = this.activeQuestMap.Values.ToList();
            foreach (var quest in quests)
            {
                this.DisableQuest(quest);
            }

            this.TryInflateFreshQuests();
        }

        public Boolean TryDisableQuests()
        {
            var quests = this.activeQuestMap.Values.ToList();
            if (this.activeQuestMap.Count > 0)
            {
                foreach (var quest in quests)
                {
                    this.DisableQuest(quest);
                    this.DeleteQuest(quest);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasAnyCompletedQuest()
        {
            foreach (Quest quest in activeQuestMap.Values)
            {
                if (quest.isCompleted && !quest.isRewardTaken)
                    return true;
            }

            return false;
        }

        public void UnlockQuests(object sender)
        {
            this.isQuestsUnlocked = true;
            this.repository.SetIsUnlocked(true);
            this.UpdateLastResetTime(this);
            this.InflateActiveQuests();
            this.OnQuestUnlockedEvent?.Invoke(sender);
        }

        public bool HasActiveQuestOfType<T>() where T : QuestInfo {
            return activeQuestMap.Select(aQuest => aQuest.Value.info).OfType<T>().Any();
        }

        public bool AllActiveQuestsCompleted() {
            var activeQuests = GetActiveQuests();
            return activeQuests.All(quest => quest.isCompleted);
        }
    }
}