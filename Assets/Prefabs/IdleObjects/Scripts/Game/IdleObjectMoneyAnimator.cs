using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core {
    [RequireComponent(typeof(IdleObjectAnimator))]
    public class IdleObjectMoneyAnimator : MonoBehaviour {
        private IdleObject idleObject;
        private CharactersInteractor charactersInteractor;
        private CardsInteractor cardsInteractor;
        private List<ICharacter> idleChars;
        
        private void Awake() {
            Initialize();
        }
        
        private void Initialize() {
            idleObject = gameObject.GetComponentInParent<IdleObject>();
            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            charactersInteractor = Game.GetInteractor<CharactersInteractor>();
            idleChars = charactersInteractor.GetCharactersForIdle(idleObject.id);
            cardsInteractor = Game.GetInteractor<CardsInteractor>();
            cardsInteractor.OnCardAmountChanged += CheckVisual;
            UpdateVisual();
        }

        private void CheckVisual(ICard obj) {
            UpdateVisual();
        }

        private void UpdateVisual() {
            var hasActiveCharacter = idleChars.Any(character => character.isActive);
            gameObject.SetActive(!hasActiveCharacter);
        }

        private void OnDestroy() {
            cardsInteractor.OnCardAmountChanged -= CheckVisual;
        }
    }
}