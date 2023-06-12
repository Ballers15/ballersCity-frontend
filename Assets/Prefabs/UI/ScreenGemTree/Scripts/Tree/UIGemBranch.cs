using System;
using Orego.Util;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIGemBranch : UIWidget<UIGemBranchProperties>
    {
        #region Event

        public delegate void ClickEventHandler(UIGemBranch uiGemBranch);
        public event ClickEventHandler OnClickEvent;

        #endregion

        public string id
        {
            get { return this.properties.id; }
        }

        private void OnEnable() {
            this.properties.gemFruit.OnClickEvent += OnGemFruitClick;
        }

        private void OnDisable() {
            this.properties.gemFruit.OnClickEvent -= OnGemFruitClick;
        }

        public void Setup(GemBranchObject branchObject)
        {
            var state = branchObject.state;
            var gemFruit = this.properties.gemFruit;
            gemFruit.Activate(state.isReady, state.isOpened);
        }

        public void OnGemOpened(GemBranchObject gemBranch) {
            this.properties.gemFruit.ActivateFirstTime();
        }

        public void OnGemReceived(GemBranchObject gemBranch, GemBranchReward reward) {
            this.MakeFX(reward);
        }

        private void MakeFX( GemBranchReward reward) {
            UIObjectEcoClicker uiObjectEcoClicker = new UIObjectEcoClicker(this.properties.gemFruit.transform.position);
            FXGemsGenerator.MakeFX(uiObjectEcoClicker, reward.gemsReward, true);

            if (reward.jackpot) {
                string textJackpot = $"{Localization.GetTranslation("ID_JACKPOT")} +{reward.gemsReward}";
                FXTextGenerator.MakeFX(uiObjectEcoClicker, textJackpot);
            }
        }

        public void OnGemUpdated(GemBranchObject gemBranch)
        {
            Debug.Log($"<color=orange> {gemBranch.info.id}: {gemBranch.state.remainingTimeSec}</color>");
        }

        public void OnGemReady(GemBranchObject gemBranch) {
            this.properties.gemFruit.ActivateSecondTime();
        }

        private void OnGemFruitClick() {
            this.OnClickEvent?.Invoke(this);
        }
    }
}