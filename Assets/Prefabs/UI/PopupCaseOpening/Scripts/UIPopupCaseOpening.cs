using System;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Meta.Rewards;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIPopupCaseOpening : UIDialogueAnim<UIPopupCaseOpeningProperties, UIDialogueArgs>
    {
        #region Const

        private const string GOLD_CASE_PREF_NAME = "CaseOpeningGold";

        private const string SIMPLE_CASE_PREF_NAME = "CaseOpeningSimple";

        private const string STEEL_CASE_PREF_NAME = "CaseOpeningSteel";

        #endregion

        private ProductInfoCase currentCase;

        private IEnumerable<RewardInfoEcoClicker> rewardInfoSet;
        private static readonly int TRIGGER_HIDE_CASE = Animator.StringToHash("hide_case");

        public override void Initialize()
        {
            this.gameObject.SetActive(false);
        }

        public void Setup(ProductInfoCase infoCase, IEnumerable<RewardInfoEcoClicker> rewardInfoSet)
        {
            this.currentCase = infoCase;
            this.rewardInfoSet = rewardInfoSet;
            this.SetupCase();
            CommonAnalytics.LogCaseOpened(infoCase.GetId(), rewardInfoSet);
        }

        private void SetupCase()
        {
            this.CleanContainer();
            var pref = this.GetCasePref();
            var createdItem = Instantiate(pref, this.properties.container);
            createdItem.OnCaseOpened += OnCaseOpened;
            createdItem.OnCaseOpenedAnimaionOver += OnCaseOpenedAnimaionOver;
        }
        
        private void OnCaseOpened(UIItemCaseOpening uicase) {
            uicase.OnCaseOpened -= OnCaseOpened;
            ProductInfoCase.NotifyOnProductCaseOpened(this.currentCase);
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var rewardSummary = uiInteractor.ShowElement<UIPopupRewardSummary>();
            rewardSummary.Setup(this.rewardInfoSet);
            rewardSummary.OnDialogueResults += OnDialogueResults;
        }

        private void OnCaseOpenedAnimaionOver(UIItemCaseOpening uicase) {
            uicase.OnCaseOpenedAnimaionOver -= OnCaseOpenedAnimaionOver;
            this.HideCase();
        }

        private void HideCase() {
            this.SetTrigger(TRIGGER_HIDE_CASE);
        }
        
        private void OnDialogueResults(UIPopupArgs e) {
            UIPopupRewardSummary popupRewardSummary = e.uiElement as UIPopupRewardSummary;
            popupRewardSummary.OnDialogueResults -= OnDialogueResults;
            this.HideInstantly();
        }

        private void CleanContainer()
        {
            foreach (Transform child in this.properties.container)
            {
                Destroy(child.gameObject);
            }
        }

        private UIItemCaseOpening GetCasePref()
        {
            if (this.currentCase is ProductInfoSimpleCase)
            {
                return Resources.Load<UIItemCaseOpening>(SIMPLE_CASE_PREF_NAME);
            }

            if (this.currentCase is ProductInfoSteelCase)
            {
                return Resources.Load<UIItemCaseOpening>(STEEL_CASE_PREF_NAME);
            }

            if (this.currentCase is ProductInfoGoldCase)
            {
                return Resources.Load<UIItemCaseOpening>(GOLD_CASE_PREF_NAME);
            }

            throw new Exception("Case not found!");
        }
    }
}