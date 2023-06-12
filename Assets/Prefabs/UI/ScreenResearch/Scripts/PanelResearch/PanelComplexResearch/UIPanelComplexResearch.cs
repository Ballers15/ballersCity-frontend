using System;
using SinSity.Meta.Rewards;
using SinSity.Services;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIPanelComplexResearch : UIPanelResearch, IBankStateWithoutNotification
    {
        protected override UIPanelResearchProperties properties
        {
            get { return this.complexProperties; }
        }

        [SerializeField]
        private UIPanelComplexResearchProperties complexProperties;

        public override void Setup(ResearchObject researchObject)
        {
            var researchObjectInfo = researchObject.info;
            var rewardInfo = researchObjectInfo.rewardInfo;
            if (!(rewardInfo is ComplexResearchRewardInfo complexResearchRewardInfo))
            {
                throw new Exception("Required complex reward info");
            }

            base.Setup(researchObject);
            var innerRewardInfoSet = complexResearchRewardInfo.innerRewardInfoSet;
            var widgets = this.complexProperties.widgetRewards;
            for (var i = 0; i < innerRewardInfoSet.Length; i++)
            {
                var innerRewardInfo = innerRewardInfoSet[i];
                var widgetReward = widgets[i];
                widgetReward.Setup(innerRewardInfo);
            }
        }

        protected override void OnStartBtnClick() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popupAdLoading = uiInteractor.ShowElement<UIPopupADLoading>();
            popupAdLoading.OnADResultsReceived += this.OnAdsResultsReceived;
            popupAdLoading.ShowAD("research_complex");
        }

        private void OnAdsResultsReceived(UIPopupADLoading popup, bool success, string error)
        {
            popup.OnADResultsReceived -= this.OnAdsResultsReceived;
            if (success)
                base.OnStartBtnClick();

            ResearchAnalytics.LogResearchStartedAds(this.researchObjectCurrent, success);
        }
        
        protected override void OnGetRewardBtnClick() {
            ComplexResearchRewardInfo complexResearchRewardInfo = researchObjectCurrent.info.rewardInfo as ComplexResearchRewardInfo;
            int count = complexResearchRewardInfo.innerRewardInfoSet.Length;

            var reward = new Reward(complexResearchRewardInfo);
            reward.Apply(this, false);

            for (int i = 0; i < count; i++) {
                Vector3 iconPosition = this.complexProperties.GetIconPosition(i);
                MakeFX(complexResearchRewardInfo.innerRewardInfoSet[i], iconPosition);
            }

            base.OnGetRewardBtnClick();
        }
    }
}