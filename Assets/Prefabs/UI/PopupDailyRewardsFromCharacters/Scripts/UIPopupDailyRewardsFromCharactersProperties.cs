using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupDailyRewardsFromCharactersProperties : UIProperties {
        [SerializeField] private WidgetCharacterReward _widgetCharacterRewardPrefab;
        [SerializeField] private Transform _rewardsContainer;
        [SerializeField] private Button _btnGet;
        [SerializeField] private Sprite[] _rewardsIcons;
        [SerializeField] private WidgetTotalRewards[] _rewardsWidgets;

        public WidgetCharacterReward widgetCharacterRewardPrefab => _widgetCharacterRewardPrefab;
        public Transform rewardsContainer => _rewardsContainer;
        public Button btnGet => _btnGet;
        public Sprite[] rewardsIcons => _rewardsIcons;
        public WidgetTotalRewards[] rewardsWidgets => _rewardsWidgets;
    }
}