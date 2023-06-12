using System.Collections.Generic;
using SinSity.Core;

namespace SinSity.Domain {
    public class CharactersIdleObjectIncomeMultiplierController {
        private CharactersInteractor _charactersInteractor;
        private IdleObjectsInteractor _idleObjectsInteractor;
        private List<ICharacter> _characters;
        
        public CharactersIdleObjectIncomeMultiplierController(CharactersInteractor charactersInteractor) {
            _charactersInteractor = charactersInteractor;
        }

        public void Initialize() {
            _characters = _charactersInteractor.GetAllCharacters();
            foreach (var character in _characters) {
                if (character.isActive) character.ApplyIncomeMultiplierToIdleObject();
                character.OnCharacterActiveStateChanged += UpdateIncomeMultiplierState;
            }
        }

        private void UpdateIncomeMultiplierState(ICharacter character) {
            if(character.isActive) 
                character.ApplyIncomeMultiplierToIdleObject();
            else
                character.RemoveIncomeMultiplierFromIdleObject();
        }
    }
}