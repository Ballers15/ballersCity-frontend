using UnityEngine;

namespace SinSity.Core {
    [CreateAssetMenu(
        fileName = "CharacterInfo",
        menuName = "GamePlay/Characters/CharacterInfo"
    )]
    public class CharacterInfo : ScriptableObject, ICharacterInfo {
        [SerializeField] private string _id;
        [SerializeField] private string _description;
        [SerializeField] private IdleObjectInfo _idleObject;
        [SerializeField] private CardInfo _characterCard;
        [SerializeField] private float _characterIncomeMultiplier;
        [SerializeField] private RewardWithChanceWeight[] _rewardsWithWeights;

        public string id => _id;
        public string description => _description;
        public IdleObjectInfo idleObjectInfo => _idleObject;
        public CardInfo characterCardInfo => _characterCard;
        public float characterIncomeMultiplier => _characterIncomeMultiplier;
        public RewardWithChanceWeight[] rewardsWithWeights => _rewardsWithWeights;
    }
}