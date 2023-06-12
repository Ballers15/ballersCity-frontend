using System;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Core;
using SinSity.Meta.Rewards;
using UnityEngine;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupDailyRewardsFromCharacters : UIPopupAnim<UIPopupDailyRewardsFromCharactersProperties, UIPopupArgs> {
        private List<CharacterReward> rewards;

        public void Setup(List<CharacterReward> rewards) {
            this.rewards = rewards;
            ClearOldWidgets();
            InstantinateNewWidgets();
            UpdateTotalRewards();
        }

        private void ClearOldWidgets() {
            foreach (Transform child in properties.rewardsContainer) {
                Destroy(child.gameObject);
            }
        }
        
        private void InstantinateNewWidgets() {
            foreach (var reward in rewards) {
                var widget = Instantiate(properties.widgetCharacterRewardPrefab, properties.rewardsContainer);
                widget.Setup(reward);
            }
        }
        
        private void UpdateTotalRewards() {
            var totalGold = GetGoldTotalAmount();
            var totalCrypto = GetCryptoTotalAmount();
            var totalCards = GetCardsTotalAmount();
            UpdateGoldWidget(totalGold);
            UpdateCryptoWidget(totalCrypto);
            UpdateCardsWidget(totalCards);
        }

        private int GetGoldTotalAmount() {
            var totalAmount = 0;
            foreach (var charReward in rewards) {
                var reward = charReward.rewardInfo;
                if (reward is RewardInfoHardCurrency castedReward) {
                    totalAmount += castedReward.value;
                }
            }
            return totalAmount;
        }
        
        private float GetCryptoTotalAmount() {
            var totalAmount = 0f;
            foreach (var charReward in rewards) {
                var reward = charReward.rewardInfo;
                if (reward is RewardInfoCryptoCurrency castedReward) {
                    totalAmount += castedReward.value;
                }
            }
            return totalAmount;
        }
        
        private int GetCardsTotalAmount() {
            var totalAmount = 0;
            foreach (var charReward in rewards) {
                var reward = charReward.rewardInfo;
                if (reward is RewardInfoCard castedReward) {
                    totalAmount += castedReward.count;
                }
            }
            return totalAmount;
        }
        
        private void UpdateGoldWidget(int totalGold) {
            if (totalGold <= 0)
                properties.rewardsWidgets[0].Hide();
            else {
                properties.rewardsWidgets[0].UpdateVisual(properties.rewardsIcons[0], totalGold.ToString());
                properties.rewardsWidgets[0].Show();
            }
        }
        
        private void UpdateCryptoWidget(float totalCrypto) {
            if (totalCrypto <= 0)
                properties.rewardsWidgets[1].Hide();
            else {
                properties.rewardsWidgets[1].UpdateVisual(properties.rewardsIcons[1], totalCrypto.ToString());
                properties.rewardsWidgets[1].Show();
            }
        }
        
        private void UpdateCardsWidget(int totalCards) {
            if (totalCards <= 0)
                properties.rewardsWidgets[2].Hide();
            else {
                properties.rewardsWidgets[2].UpdateVisual(properties.rewardsIcons[2], totalCards.ToString());
                properties.rewardsWidgets[2].Show();
            }
        }

        private void OnEnable() {
            properties.btnGet.AddListener(OnBtnGetClicked);
        }

        private void OnBtnGetClicked() {
            Hide();
        }
        
        private void OnDisable() {
            properties.btnGet.RemoveListener(OnBtnGetClicked);
        }
    }
}