using System.Collections.Generic;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Scripts;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIControllerDailyRewardsFromCharacters : UISceneElement {
        public bool isInitialized { get; protected set; }

        private CharactersInteractor charactersInteractor;
        
        protected override void OnGameInitialized() {
            base.OnGameInitialized();
            charactersInteractor = Game.GetInteractor<CharactersInteractor>();
            charactersInteractor.OnRewardsGenerated += ShowPopup;
            isInitialized = true;
        }

        private void ShowPopup(List<CharacterReward> characterRewards) {
            var uiController = Game.GetInteractor<UIInteractor>();
            var popup = uiController.GetUIElement<UIPopupDailyRewardsFromCharacters>();
            popup.Setup(characterRewards);
            popup.Show();
        }
    }
}