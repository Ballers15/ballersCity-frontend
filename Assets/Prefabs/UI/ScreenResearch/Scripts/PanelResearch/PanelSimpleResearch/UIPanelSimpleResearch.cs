using SinSity.Domain;
using SinSity.Meta.Rewards;
using SinSity.Tools;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;

namespace SinSity.UI
{
    public sealed class UIPanelSimpleResearch : UIPanelResearch, IBankStateWithoutNotification
    {
        protected override UIPanelResearchProperties properties
        {
            get { return this.simpleProperties; }
        }

        [SerializeField]
        private UIPanelSimpleResearchProperties simpleProperties;

        private UIWidgetPanelResearchPriceButton priceButtonCurrent;

        public override void Setup(ResearchObject researchObject)
        {
            base.Setup(researchObject);
            var researchObjectInfo = researchObject.info;
            this.simpleProperties.SetIcon(researchObjectInfo.GetIcon());
            this.simpleProperties.SetCountText(researchObjectInfo.GetRewardCountToString());
        }

        public override void SetStateCanStart()
        {
            priceButtonCurrent = (UIWidgetPanelResearchPriceButton) this.properties.CreateWidgetButtonStart();
            priceButtonCurrent.SetupPrice(this.researchObjectCurrent.state.price);
            priceButtonCurrent.AddListener(this.OnStartBtnClick);
        }

        protected override void OnStartBtnClick()
        {
            base.OnStartBtnClick();
            ResearchAnalytics.LogResearchStartedNoAds(this.researchObjectCurrent);
        }

        protected override void OnGetRewardBtnClick() {
            RewardInfoEcoClicker rewardInfoEcoClicker = this.researchObjectCurrent.info.rewardInfo;
            var reward = new Reward(rewardInfoEcoClicker);
            reward.Apply(this, false);

            Vector3 position = this.properties.GetRewardIconPosition();
            MakeFX(rewardInfoEcoClicker, position);
            base.OnGetRewardBtnClick();
        }

        protected override void OnEnable() {
            base.OnEnable();
            Localization.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged() {
            if (priceButtonCurrent)
                priceButtonCurrent.SetupPrice(this.researchObjectCurrent.state.price);
        }

        protected override void OnDisable() {
            base.OnDisable();
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}