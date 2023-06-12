using SinSity.UI;
using System;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorRenovationInfo",
        menuName = "Domain/Hint/HintInspectorRenovationInfo"
    )]
    public sealed class HintInspectorRenovationInfo : HintStateInspector<HintStateRenovationInfo>
    {
        private ModernizationInteractor modernizationInteractor;

        public bool IsReadyForView()
        {
            return this.state.isReady;
        }

        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutRenovationInfoViewed()
        {
            if (!this.IsReadyForView())
            {
                throw new Exception("Renovation info has not ready yet!");
            }

            if (this.IsViewed())
            {
                throw new Exception("Renovation info has already viewed!");
            }

            this.state.isCompleted = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
            this.NotifyAboutTriggered();
        }

        #region Init

        protected override HintStateRenovationInfo CreateDefaultState()
        {
            return new HintStateRenovationInfo();
        }

        #endregion

        public override void OnReady()
        {
            base.OnReady();
            this.modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            if (this.state.isCompleted || this.state.isReady)
            {
                return;
            }

            var hasNotRenovation = this.modernizationInteractor.modernizationData.multiplier == 1;
            if (hasNotRenovation)
            {
                this.modernizationInteractor.OnModernizationIsAvalible +=
                    this.OnModernizationAvalabilityChanged;
            }
            else
            {
                this.SetReadyState();
            }
        }

        private void OnModernizationAvalabilityChanged()
        {
            if (!this.modernizationInteractor.modernizationData.isAvailable)
            {
                return;
            }

            this.modernizationInteractor.OnModernizationIsAvalible -=
                this.OnModernizationAvalabilityChanged;
            this.SetReadyState();
        }

        private void SetReadyState()
        {
            this.state.isReady = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
        }
    }
}