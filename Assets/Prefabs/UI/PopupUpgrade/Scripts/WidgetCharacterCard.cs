using SinSity.Core;
using SinSity.Scripts;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class WidgetCharacterCard : UISceneElement {
        [SerializeField] private Image imageCard;
        [SerializeField] private Button buttonCardPopup;
        
        private ICharacter character;

        protected override void OnGameInitialized() {
            base.OnGameInitialized();
        }

        protected override void OnElementEnable() {
            base.OnElementEnable();
            if (character == null) return;
            character.OnCharacterActiveStateChanged += CheckState;
            UpdateVisual();
            buttonCardPopup.onClick.AddListener(OnBtnPopupCardClicked);
        }

        private void CheckState(ICharacter obj) {
            UpdateVisual();
        }

        protected override void OnElementDisable() {
            base.OnElementDisable();
            if (character == null) return;
            buttonCardPopup.onClick.RemoveListener(OnBtnPopupCardClicked);
        }

        private void OnBtnPopupCardClicked() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popupCard = uiInteractor.GetUIElement<UIPopupCard>();
            popupCard.Setup(character.characterCard);
            popupCard.Show();
        }

        private void UpdateVisual() {
            var characterCard = character.characterCard;
            var sprite = character.isActive ? characterCard.GetActiveSprite() : characterCard.GetInactiveSprite();
            imageCard.sprite = sprite;
        }

        public void Setup(ICharacter character) {
            this.character = character;
        }
    }
}