#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using EcoClicker.Achievements;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

public sealed class AchievementProviderGoogle : AchievementProvider
{
    #region CONSTANTS

    private double REVEAL_ACHIEVE = 0.0f;
    private double COMPLETE_ACHIEVE = 100.0f;
    private SignInStatus loginStatus;

    #endregion
    
    #region DELEGATES
    private delegate void AchievementProviderGoogleHandler(bool success);
    private event AchievementProviderGoogleHandler userLoginEvent;
    
    #endregion

    private bool isUserAutorized = false;
    
    protected override void Initialize()
    {
        Game.OnGameInitialized += onGameInitialized;
        this.userLoginEvent += onUserLogin;
    }
    
    private void autorizeUser()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways , (result) =>
        {
            var success = (result == SignInStatus.Success);
            loginStatus = result;
            Debug.Log("USER PLAY AUTH IS: "+success+" and status is: "+result);
            userLoginEvent?.Invoke(success);
        });
    }
    
    #region EVENTS

    private void onGameInitialized(Game game)
    {
        Game.OnGameInitialized -= onGameInitialized;
        var config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        autorizeUser();
    }

    private void onUserLogin(bool success)
    {
        this.userLoginEvent -= onUserLogin;
        if (success) isUserAutorized = true;
        Debug.Log("USER IS AUTORIZED: "+success);
    }
    
    #endregion
    
    public override void ShowAchievement(AchievementEntity achievement)
    {
        Social.localUser.Authenticate((bool successAuth) =>
        {
            if (successAuth)
            {
                Social.ReportProgress(achievement.id, COMPLETE_ACHIEVE, (bool success) => {
                    Debug.Log("ACHIEVEMENT SHOWING IS: "+success);
                    if(success) achievement.ShowAchievement();
                });
            }
        });
    }

    public override void CompleteAchievement(AchievementEntity achievement)
    {
        Debug.Log("TRY UNLOCK ACHIEVE WITH LOGIN STATUS: "+loginStatus+" AND AUTORIZATION "+isUserAutorized);
        if (!isUserAutorized) return;
        achievement.CompleteAchievement();
        Social.localUser.Authenticate((bool successAuth) =>
        {
            if (successAuth)
            {
                Social.ReportProgress(achievement.id, COMPLETE_ACHIEVE, (bool successComplete) => {
                    if(successComplete) achievement.ShowAchievement();
                    Debug.Log("ACHIEVEMENT COMPLITION IS: "+successComplete);
                });
            }
        });
    }

    public override void ShowAchievementsList()
    {
        Social.localUser.Authenticate((bool successAuth) =>
        {
            if (successAuth)
            {
                Social.ShowAchievementsUI();
            }
        });
    }
}
#endif