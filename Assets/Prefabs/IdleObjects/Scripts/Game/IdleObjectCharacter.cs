using System;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;
using CharacterInfo = SinSity.Core.CharacterInfo;

[RequireComponent(typeof(IdleObjectAnimator))]
public class IdleObjectCharacter : MonoBehaviour {
    [SerializeField] private CharacterInfo info;

    private ICharacter curCharacter;
    
    private void Awake() {
        Game.OnGameInitialized += OnGameInitialized;
    }
    
    private void OnGameInitialized(Game game) {
        var charactersInteractor = Game.GetInteractor<CharactersInteractor>();
        curCharacter = charactersInteractor.GetCharacter(info.id);

        curCharacter.OnCharacterActiveStateChanged += CheckCharacterState;
        CheckCharacterState(curCharacter);
    }

    private void CheckCharacterState(ICharacter character) {
        gameObject.SetActive(curCharacter.isActive);
    }

    private void OnDestroy() {
        curCharacter.OnCharacterActiveStateChanged -= CheckCharacterState;
        curCharacter = null;
    }
}
