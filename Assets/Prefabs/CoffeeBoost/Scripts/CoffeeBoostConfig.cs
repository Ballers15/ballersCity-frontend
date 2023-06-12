using UnityEngine;

namespace IdleClicker.Gameplay {
    [CreateAssetMenu(fileName = "CoffeeBoostConfig", menuName = "Game/CoffeeBoost/New CoffeeBoostConfig")]
    public class CoffeeBoostConfig : ScriptableObject {

        #region CONSTANTS

        public const string PATH = "Config/CoffeeBoostConfig";

        #endregion

        [SerializeField] private GameObject _cupPrefabForGameplayScreen;
        [SerializeField] private GameObject _cupPrefabForPopup;
        [SerializeField] private float m_duration = 6f;
        [SerializeField] private float m_boostTimeScale = 10f;

        public GameObject cupPrefabForGameplayScreen => _cupPrefabForGameplayScreen;
        public GameObject cupPrefabForPopup => _cupPrefabForPopup;
        public float duration => this.m_duration;
        public float boostTimeScale => this.m_boostTimeScale;
    }
}