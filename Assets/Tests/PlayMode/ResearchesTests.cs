using System;
using System.Collections;
using System.Collections.Generic;
using IdleClicker.Gameplay;
using UnityEngine;
using NUnit.Framework;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Repo;
using SinSity.Scripts;
using UnityEngine.TestTools;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using Object = UnityEngine.Object;

public class ResearchesTests {
    private ResearchDataRepository _researchesRepository;
    private ResearchObjectDataInteractor _dataInteractor;
    private ResearchObjectTimerInteractor _timerInteractor;
    private ResearchObjectRewardInteractor _rewardInteractor;
    private ResearchObjectRenovationInteractor _renovationInteractor;
    private ResearchStateInteractor _stateInteractor;
    private BankInteractor _bankInteractor;
    private MockedGameManager _mockedGameManager;
    private bool _idleObjectsInstantiated;
    
    [UnitySetUp]
    public IEnumerator Setup() {
        if (!_idleObjectsInstantiated) {
            var idleObjects = Resources.LoadAll<IdleObject>("Prefabs/IdleObjects");
            foreach (var idle in idleObjects) {
                Object.Instantiate(idle);
            }

            _idleObjectsInstantiated = true;
        }
        
        GameEcoClicker.Run();
        if (_mockedGameManager == null) {
            var mockedGameManagerPath = "[MOCKED GAME MANAGER]";
            var mockedGameManagerPrefab = Resources.Load<MockedGameManager>(mockedGameManagerPath);
            _mockedGameManager = Object.Instantiate(mockedGameManagerPrefab);
        }

        while (!GameSinSityForTests.isInitialized) {
            yield return null;
        }

        _researchesRepository = Game.GetRepository<ResearchDataRepository>();
        _dataInteractor = Game.GetInteractor<ResearchObjectDataInteractor>();
        _timerInteractor = Game.GetInteractor<ResearchObjectTimerInteractor>();
        _rewardInteractor = Game.GetInteractor<ResearchObjectRewardInteractor>();
        _renovationInteractor = Game.GetInteractor<ResearchObjectRenovationInteractor>();
        _stateInteractor = Game.GetInteractor<ResearchStateInteractor>();
    }

    [Test]
    public void ResearchesRepositoryAreInitialized() {
        Assert.True(_researchesRepository != null && _researchesRepository.isInitialized);
    }
    
    [Test]
    public void ResearchesInteractorsAreInitialized() {
        Assert.True(_dataInteractor != null && _dataInteractor.isInitialized);
        Assert.True(_timerInteractor != null && _timerInteractor.isInitialized);
        Assert.True(_rewardInteractor != null && _rewardInteractor.isInitialized);
        Assert.True(_renovationInteractor != null && _renovationInteractor.isInitialized);
        Assert.True(_stateInteractor != null && _stateInteractor.isInitialized);
    }

    [Test]
    public void ResearchesAreCreated() {
        var researchesCount = _dataInteractor.GetResearchObjectsCount();
        Debug.Log($"Loaded {researchesCount} researches");
        Assert.Greater(researchesCount, 0);
    }

    [UnityTest]
    public IEnumerator ResearchesAreLaunchingAndStopping() {
        var researches = _dataInteractor.GetResearchObjects();
        foreach (var research in researches) {
            PrepareResearchForLaunch(research);
            _timerInteractor.LaunchResearchObject(research);
            yield return null;
            Assert.True(research.state.isEnabled);
            _timerInteractor.StopResearchObject(research);
            yield return null;
            Assert.False(research.state.isEnabled);
        }
    }

    [UnityTest]
    public IEnumerator ResearchesTimeAreDecreasesCorrectly() {
        var researches = _dataInteractor.GetResearchObjects();
        var decreasingTime = 2f;
        foreach (var research in researches) {
            PrepareResearchForLaunch(research);
            var researchTimeBeforeDecreasing = research.state.remainingTimeSec;
            if (researchTimeBeforeDecreasing == 0) researchTimeBeforeDecreasing = research.info.durationSeconds;
            _timerInteractor.LaunchResearchObject(research);
            
            yield return new WaitForSecondsRealtime(decreasingTime);
            
            var researchTimeAfterDecreasing = research.state.remainingTimeSec;
            var expectedTime = researchTimeBeforeDecreasing - Mathf.RoundToInt(decreasingTime);
            Assert.AreEqual(researchTimeAfterDecreasing,expectedTime);
            _timerInteractor.StopResearchObject(research);
        }
    }
    
    [UnityTest]
    public IEnumerator ResearchesAreFinishingCorrectly() {
        var researches = _dataInteractor.GetResearchObjects();
        var researchTestTime = 2f;
        foreach (var research in researches) {
            PrepareResearchForLaunch(research);
            _timerInteractor.LaunchResearchObject(research);
            research.state.remainingTimeSec = Mathf.RoundToInt(researchTestTime);
            
            yield return new WaitForSecondsRealtime(researchTestTime);

            Assert.False(research.state.isEnabled);
            Assert.True(research.state.isRewardReady);
            _rewardInteractor.ReceiveReward(this, research);
        }
    }

    private void PrepareResearchForLaunch(ResearchObject research) {
        if(research.state.isEnabled) _timerInteractor.StopResearchObject(research);
        if(research.state.isRewardReady) _rewardInteractor.ReceiveReward(this, research);
        AddMoneyForResearchLaunch(research);
    }
    
    private void AddMoneyForResearchLaunch(ResearchObject research) {
        var researchCost = research.state.price;
        Bank.AddSoftCurrency(researchCost, this);
    }
}
