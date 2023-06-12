using System;
using System.Collections;
using System.Collections.Generic;
using IdleClicker;
using UnityEngine;
using NUnit.Framework;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Repo;
using SinSity.Scripts;
using Tests.Scripts;
using UnityEditor.VersionControl;
using UnityEngine.TestTools;
using VavilichevGD.Architecture;
using Object = UnityEngine.Object;

public class CharactersTests {
    private CharactersInteractor _charactersInteractor;

    [UnitySetUp]
    public IEnumerator Setup() {
        if (Game.isInitialized) yield break;

        LoaderForTests.LoadIdleObject();
        LoaderForTests.LoadGameManager();
        LoaderForTests.LoadCameraController();
        GameEcoClicker.Run();
        
        while (!Game.isInitialized) {
            yield return null;
        }
        
        _charactersInteractor = Game.GetInteractor<CharactersInteractor>();
    }

    [Test]
    public void CharactersInteractorAreInitialized() {
        Assert.True(_charactersInteractor != null && _charactersInteractor.isInitialized);
    }
    
    [Test]
    public void CharactersAreCreated() {
        Assert.Greater(_charactersInteractor.GetCharactersCount(), 0);
    }
    
    [Test]
    public void AllCharactersHaveCorrectIdleObject() {
        var chars = _charactersInteractor.GetAllCharacters();
        foreach (var character in chars) {
            var expectedIdleObjectId = character.info.idleObjectInfo.id;
            var idleObject = character.idleObject;
            Assert.True(idleObject != null && idleObject.id == expectedIdleObjectId);
        }
    }
    
    [Test]
    public void AllCharactersHaveCorrectCard() {
        var chars = _charactersInteractor.GetAllCharacters();
        foreach (var character in chars) {
            var expectedCardId = character.info.characterCardInfo.id;
            var card = character.characterCard;
            Assert.True(card != null && card.id == expectedCardId);
        }
    }
    
    [Test]
    public void AllCharactersGeneratingRewards() {
        var chars = _charactersInteractor.GetAllCharacters();
        foreach (var character in chars) {
            if (!character.isActive) {
                character.characterCard.IncreaseAmount(1);
            }
            
            var rewardInfo = character.GetRandomDailyReward();
            Assert.NotNull(rewardInfo);
            
            character.characterCard.DecreaseAmount(character.characterCard.amount);
        }
    }


    [UnityTest] 
    public IEnumerator CharactersAreActivatingAndDeactivating() {
        var chars = _charactersInteractor.GetAllCharacters();
        foreach (var character in chars) {
            var card = character.characterCard;
            
            if (character.isActive) {
                card.DecreaseAmount(card.amount);
                yield return null;
            }
            
            Assert.False(character.isActive);
            card.IncreaseAmount(1);
            
            yield return null;
            
            Assert.True(character.isActive);
            
            card.DecreaseAmount(card.amount);
            yield return null;
            
            Assert.False(character.isActive);
        }
    }

    [UnityTest]
    public IEnumerator CharactersAreIncreasingAndDecreasingIdleIncome() {
        var chars = _charactersInteractor.GetAllCharacters();
        foreach (var character in chars) {
            var card = character.characterCard;
            var idle = character.idleObject;

            if (!idle.isBuilt) continue;

            if (character.isActive) {
                card.DecreaseAmount(card.amount);
                yield return null;
            }
            
            var incomeBeforeCharActivation = idle.incomeCurrent;
            card.IncreaseAmount(1);
            
            yield return null;
            
            Assert.True(idle.incomeCurrent > incomeBeforeCharActivation);
            
            card.DecreaseAmount(1);
            
            yield return null;
            
            Assert.False(idle.incomeCurrent > incomeBeforeCharActivation);
        }
    }
    
    
}
