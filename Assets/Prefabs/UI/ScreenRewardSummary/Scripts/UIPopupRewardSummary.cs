using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Services;
using SinSity.Core;
using SinSity.Meta.Rewards;
using SinSity.Monetization;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPopupRewardSummary : UIPopupAnim<UIScreenRewardSummaryProperties, UIPopupArgs> {
        public delegate void OpenNextCaseBtnClickHandler();
        public static event OpenNextCaseBtnClickHandler OnOpenNextCaseBtnClicked;
        
        private Transform cardLayoutTransform;

        private List<UICardRewardSummary> uiRewardCards;

        private Coroutine showBtnsRoutine;
        private Product nextCase;
        private int currentShowingCardIndex = -1;

        protected override void Awake()
        {
            base.Awake();
            this.uiRewardCards = new List<UICardRewardSummary>();
            this.cardLayoutTransform = this.properties.cardLayout.transform;
        }

        private void OnEnable()
        {
            this.properties.buttonOk.onClick.AddListener(
                this.OnOkClick
            );
            this.properties.buttonOpenNext.onClick.AddListener(
                this.OpenNextCaseClick
            );
            
            this.properties.btnHurryUp.onClick.AddListener(OnHurryUpBtnClick);
            this.properties.DeactivateBtns();
        }

        public void Setup(IEnumerable<RewardInfoEcoClicker> rewardInfoSet)
        {
            foreach (var uiCard in this.uiRewardCards)
            {
                Destroy(uiCard.gameObject);
            }

            this.uiRewardCards.Clear();
            var cardRewardSummaryPref = this.properties.cardRewardSummaryPref;
            foreach (var rewardInfo in rewardInfoSet)
            {
                var uiRewardCard = Instantiate(cardRewardSummaryPref, this.cardLayoutTransform);
                uiRewardCard.Setup(rewardInfo);
                this.uiRewardCards.Add(uiRewardCard);
            }
        }
        
        private void Handle_AppearAnimationIsOver() {
            currentShowingCardIndex = 0;
            ShowCard(currentShowingCardIndex);
        }

        private void ShowCard(int index) {
            if (index >= uiRewardCards.Count) {
                if (this.showBtnsRoutine != null) {
                    StopCoroutine(this.showBtnsRoutine);
                    this.showBtnsRoutine = null;
                }
                
                this.showBtnsRoutine = StartCoroutine(ShowBtnsRoutine());
                return;
            }
            
            uiRewardCards[index].ShowCard();
            uiRewardCards[index].OnAppearingOver += delegate
            {
                currentShowingCardIndex = index + 1;
                ShowCard(currentShowingCardIndex);
            };
        }

        private IEnumerator ShowBtnsRoutine() {
            yield return new WaitForSecondsRealtime(0.5f);
            var hasNextPriorityCase = CaseProductUtils.HasNextPriorityCase(out this.nextCase);
            if (hasNextPriorityCase)
                this.properties.ActivateBtns();
            else 
                this.properties.buttonOk.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (this.showBtnsRoutine != null) {
                StopCoroutine(this.showBtnsRoutine);
                this.showBtnsRoutine = null;
            }
            
            this.properties.StopAllCoroutines();
            this.properties.buttonOk.onClick.RemoveAllListeners();
            this.properties.buttonOpenNext.onClick.RemoveAllListeners();
            this.properties.btnHurryUp.onClick.RemoveListener(OnHurryUpBtnClick);
        }

        #region ClickEvents

        private void OnOkClick()
        {
            this.Hide();
            NotifyAboutResults(new UIPopupArgs(this));
        }


        private void OpenNextCaseClick()
        {
            if (this.nextCase == null)
            {
                throw new Exception("Next case is absent!");
            }
            
            SFX.PlaySFX(this.properties.audioClipOpenNextClick);
            var nextCaseState = this.nextCase.GetState<ProductStateCase>();
            nextCaseState.SpendCase();
            var nextCaseInfo = this.nextCase.GetInfo<ProductInfoCase>();
            var rewardInfoSet = nextCaseInfo.GetRewardInfoSet();
            ProductStateCase.ApplyRewards(rewardInfoSet);
            
            this.Hide();
            NotifyAboutResults(new UIPopupArgs(this));

            
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popupCaseOpening = uiInteractor.ShowElement<UIPopupCaseOpening>();
            popupCaseOpening.Setup(nextCaseInfo, rewardInfoSet);
            
            OnOpenNextCaseBtnClicked?.Invoke();
        }
        
        private void OnHurryUpBtnClick() {
            if (currentShowingCardIndex < 0 || currentShowingCardIndex >= uiRewardCards.Count)
                return;
            
            uiRewardCards[currentShowingCardIndex].SpeedUp();
        }

        #endregion
    }
}