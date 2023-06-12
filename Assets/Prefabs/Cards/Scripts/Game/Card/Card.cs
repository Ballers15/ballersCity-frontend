using System;
using UnityEngine;

namespace SinSity.Core {
    public class Card : ICard{
        private ICardInfo info;
        
        public event Action<ICard> OnAmountChanged;

        public string id => info.id;
        public int amount { get; private set; }

        public Card(ICardInfo info, int amount = 0) {
            this.info = info;
            this.amount = amount;
        }
        
        public void IncreaseAmount(int amount) {
            this.amount += amount;
            OnAmountChanged?.Invoke(this);
        }

        public void DecreaseAmount(int amount) {
            var newAmount = this.amount - amount;
            this.amount = Math.Max(0, newAmount);
            OnAmountChanged?.Invoke(this);
        }

        public Sprite GetActiveSprite() {
            return info.activeSprite;
        }

        public Sprite GetInactiveSprite() {
            return info.inactiveSprite;
        }

        public string GetName() {
            return info.cardName;
        }

        public string GetDesription() {
            return info.description;
        }

        public int GetSortingOrder() {
            return info.sortingOrder;
        }
    }
}