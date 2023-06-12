using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Repo;
using SinSity.Scripts;
using UnityEngine.TestTools;
using VavilichevGD.Architecture;

public class CardsTests {
    private CardsRepository _cardsRepository;
    private CardsInteractor _cardsInteractor;
    
    [UnitySetUp]
    public IEnumerator Setup() {
        yield return InitializeMockedGameManager();

        _cardsRepository = Game.GetRepository<CardsRepository>();
        _cardsInteractor = Game.GetInteractor<CardsInteractor>();
    }

    private IEnumerator InitializeMockedGameManager() {
        var usedRepositoryTypes = new List<Type>{
            typeof(CardsRepository)
        };
        var usedInteractorTypes = new List<Type>{
            typeof(CardsInteractor)
        };
        GameSinSityForTests.Run(usedRepositoryTypes, usedInteractorTypes);

        while (!GameSinSityForTests.isInitialized) {
            yield return null;
        }
    }
    
    [Test]
    public void CardsRepositoryIsInitialized() {
        Assert.True(_cardsRepository.isInitialized);
    }
    
    [Test]
    public void CardsInteractorIsInitialized() {
        Assert.True(_cardsInteractor.isInitialized);
    }
    
    [Test]
    public void CardsCollectionAreCreatedAndNotEmpty() {
        var cardsCollection = _cardsInteractor.GetCardsCollection();
        var cardsInCollection = cardsCollection.GetAllCards().ToArray().Length;
        Assert.True(cardsCollection != null && cardsInCollection > 0);
    }

    [Test]
    public void CardsAmountIncreasingAndDecreasingAreCorrect() {
        var cards = GetAllCardsFromCollection();
        const int amountToIncrease = 5;
        foreach (var card in cards) {
            var startingAmount = card.amount;
            card.IncreaseAmount(amountToIncrease);
            var validAmountAfterIncreasing = startingAmount + amountToIncrease;
            Assert.AreEqual(card.amount, validAmountAfterIncreasing);
            
            card.DecreaseAmount(amountToIncrease);
            Assert.AreEqual(card.amount, startingAmount);
        }
    }

    [Test]
    public void CardsAmountDoesNotGoBelowZero() {
        var cards = GetAllCardsFromCollection();
        foreach (var card in cards) {
            var curCardAmount = card.amount;
            var newCardAmount = curCardAmount + 1;
            card.DecreaseAmount(newCardAmount);
            Assert.AreEqual(card.amount, 0);
        }
    }
    
    [Test]
    public void CardsInteractorOnCardAmountChangedEventAreTriggering() {
        var eventTriggered = false;
        
        _cardsInteractor.OnCardAmountChanged += card => {
            eventTriggered = true;
        };

        var cards = GetAllCardsFromCollection();
        foreach (var card in cards) {
            _cardsInteractor.IncreaseCardAmount(card.id,1);
        }
        
        Assert.True(eventTriggered);
    }
    
    [Test]
    public void CardsOnCardAmountChangedEventAreTriggering() {
        var eventTriggered = false;
        var firstCard = GetAllCardsFromCollection().First();
        
        firstCard.OnAmountChanged += it => {
            eventTriggered = true;
        };
        
        firstCard.IncreaseAmount(1);
        firstCard.DecreaseAmount(1);
        
        Assert.True(eventTriggered);
    }
    
    [Test]
    public void CardsRepositoryAreSavingAndLoadingData() {
        var firstCard = GetAllCardsFromCollection().First();
        const int amountToIncrease = 5;
        //Amount changing causes the data to be saved
        firstCard.IncreaseAmount(amountToIncrease);
        var validAmountAfterLoading = firstCard.amount;
        
        //_cardsRepository.Load();
        var loadedData = _cardsRepository.GetSavedData();
        var firstCardData = loadedData.GetSavedData(firstCard.id);
        Assert.AreEqual(firstCardData.amount, validAmountAfterLoading);
    }

    private IEnumerable<ICard> GetAllCardsFromCollection() {
        var cardsCollection = _cardsInteractor.GetCardsCollection();
        return cardsCollection.GetAllCards();
    }
}
