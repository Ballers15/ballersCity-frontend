using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Data {
    [Serializable]
    public class CardsSavedStates : ICloneable<CardsSavedStates> {
        [SerializeField] private List<CardSavedData> _savedStates;

        public CardsSavedStates(IEnumerable<CardSavedData> savedData) {
            _savedStates = new List<CardSavedData>();
            foreach (var save in savedData) {
                _savedStates.Add(save);
            }
        }
        
        public CardsSavedStates(IEnumerable<ICard> cards) {
            _savedStates = new List<CardSavedData>();
            foreach (var card in cards) {
                _savedStates.Add(new CardSavedData(card.id, card.amount));
            }
        }
        
        public void UpdateCardSaveData(string id, int amount) {
            var savedData = GetSavedData(id);
            savedData.UpdateAmount(amount);
        }

        public CardSavedData GetSavedData(string cardInfoId) {
            return _savedStates.First(it => it.id == cardInfoId);
        }

        public CardsSavedStates Clone() {
            return new CardsSavedStates(_savedStates);
        }

        public bool HasState(string id) {
            return _savedStates.Any(it => it.id == id);
        }

        public List<CardSavedData> GetAllSavedData() {
            return _savedStates;
        }

        public void AddSaveData(CardSavedData cardSaveData) {
            _savedStates.Add(cardSaveData);
        }
    }

    [Serializable]
    public class CardSavedData {
        public string id;
        public int amount;

        public CardSavedData(string id, int amount) {
            this.id = id;
            this.amount = amount;
        }

        public void UpdateAmount(int amount) {
            this.amount = amount;
        }
    }
}