using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Domain;
using Spine;
using UnityEngine;
using VavilichevGD.UI;
using AnimationState = Spine.AnimationState;

namespace SinSity.UI
{
    public sealed class UIGemTree : UIWidget<UIGemTreeProperties>
    {
        #region Const

        private const string GROW_ANIMATION_PREFIX = "Growthing_stage";
        

        private const string IDLE_ANIMATION_PREFIX = "Idle_stage";

        #endregion

        private GemTreeBranchDataInteractor gemTreeBranchDataInteractor;

        private GemTreeStateInteractor gemTreeStateInteractor;

        private GemTreeTimerInteractor gemTreeTimerInteractor;

        private readonly Dictionary<string, UIGemBranch> uiBranchMap;

        private AnimationState animationState;

        private bool isAnimationEnabled;

        public UIGemTree()
        {
            this.uiBranchMap = new Dictionary<string, UIGemBranch>();
        }

        protected override void Awake()
        {
            base.Awake();
            this.animationState = this.properties.skeletonGraphic.AnimationState;
            var uiGemBranches = this.properties.gemBranches;
            foreach (var uiBranch in uiGemBranches) {
                var branchId = uiBranch.id;
                this.uiBranchMap[branchId] = uiBranch;
            }
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.gemTreeStateInteractor = this.GetInteractor<GemTreeStateInteractor>();
            this.gemTreeTimerInteractor = this.GetInteractor<GemTreeTimerInteractor>();
            this.gemTreeBranchDataInteractor = this.GetInteractor<GemTreeBranchDataInteractor>();
        }

        public void OnShow()
        {
            this.gemTreeTimerInteractor.OnBranchObjectRemainingSecondsChangedEvent += this.OnGemBranchSecondsChanged;
            this.gemTreeTimerInteractor.OnBranchObjectReadyEvent += this.OnGemBranchReady;
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent += this.OnGemBranchRewardReceived;
            this.SubscribeOnBranchClickedEvents();
            this.SetupState();
        }

        private void SubscribeOnBranchClickedEvents() {
            var uiGemBranches = this.properties.gemBranches;
            foreach (var uiBranch in uiGemBranches)
                uiBranch.OnClickEvent += this.OnGemBranchClicked;
        }

       

        private void SetupState()
        {
            var currentLevelNumber = this.gemTreeStateInteractor.currentLevelIndex + 1;
            if (!this.gemTreeStateInteractor.isTreeViewed)
            {
                this.gemTreeStateInteractor.ViewTree(this);
                this.ShowGrowAnimation(currentLevelNumber);
            }
            else
            {
                this.ShowIdleAnimation(currentLevelNumber);
                this.SetupBranches();
            }
        }

        private void SetupBranches()
        {
            var branchObjects = this.gemTreeBranchDataInteractor.GetBranchObjects();
            foreach (var branchObject in branchObjects)
            {
                var branchId = branchObject.info.id;
                var uiGemBranch = this.uiBranchMap[branchId];
                uiGemBranch.Setup(branchObject);
            }
        }

        public void OnHide()
        {
            this.gemTreeTimerInteractor.OnBranchObjectRemainingSecondsChangedEvent -= this.OnGemBranchSecondsChanged;
            this.gemTreeTimerInteractor.OnBranchObjectReadyEvent -= this.OnGemBranchReady;
            this.gemTreeTimerInteractor.OnBranchObjectRewardReceivedEvent -= this.OnGemBranchRewardReceived;
            this.UnsubscribeOnBranchClickedEvents();
        }
        
        private void UnsubscribeOnBranchClickedEvents() {
            var uiGemBranches = this.properties.gemBranches;
            foreach (var uiBranch in uiGemBranches)
                uiBranch.OnClickEvent -= this.OnGemBranchClicked;
        }

        public IEnumerator AnimateLevelUp(int levelNumber)
        {
            this.isAnimationEnabled = true;
            this.ShowGrowAnimation(levelNumber);
            while (this.isAnimationEnabled)
            {
                yield return null;
            }
        }

        private void OnCompleteLevelUpAnimation(TrackEntry trackentry)
        {
            trackentry.Complete -= this.OnCompleteLevelUpAnimation;
            var newLevel = this.gemTreeStateInteractor.currentLevelIndex + 1;
            var branchId = newLevel.ToString();
            var uiBranch = this.uiBranchMap[branchId];
            var branchObject = this.gemTreeBranchDataInteractor.GetBranchObject(branchId);
            uiBranch.OnGemOpened(branchObject);
            this.isAnimationEnabled = false;
        }

        private void ShowGrowAnimation(int levelNumber) {
            Debug.Log($"<color=green>ShowGrowAnimation_{GROW_ANIMATION_PREFIX}{levelNumber}</color>");
            this.animationState.ClearTrack(0);
            var growAnim = this.animationState.SetAnimation(0, $"{GROW_ANIMATION_PREFIX}{levelNumber}", false);
            growAnim.Complete += this.OnCompleteLevelUpAnimation;
            var growAnimTrackTime = growAnim.TrackTime;
            this.animationState.AddAnimation(0, $"{IDLE_ANIMATION_PREFIX}{levelNumber}", true, growAnimTrackTime);
        }

        private void ShowIdleAnimation(int levelNumber)
        {
            Debug.Log("<color=green>ShowIdleAnimation</color>");
            this.animationState.ClearTrack(0);
            this.animationState.SetAnimation(0, $"{IDLE_ANIMATION_PREFIX}{levelNumber}", true);
        }

        #region ClickEvents

        private void OnGemBranchClicked(UIGemBranch branch) {
            var branchId = branch.id;
            var branchObject = this.gemTreeBranchDataInteractor.GetBranchObject(branchId);
            if (this.gemTreeTimerInteractor.CanCollectGem(branchObject))
                this.gemTreeTimerInteractor.CollectGem(this, branchObject);
        }

        #endregion

        #region InteractorEvents

        private void OnGemBranchRewardReceived(object sender, GemBranchObject gemBranch, GemBranchReward reward)
        {
            var branchId = gemBranch.info.id;
            var uiBranch = this.uiBranchMap[branchId];
            uiBranch.OnGemReceived(gemBranch, reward);
        }

        private void OnGemBranchReady(object sender, GemBranchObject gemBranch)
        {
            var branchId = gemBranch.info.id;
            var branch = this.uiBranchMap[branchId];
            branch.OnGemReady(gemBranch);
        }

        private void OnGemBranchSecondsChanged(object sender, GemBranchObject gemBranch)
        {
            var branchId = gemBranch.info.id;
            var branch = this.uiBranchMap[branchId];
            branch.OnGemUpdated(gemBranch);
        }

        #endregion
    }
}