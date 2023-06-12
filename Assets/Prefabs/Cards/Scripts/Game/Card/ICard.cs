using System;
using UnityEngine;

namespace SinSity.Core {
    public interface ICard {
        event Action<ICard> OnAmountChanged;
        string id { get; }
        int amount { get; }

        void IncreaseAmount(int amount);
        void DecreaseAmount(int amount);
        Sprite GetActiveSprite();
        Sprite GetInactiveSprite();
        string GetName();
        string GetDesription();
        int GetSortingOrder();
    }
}