using System;
using System.Collections.Generic;
using SinSity.Domain;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIScreenResearch : UIScreen<UIScreenResearchProperties>
    {
        #region Const

        private const int REQUIRED_RESEARCH_OBJECT_COUNT = 4;

        #endregion

        private ResearchObjectDataInteractor dataInteractor;

        private ResearchObjectTimerInteractor timerInteractor;

        private ResearchObjectRewardInteractor rewardInteractor;

        private readonly Dictionary<string, IUIPanelResearch> panelsMap;

        public UIScreenResearch()
        {
            this.panelsMap = new Dictionary<string, IUIPanelResearch>();
        }

        protected override void Awake()
        {
            base.Awake();
            foreach (var panel in this.properties.panels)
            {
                var researchId = panel.researchId;
                this.panelsMap[researchId] = panel;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.HideInstantly();
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.dataInteractor = this.GetInteractor<ResearchObjectDataInteractor>();
            this.timerInteractor = this.GetInteractor<ResearchObjectTimerInteractor>();
            this.rewardInteractor = this.GetInteractor<ResearchObjectRewardInteractor>();
            this.InitPanelsMap();
        }

        private void InitPanelsMap()
        {
            var count = this.dataInteractor.GetResearchObjectsCount();
            if (count != REQUIRED_RESEARCH_OBJECT_COUNT)
            {
                throw new Exception($"Expected {REQUIRED_RESEARCH_OBJECT_COUNT} " +
                                    $"but actual {count} count of researchObjects");
            }

            foreach (var panel in this.panelsMap.Values)
            {
                var researchId = panel.researchId;
                var researchObject = this.dataInteractor.GetResearchObject(researchId);
                panel.Setup(researchObject);
                panel.OnStartBtnClickEvent.AddListener(this.OnStartResearchBtnClick);
                panel.OnGetRewardBtnClickEvent.AddListener(this.OnGetResearchRewardBtnClick);
            }
        }

        public override void Show()
        {
            base.Show();
            this.UpdateView();
            this.timerInteractor.OnResearchObjectRemainingSecondsChangedEvent +=
                this.OnResearchObjectRemainingSecondsChanged;
            this.timerInteractor.OnResearchObjectFinishedEvent += this.OnResearchObjectFinished;
        }

        private void UpdateView()
        {
            foreach (var panel in this.panelsMap.Values)
            {
                panel.Setup(panel.researchObjectCurrent);
            }
        }

        public override void Hide()
        {
            base.Hide();
            this.timerInteractor.OnResearchObjectRemainingSecondsChangedEvent -=
                this.OnResearchObjectRemainingSecondsChanged;
            this.timerInteractor.OnResearchObjectFinishedEvent -=
                this.OnResearchObjectFinished;
        }

        #region InteractorEvents

        private void OnResearchObjectRemainingSecondsChanged(ResearchObject researchObject)
        {
            var researchObjectInfo = researchObject.info;
            var id = researchObjectInfo.id;
            var panel = this.panelsMap[id];
            panel.UpdateTimerValue();
        }

        private void OnResearchObjectFinished(ResearchObject researchObject)
        {
            var researchObjectInfo = researchObject.info;
            var id = researchObjectInfo.id;
            var panel = this.panelsMap[id];
            panel.SetStateAwaitingReward();
        }

        #endregion

        #region ClickEvents

        private void OnGetResearchRewardBtnClick(IUIPanelResearch panel)
        {
            var researchObject = panel.researchObjectCurrent;
            this.rewardInteractor.ReceiveReward(this, researchObject);
            panel.SetStateCanStart();
        }

        private void OnStartResearchBtnClick(IUIPanelResearch panel)
        {
            var researchObject = panel.researchObjectCurrent;
            if (this.timerInteractor.CanLaunchResearch(researchObject))
            {
                this.timerInteractor.LaunchResearchObject(researchObject);
                panel.SetStateTimerWork();
                return;
            }

//            if (Bank.softCurrencyCount < researchObject.state.price &&
//                panel is UIPanelSimpleResearch simplePanel)
//            {
//                simplePanel.ShowNotEnoughCurrencyFX();
//            }
        }
        
        #endregion
    }
}