#if UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using EcoClicker.Achievements;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

public sealed class AchievementProviderGameCenter : AchievementProvider
{
    #region CONSTANTS

    private double REVEAL_ACHIEVE = 0.0f;
    private double COMPLETE_ACHIEVE = 100.0f;

    #endregion
    
    #region DELEGATES
    private delegate void AchievementProviderGameCenterHandler(bool success);
    private event AchievementProviderGameCenterHandler onUserLoginEvent;
    
    #endregion

    private bool isUserAutorized = false;
    
    protected override void Initialize()
    {
        Game.OnGameInitialized += onGameInitialized;
        onUserLoginEvent += onUserLogin;
    }
    
    private void autorizeUser()
    {
        Social.localUser.Authenticate(success => {
            Debug.Log("USER GAME CENTER AUTH IS: "+success);
            onUserLoginEvent?.Invoke(success);
        });
    }
    
    #region EVENTS

    private void onGameInitialized(Game game)
    {
        Game.OnGameInitialized -= onGameInitialized;
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
        autorizeUser();
    }

    private void onUserLogin(bool success)
    {
        onUserLoginEvent -= onUserLogin;
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
        Debug.Log("TRY UNLOCK ACHIEVE WITH AUTORIZATION: "+isUserAutorized);
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