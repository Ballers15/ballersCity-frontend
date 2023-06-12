using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using SinSity.Data;
using SinSity.Domain;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Core {
    public class CardsCollection : ICardsCollection, IDisposable {
        private ICardInfo[] _cardInfos;
        private List<ICard> _cardsCollection;
        private CardsRepository _cardsSaveDataStorage;
        private CardsSavedStates _collectionSavedStates;
        
        public ICard GetCard(string id) {
            return _cardsCollection.First(card => card.id == id);
        }

        public bool HasCard(string id) {
            return _cardsCollection.Any(card => card.id == id);
        }

        public IEnumerable<ICard> GetAllCards() {
            return _cardsCollection;
        }

        public CardsCollection(ICardInfo[] cardsInfo, CardsRepository repository) {
            _cardInfos = cardsInfo;
            _cardsSaveDataStorage = repository;
            CreateCardsCollection();
            FillUpSavedStatesFromCollection();
            SortCardsCollection();
            SubscribeOnCardsAmountChanged();
        }

        private void CreateCardsCollection() {
            _cardsCollection = new List<ICard>();
            _collectionSavedStates = _cardsSaveDataStorage.GetSavedData();

            foreach (var cardInfo in _cardInfos)
                InsertCardIntoCollection(cardInfo);
        }

        private void InsertCardIntoCollection(ICardInfo cardInfo) {
            var card = CreateCard(cardInfo);
            _cardsCollection.Add(card);
        }

        private ICard CreateCard(ICardInfo cardInfo) {
            if (IsCollectionSavedStatesIsEmpty() || !HasSavedState(cardInfo)) return CreateCardWithDefaultState(cardInfo);
            return CreateCardWithSavedState(cardInfo);
        }

        private bool IsCollectionSavedStatesIsEmpty() {
            return _collectionSavedStates == null;
        }

        private bool HasSavedState(ICardInfo cardInfo) {
            return _collectionSavedStates.HasState(cardInfo.id);
        }
        
        private void FillUpSavedStatesFromCollection() {
            _collectionSavedStates = new CardsSavedStates(_cardsCollection);
        }
        
        private ICard CreateCardWithDefaultState(ICardInfo cardInfo) {
            return new Card(cardInfo);
        }
        
        private ICard CreateCardWithSavedState(ICardInfo cardInfo) {
            var savedState = _collectionSavedStates.GetSavedData(cardInfo.id);
            return new Card(cardInfo, savedState.amount);
        }

        private void SubscribeOnCardsAmountChanged() {
            foreach (var card in _cardsCollection) {
                card.OnAmountChanged += SaveCardsStatesAfterAmountChanged;
            }
        }
        
        private void SaveCardsStatesAfterAmountChanged(ICard card) {
            _collectionSavedStates.UpdateCardSaveData(card.id, card.amount);
            var user = User.GetInstance();
            var data = new SetNftCardRequestPDO {
                login = user.userId,
                command = "SET",
                cardId = card.id,
                amount = card.amount
            };
            var jsonData = JsonUtility.ToJson(data);
            FakeServerRequestsProcessor.SaveNftCards(SinCityEncryptor.Encrypt(jsonData));
            //Coroutines.StartRoutine(SendSetNftRequest(card));
        }

        /*private IEnumerator SendSetNftRequest(ICard card) {
            using var request = new NftCardsRequest(card);
            yield return request.Send();
        }*/

        private void SortCardsCollection() {
            _cardsCollection = _cardsCollection.OrderBy(card => card.GetSortingOrder()).ToList();
        }

        public void Dispose() {
            UnsubscribeFromCardsAmountChangedEvent();
        }
        
        private void UnsubscribeFromCardsAmountChangedEvent() {
            foreach (var card in _cardsCollection) {
                card.OnAmountChanged -= SaveCardsStatesAfterAmountChanged;
            }
        }
    }
}