using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Quests;
using VavilichevGD.UI;
using System;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta.Quests;
using SinSity.Meta.Rewards;
using SinSity.Services;

namespace SinSity.UI
{
    public sealed class UIWidgetMiniQuest : UIWidgetAnim<UIWidgetMiniQuestProperties>
    {
        private Quest currentQuest;
        private MiniQuestInteractor interactor;
        private static readonly int TRIGGER_NEW_QUEST = Animator.StringToHash("new_quest");

        public static event Action OnMiniQuestTakeRewardEvent;

        public void Setup(Quest quest)
        {
            this.currentQuest = quest;
            this.properties.buttonTakeReward.onClick.RemoveAllListeners();

            var questInfo = quest.GetInfo<QuestInfoEcoClicker>();
            this.properties.textTask.text = GetTranslatedDescription(quest);
            this.properties.textProgress.text = quest.progressDescription;
            this.properties.progressBar.SetValue(quest.progressNormalized);
            var rewardInfo = (RewardInfoEcoClicker) questInfo.rewardInfo;
            this.properties.rewardIcon.sprite = rewardInfo.GetSpriteIcon();
            SetRewardCount(rewardInfo.GetCountToString());

#if DEBUG
            // Debug.Log($"QUEST COMPLETED: {quest.id}" + quest.isCompleted);
#endif
            if (quest.isCompleted && !quest.isRewardTaken)
            {
                this.properties.buttonTakeReward.onClick.AddListener(this.OnTakeRewardClick);
                this.properties.imageTakeReward.sprite = this.properties.spriteTakeRewardEnable;
                this.properties.imageTakeRewardHighlight.sprite = this.properties.spriteTakeRewardEnableHighlight;
                this.properties.progressBar.imageFiller.color = this.properties.colorFillerEnable;
            }
            else
            {
                this.properties.imageTakeReward.sprite = this.properties.spriteTakeRewardDisable;
                this.properties.imageTakeRewardHighlight.sprite = this.properties.spriteTakeRewardDisableHighlight;
                this.properties.progressBar.imageFiller.color = this.properties.colorFillerDisable;
            }
            if(quest.isCompleted && quest.isRewardTaken)
            {
                this.ShowCompleted();
            }
        }

        private string GetTranslatedDescription(Quest quest) {
            QuestStateEcoClicker state = quest.GetState<QuestStateEcoClicker>();
            return state.GetDescription(quest);
        }
        
        private void SetRewardCount(string countText)
        {
            this.properties.textCount.text = countText;
            //RectTransform parentRectTransform = properties.textCount.transform.parent as RectTransform;
            //parentRectTransform.RecalculateWithHorizontalFitterInside(ContentSizeFitter.FitMode.PreferredSize);
        }

        private void OnTakeRewardClick() {
            ShowFX(currentQuest);
            var miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
            this.currentQuest.state.MarkThatRewardIsTaken();
            miniQuestInteractor.FinishQuest(this.currentQuest);
            ShowAnimation();

            this.properties.buttonTakeReward.onClick.RemoveListener(this.OnTakeRewardClick);
            this.properties.imageTakeReward.sprite = this.properties.spriteTakeRewardDisable;
            this.properties.imageTakeRewardHighlight.sprite = this.properties.spriteTakeRewardDisableHighlight;
            this.properties.progressBar.imageFiller.color = this.properties.colorFillerDisable;
            
            this.ShowCompleted();

            TutorialAnalytics.LogReceiveFirstMiniQuestReward();
            SFX.PlaySFX(this.properties.audioClipFinishMiniQuest);
            OnMiniQuestTakeRewardEvent?.Invoke();
        }

        private void ShowFX(Quest quest) {
            RewardInfoHardCurrency gemsInfo = quest.GetInfo<QuestInfoEcoClicker>().rewardInfo as RewardInfoHardCurrency;
            int gemsCount = gemsInfo.value;

            UIObjectEcoClicker objectEcoClicker = new UIObjectEcoClicker(this.properties.buttonTakeReward.transform.position);
            FXGemsGenerator.MakeFX(objectEcoClicker, gemsCount);
        }

        private void ShowAnimation() {
            SetTrigger(TRIGGER_NEW_QUEST);
        }
        
        private void ShowCompleted()
        {
            this.properties.progressBar.SetInvisible();
            this.properties.buttonTakeReward.SetInvisible();
            this.properties.textTask.SetInvisible();
            this.properties.textProgress.SetInvisible();
            this.properties.Complete.SetActive(true);
        }
        
        private void HideCompleted()
        {
            this.properties.progressBar.SetVisible();
            this.properties.buttonTakeReward.SetVisible();
            this.properties.textTask.SetVisible();
            this.properties.textProgress.SetVisible();
            this.properties.Complete.SetActive(false);
        }

        private void OnEnable()
        {
            interactor = GetInteractor<MiniQuestInteractor>();
            interactor.OnResetTimeChangedEvent += OnResetTimeChanged;
            Localization.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnResetTimeChanged(object sender)
        {
            this.HideCompleted();
        }
        
        private void OnLanguageChanged() {
            if (currentQuest != null)
                Setup(currentQuest);
        }

        private void OnDisable() {
            interactor.OnResetTimeChangedEvent -= OnResetTimeChanged;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}