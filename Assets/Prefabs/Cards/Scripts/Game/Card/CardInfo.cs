using UnityEngine;

namespace SinSity.Core {
    [CreateAssetMenu(menuName = "GamePlay/Cards/new CardInfo", fileName = "CardInfo")]
    public class CardInfo : ScriptableObject, ICardInfo {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _sortingOrder;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;

        public string id => _id;
        public Sprite activeSprite => _activeSprite;
        public Sprite inactiveSprite => _inactiveSprite;
        public string description => _description;
        public string cardName => _name;
        public int sortingOrder => _sortingOrder;
    }
}