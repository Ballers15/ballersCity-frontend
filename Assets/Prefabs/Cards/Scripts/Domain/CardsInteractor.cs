using System;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Data;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public class CardsInteractor : Interactor {
        public event Action<ICard> OnCardAmountChanged;

        public override bool onCreateInstantly { get; } = true;

        private ICardsCollection _cardsCollection;
        
        public ICard GetCard(string cardId) {
            return _cardsCollection.GetCard(cardId);
        }
        
        public void IncreaseCardAmount(string cardId, int amount) {
            var card = _cardsCollection.GetCard(cardId);
            card.IncreaseAmount(amount);
            OnCardAmountChanged?.Invoke(card);
        }

        public void DecreaseCardAmount(string cardId, int amount) {
            var card = _cardsCollection.GetCard(cardId);
            card.DecreaseAmount(amount);
            OnCardAmountChanged?.Invoke(card);
        }
        
        public ICardsCollection GetCardsCollection() {
            return _cardsCollection;
        }

        protected override void Initialize() {
            base.Initialize();

            var cardsInfo = GetCardsInfo();
            var cardsSaveDataStorage = GetRepository<CardsRepository>();
            _cardsCollection = new CardsCollection(cardsInfo, cardsSaveDataStorage);
        }

        private ICardInfo[] GetCardsInfo() {
            using var cardsInfoLoader = new CardsInfoLoader();
            return cardsInfoLoader.Load();
        }
    }
}