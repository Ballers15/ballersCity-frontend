using SinSity.Repo;
using Orego.Util;
using SinSity.Core;
using SinSity.Meta.Quests;
using SinSity.Meta.Rewards;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetMainQuest : UIWidgetAnim<UIWidgetMainQuestProperties>
    {
        private MainQuestsInteractor mainQuestsInteractor;
        private static readonly int TRIGGER_NEW_QUEST = Animator.StringToHash("new_quest");

        private void OnEnable()
        {
            this.mainQuestsInteractor = Game.GetInteractor<MainQuestsInteractor>();
            this.mainQuestsInteractor.OnAllQuestsCompletedEvent += this.OnAllMainQuestsCompleted;
            this.mainQuestsInteractor.OnQuestChangedEvent += this.MainQuestChangedEvent;
            Localization.OnLanguageChanged += OnLanguageChanged;
            
            if (Game.isInitialized)
                this.RefreshQuestInfo();
        }

        private void OnDisable()
        {
            this.mainQuestsInteractor.OnAllQuestsCompletedEvent -= this.OnAllMainQuestsCompleted;
            this.mainQuestsInteractor.OnQuestChangedEvent -= this.MainQuestChangedEvent;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }

        #region OnRefresh

        public void OnRefresh()
        {
            if (this.mainQuestsInteractor.isAllQuestCompleted)
            {
                this.RefreshAllQuestsCompleted();
            }
            else
            {
                this.RefreshQuestInfo();
            }
        }

        #endregion

        #region Refresh

        private void EnableButton()
        {
            this.properties.imageTakeRewardMQuest.sprite = this.properties.spriteTakeRewEnableMQuest;
            this.properties.imageTakeRewardHighlMQuest.sprite = this.properties.spriteTakeRewEnableHighlMQuest;
        }

        private void DisableButton()
        {
            this.properties.imageTakeRewardMQuest.sprite = this.properties.spriteTakeRewardDisableMQuest;
            this.properties.imageTakeRewardHighlMQuest.sprite = this.properties.spriteTakeRewDisableHighlMQuest;
        }

        private void RefreshQuestInfo()
        {
            this.properties.buttonReceiveRewardMQuest.onClick.RemoveAllListeners();

            var quest = this.mainQuestsInteractor.currentQuest;
            var questInfo = quest.GetInfo<QuestInfoEcoClicker>();
            this.properties.textTaskMQuest.text = GetDescription(quest);
            
            var rewardInfo = (RewardInfoEcoClicker) questInfo.rewardInfo;
            if (rewardInfo == null) return;
            this.properties.rewardIconMQuest.sprite = rewardInfo.GetSpriteIcon();
            this.properties.textRewardCountMQuest.text = rewardInfo.GetCountToString();
            
            if (!quest.isCompleted)
            {
                var textProgress = this.properties.textProgressMQuest;
                textProgress.text = quest.progressDescription;
                var progressBar = this.properties.progressBarMQuest;
                progressBar.SetValue(quest.progressNormalized);
                this.properties.progressBarMQuest.imageFiller.color = this.properties.colorFillerDisableMQuest;
                this.DisableButton();
            }
            else
            {
                var textProgress = this.properties.textProgressMQuest;
                textProgress.text = quest.progressDescription;
                var progressBar = this.properties.progressBarMQuest;
                progressBar.SetValue(quest.progressNormalized);
                this.properties.progressBarMQuest.imageFiller.color = this.properties.colorFillerEnableMQuest;
                this.EnableButton();
                this.properties.buttonReceiveRewardMQuest.onClick.AddListener(this.OnFinishQuestClick);
            }
        }

        private string GetDescription(Quest quest) {
            QuestStateEcoClicker questState = quest.GetState<QuestStateEcoClicker>();
            return questState.GetDescription(quest);
        }

        private void RefreshAllQuestsCompleted()
        {
            this.properties.textProgressMQuest.SetInvisible();
            this.properties.textTaskMQuest.SetInvisible();
            this.properties.progressBarMQuest.SetInvisible();
        }

        #endregion

        #region ClickEvents

        private void OnFinishQuestClick() {
            Quest mainQuest = mainQuestsInteractor.currentQuest;
            QuestInfoEcoClicker questInfoEcoClicker = mainQuest.GetInfo<QuestInfoEcoClicker>();

            if (questInfoEcoClicker.rewardInfo is RewardInfoCase rewardInfoCase) {
                this.MakeFXCase(rewardInfoCase.caseInfo, rewardInfoCase.count);
            }
            else if (questInfoEcoClicker.rewardInfo is RewardInfoHardCurrency rewardInfoHardCurrency) {
                this.MakeFXGems(rewardInfoHardCurrency);
            }
            
            mainQuestsInteractor.FinishCurrentQuest();
            SetTrigger(TRIGGER_NEW_QUEST);
            
            TutorialAnalytics.LogReceiveFirstMainQuestReward();
            SFX.PlaySFX(this.properties.audioClipFinishMQuest);
        }

        private void MakeFXCase(ProductInfoCase infoCase, int count) {
            Product product = Shop.GetProduct(infoCase.GetId());
            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(this.properties.rewardIconMQuest.transform.position);
            FXCasesGenerator.MakeFX(objectEcoClicker, product, count);
        }

        private void MakeFXGems(RewardInfoHardCurrency rewardInfoHardCurrency) {
            var objectEcoClicker = new UIObjectEcoClicker(this.properties.rewardIconMQuest.transform.position);
            FXGemsGenerator.MakeFX(objectEcoClicker, rewardInfoHardCurrency.value);
        }
        
        

        #endregion

        #region QuestEvents

        private void MainQuestChangedEvent(Quest mainQuest)
        {
            this.RefreshQuestInfo();
        }

        private void OnAllMainQuestsCompleted()
        {
            this.RefreshAllQuestsCompleted();
        }
        
        private void OnLanguageChanged() {
            RefreshQuestInfo();
        }

        #endregion
    }
}