using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIPopupPersonageCardAnimator : AnimObject {

        /*private UIPopupPersonageCard popup;

        private void Start() {
            popup = gameObject.GetComponentInParent<UIPopupPersonageCard>();
        }

        public struct CardLevelUpgradeAnimationData {
            public UIPopupPersonageCardProperties properties;
            public string newSpeedValueText;
            public string newIncomeValueText;
        }

        private CardLevelUpgradeAnimationData animationDataRelevant;
        private string oldPorgressValueText;
        
        private static readonly int triggerLevelUpgraded = Animator.StringToHash("level_upgraded");
        private static readonly int triggerNotEnoughCards = Animator.StringToHash("not_enough_cards");
        private static readonly int triggerNotEnoughGems = Animator.StringToHash("not_enough_gems");
        
        
        public void PlayNotEnoughCards() {
            SetTrigger(triggerNotEnoughCards);
        }

        public void PlayNotEnoughGems() {
            SetTrigger(triggerNotEnoughGems);
        }

        public void PlayUpgradeSuccessful(CardLevelUpgradeAnimationData animationData) {
            this.animationDataRelevant = animationData;

            SetTrigger(triggerLevelUpgraded);
        }

        private void Handle_ChangeSpeedValue() {
            UIPopupPersonageCardProperties properties = this.animationDataRelevant.properties;
            string newSpeedValueText = this.animationDataRelevant.newSpeedValueText;
            properties.SetSpeedValue(newSpeedValueText);
        }

        private void Handle_ChangeIncomeValue() {
            UIPopupPersonageCardProperties properties = this.animationDataRelevant.properties;
            string newIncomeValueText = this.animationDataRelevant.newIncomeValueText;
            properties.SetIncomeValue(newIncomeValueText);
        }

        private void Handle_ChangeProgressValue() {
            this.popup.UpdateView();
        }*/
    }
}