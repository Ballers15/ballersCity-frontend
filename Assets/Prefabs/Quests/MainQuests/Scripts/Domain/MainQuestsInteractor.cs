using System;
using System.Collections;
using SinSity.Core;
using SinSity.Domain;
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
using Object = UnityEngine.Object;

namespace SinSity.Meta.Quests
{
    public sealed class MainQuestsInteractor : Interactor, ISaveListenerInteractor
    {
        #region Const

        private const string PIPELINE_NAME = "MainQuestsPipeline";

        #endregion

        #region Event

        public event Action<object> OnQuestsUnlockedEvent;

        public event Action<Quest> OnQuestChangedEvent;

        public event Action OnAllQuestsCompletedEvent;

        public event Action<Quest> OnQuestFinishedEvent;

        #endregion

        private new MainQuestRepository repository;

        private MainQuestsPipeline questsPipeline;

        public Quest currentQuest { get; private set; }

        public bool isAllQuestCompleted { get; private set; }

        public bool isQuestsUnlocked { get; private set; }
        
        public override bool onCreateInstantly { get; } = true;


        #region Initialize

        protected override void Initialize() {
            base.Initialize();
            this.repository = this.GetRepository<MainQuestRepository>();
            this.questsPipeline = Object.Instantiate(Resources.Load<MainQuestsPipeline>(PIPELINE_NAME));
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
            if (this.isQuestsUnlocked)
            {
                this.InitSavedQuestProgression();
            }
        }


        private void InitSavedQuestProgression()
        {
            if (!this.repository.HasStatistics(out var questStatistics) ||
                questStatistics.currentQuestId == "" ||
                questStatistics.currentQuestId == null)
            {
                var firstQuestInfo = this.questsPipeline.GetQuest(0);
                var defaultState = firstQuestInfo.CreateStateDefault();
                var json = defaultState.GetStateJson();
                questStatistics = new MainQuestStatisitcs
                {
                    currentQuestId = firstQuestInfo.id,
                    currentQuestJson = json,
                    isAllQuestsCompleted = false
                };
                this.repository.UpdateStatistics(questStatistics);
                CommonAnalytics.LogMainQuestStarted(firstQuestInfo.id, true);
            }

            if (questStatistics.isAllQuestsCompleted)
            {
                this.isAllQuestCompleted = true;
                this.currentQuest = null;
            }
            else
            {
                this.isAllQuestCompleted = false;
                this.InflateSavedQuest(questStatistics);
            }
        }

        private void InflateSavedQuest(MainQuestStatisitcs questStatistics)
        {
            var questId = questStatistics.currentQuestId;
            if (!this.questsPipeline.HasQuest(questId))
            {
                this.TryInflateNextQuest();
                return;
            }

            var questInfo = this.questsPipeline.GetQuest(questId);
            var questJson = questStatistics.currentQuestJson;
            var questState = questInfo.CreateState(questJson);
            var currentQuest = new Quest(questInfo, questState);
            this.currentQuest = currentQuest;
            this.EnableQuest(currentQuest);
        }

        #endregion

        #region TryInflateNextQuest

        private void TryInflateNextQuest()
        {
            var currentQuestId = this.currentQuest.id;
            while (this.questsPipeline.HasNextQuest(currentQuestId, out var nextQuestInfo))
            {
                currentQuestId = nextQuestInfo.id;
                var currentQuest = (QuestInfoEcoClicker) nextQuestInfo;
                if (!currentQuest.CanCreateQuest())
                {
                    Debug.Log("Cannot inflate next quest");
                    continue;
                }
                
                this.InflateNextQuest(nextQuestInfo);
                return;
            }

            this.FinishQuestList();
        }

        private void InflateNextQuest(QuestInfo nextQuestInfo)
        {
            var freshQuestState = nextQuestInfo.CreateStateDefault();
            var nextQuest = new Quest(nextQuestInfo, freshQuestState);
            this.currentQuest = nextQuest;
            this.OnSave();
            this.EnableQuest(nextQuest);
            this.OnQuestChangedEvent?.Invoke(nextQuest);
            CommonAnalytics.LogMainQuestStarted(nextQuest.id, true);
        }

        private void FinishQuestList()
        {
            this.isAllQuestCompleted = true;
            this.OnAllQuestsCompletedEvent?.Invoke();
            this.OnSave();
        }

        #endregion

        private void EnableQuest(Quest quest)
        {
            quest.OnStateChanged += this.OnQuestStateChanged;
            quest.StartQuest();
            OnQuestChangedEvent?.Invoke(quest);
        }

        private void DisableQuest(Quest quest)
        {
            quest.OnStateChanged -= this.OnQuestStateChanged;
        }

        #region QuestEvent

        private void OnQuestStateChanged(Quest quest, QuestState newstate)
        {
            this.OnSave();
            this.OnQuestChangedEvent?.Invoke(quest);

            if (newstate.isCompleted)
            {
                this.TryToShowNotification(quest);
                if (newstate.completeTimes == 1)
                {
                    CommonAnalytics.LogMainQuestEnd(this.currentQuest.id);
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

        #endregion

        public void OnSave()
        {
            var questStatistics = new MainQuestStatisitcs
            {
                currentQuestId = this.currentQuest?.id,
                currentQuestJson = this.currentQuest?.state?.GetStateJson(),
                isAllQuestsCompleted = this.isAllQuestCompleted
            };
            this.repository.UpdateStatistics(questStatistics);
        }

        #region Finish Quest

        public void FinishCurrentQuest()
        {
            if (!this.currentQuest.isCompleted)
            {
                throw new Exception("Cannot finish quest");
            }
            
            CommonAnalytics.LogMainQuestRewardReceived(this.currentQuest);
            this.GiveRewardForComplete(this.currentQuest);
            this.DisableQuest(this.currentQuest);
            this.OnQuestFinishedEvent?.Invoke(this.currentQuest);
            this.TryInflateNextQuest();
        }

        private void GiveRewardForComplete(Quest quest)
        {
            var mainQuestInfo = (QuestInfoEcoClicker) quest.info;
            var rewardInfo = mainQuestInfo.rewardInfo;
            var reward = new Reward(rewardInfo);
            reward.Apply(this, false);
        }

        #endregion

        public bool HasCompletedQuest() {
            if (currentQuest != null) return (currentQuest.isCompleted && !currentQuest.isRewardTaken) ? true : false;
            else return false;
        }

        public void UnlockQuests(object sender)
        {
            this.isQuestsUnlocked = true;
            this.repository.SetIsUnlocked(true);
            this.InitSavedQuestProgression();
            this.OnQuestsUnlockedEvent?.Invoke(sender);
        }
    }
}