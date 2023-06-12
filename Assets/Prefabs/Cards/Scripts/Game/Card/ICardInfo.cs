using UnityEngine;

namespace SinSity.Core {
    public interface ICardInfo {
        string id { get; }
        string cardName { get; }
        Sprite activeSprite { get; }
        Sprite inactiveSprite { get; }
        string description { get; }
        int sortingOrder { get; }
    }
}