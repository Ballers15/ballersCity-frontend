using System;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorReceivePersonForUpgrade",
        menuName = "Domain/Hint/HintInspectorReceivePersonForUpgrade"
    )]
    public sealed class HintInspectorReceivePersonForUpgrade : HintStateInspector<HintStateReceivePersonForUpgrade>
    {
        private CardsInteractor _cardsInteractor;

        public bool IsReadyForView()
        {
            return this.state.isReady;
        }

        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutPersonReceivedViewed()
        {
            if (!this.IsReadyForView())
            {
                throw new Exception("Person card has not received yet!");
            }

            if (this.IsViewed())
            {
                throw new Exception("Person card info has already received!");
            }

            this.state.isCompleted = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
            this.NotifyAboutTriggered();
        }

        #region Init

        protected override HintStateReceivePersonForUpgrade CreateDefaultState()
        {
            return new HintStateReceivePersonForUpgrade();
        }

        #endregion

        public override void OnReady()
        {
            base.OnReady();
            this._cardsInteractor = Game.GetInteractor<CardsInteractor>();
            if (this.state.isCompleted || this.state.isReady)
            {
                return;
            }

            this._cardsInteractor.OnCardAmountChanged += this.OnCollectCard;
        }

        private void OnCollectCard(ICard card)
        {
#if DEBUG
            Debug.Log("ON COLLECT CARD!!!!");
#endif
            if (this.state.isReady)
            {
                this._cardsInteractor.OnCardAmountChanged -= this.OnCollectCard;
                return;
            }

            this.SetReadyState();
            
//            if (cardObject.currentLevelIndex > 0 && cardObject.IsCollectionReadyForNextLevel())
//            {
//                this.SetReadyState();
//            }
        }

        private void SetReadyState()
        {
            Debug.Log("<color=blue>ON COLLECT CARD READY!!!!</color>");
            this.state.isReady = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
        }
    }
}