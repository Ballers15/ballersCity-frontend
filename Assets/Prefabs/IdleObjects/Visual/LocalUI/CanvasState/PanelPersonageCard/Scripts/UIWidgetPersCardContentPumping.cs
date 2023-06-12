using SinSity.Core;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetPersCardContentPumping : UIWidget<UIWidgetPersCardContentPumpingProps>
    {
        /*#region Const

        private const float HUNDRED_PERCENTS = 1.0f;

        #endregion

        public void SetActive(bool isActive)
        {
            this.gameObject.SetActive(isActive);
        }

        public void Setup(CardObject cardObject)
        {
            var currentCardCount = cardObject.currentCardCount;
            this.properties.imageIcon.sprite = cardObject.info.spriteIconThick;
            if (cardObject.HasNextLevel())
            {
                var nextLevel = cardObject.GetNextLevel();
                var requiredCardCount = nextLevel.m_requiredCardCountForUpgrade;
                this.properties.textPersCardLevelValue.text = $"{currentCardCount}/{requiredCardCount}";
                var cardsPercent = cardObject.GetCollectedCardsForNextLevelPercent();
                this.properties.progressBar.SetValue(cardsPercent);
            }
            else
            {
                this.properties.textPersCardLevelValue.text = CardObjectInfo.MAX_LEVEL_TEXT;
                this.properties.progressBar.SetValue(HUNDRED_PERCENTS);
            }
        }*/
    }
}