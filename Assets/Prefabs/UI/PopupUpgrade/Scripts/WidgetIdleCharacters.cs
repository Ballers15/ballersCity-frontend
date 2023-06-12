using System.Collections.Generic;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Scripts;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;

namespace SinSity.UI {
    public class WidgetIdleCharacters : UISceneElement {
        [SerializeField] private Text textMultiplier;
        [SerializeField] private WidgetCharacterCard[] cardWidgets;
        
        private CharactersInteractor charactersInteractor;
        private IdleObject idleObject;
        private List<ICharacter> characters;
        
        protected override void OnGameInitialized() {
            base.OnGameInitialized();
            charactersInteractor = Game.GetInteractor<CharactersInteractor>();
        }
        
        protected override void OnElementEnable() {
            base.OnElementEnable();
            if (idleObject == null) return;

            if (idleObject.isInitialized)
                UpdateCharactersIncomeMultiplierView();
        }
        
        private void UpdateCharactersIncomeMultiplierView() {
            var charactersIncomeMultiplier = 1f;
            foreach (var character in characters) {
                if(character.isActive) charactersIncomeMultiplier *= character.info.characterIncomeMultiplier;
            }
            textMultiplier.text = $"x{charactersIncomeMultiplier.ToString()}";
        }

        public void Setup(IdleObject idle) {
            if (characters != null && characters.Count > 0) {
                foreach (var character in characters) {
                    character.OnCharacterActiveStateChanged -= UpdateIncomeVisual;
                }
            }
            idleObject = idle;
            characters = charactersInteractor.GetCharactersForIdle(idleObject.id);
            foreach (var character in characters) {
                character.OnCharacterActiveStateChanged += UpdateIncomeVisual;
            }
            SetupCardWidgets();
        }

        private void UpdateIncomeVisual(ICharacter obj) {
            UpdateCharactersIncomeMultiplierView();
        }

        private void SetupCardWidgets() {
            var cardsAmount = cardWidgets.Length;
            for (var i = 0; i < cardsAmount; i++) {
                cardWidgets[i].Setup(characters[i]);
            }
        }
    }
}