using System.Collections;
using NUnit.Framework;
using SinSity.Data;
using SinSity.Domain;
using SinSity.Repo;
using SinSity.Scripts;
using SinSity.UI;
using Tests.Scripts;
using UnityEngine;
using UnityEngine.TestTools;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;


public class DailyRewardsFromCharactersTests {
    private DailyRewardsFromCharactersInteractor rewardsFromCharactersInteractor;
    private UIControllerDailyRewardsFromCharacters rewardsFromCharactersUIController;
    private UIController gameUIController;
    
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
    
        rewardsFromCharactersInteractor = Game.GetInteractor<DailyRewardsFromCharactersInteractor>();
        rewardsFromCharactersUIController = GetUIController();
    }

    private UIControllerDailyRewardsFromCharacters GetUIController() {
        gameUIController = Game.GetInteractor<UIInteractor>().uiController;
        return gameUIController.GetUIController<UIControllerDailyRewardsFromCharacters>();
    }

    [Test]
    public void MainInteractorAreInitialized() {
        Assert.True(rewardsFromCharactersInteractor != null && rewardsFromCharactersInteractor.isInitialized);
    }
    
    [Test]
    public void MainRepositoryAreInitialized() {
        var repo = Game.GetRepository<DailyRewardsFromCharactersRepository>();
        Assert.True(repo != null && repo.isInitialized);
    }
    
    [Test]
    public void MainUIControllerAreInitialized() {
        Assert.True(rewardsFromCharactersUIController != null && rewardsFromCharactersUIController.isInitialized);
    }
    
    [UnityTest]
    public IEnumerator RepositoryAreSavingAndLoadingDateRight() {
        var repo = Game.GetRepository<DailyRewardsFromCharactersRepository>();
        var dataToSave = new DailyRewardsFromCharactersData(GameTime.now);
        var savedDateTime = dataToSave.lastRewardCollectionTimeSerialized.GetDateTime();
        repo.SaveData(dataToSave);

        yield return new WaitForSecondsRealtime(1);

        yield return repo.LoadData(GameTime.now);

        var loadedData = repo.GetData();
        var loadedDateTime = loadedData.lastRewardCollectionTimeSerialized.GetDateTime();
        
        Assert.AreEqual(savedDateTime, loadedDateTime);
    }
    
    [Test]
    public void UIControllerShowPopupWhenRewardsAreGenerated() {
        var charactersInteractor = Game.GetInteractor<CharactersInteractor>();
        charactersInteractor.GenerateCharactersRewards();

        var popup = gameUIController.GetUIElement<UIPopupDailyRewardsFromCharacters>();
        Assert.True(popup.isActive);
        popup.Hide();
    }
}