using System;
using SinSity.Domain;
using SinSity.Notifications;
using SinSity.Services;
//using Unity.Notifications.Android;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

#if UNITY_ANDROID && !UNITY_EDITOR
public class MobileNotificationManager : MonoBehaviour
{

    #region Constants

    private const string CHANNEL_DEFAULT = "default_channel";
    private const int REM_TIME_HOURS_NOTIFICATION_SHORT = 4;
    private const int REM_TIME_HOURS_NOTIFICATION_LONG = 96;

    private static TimeSpan repeatInterval = new TimeSpan(12, 0, 0);

    #endregion
    
    
   // private AndroidNotificationChannel defaultNotificationChannel;


    #region Start

    private void Start() {
        if (!IsValidPlatform())
            return;

        this.defaultNotificationChannel = CreateDefaulNotificationChannel();
        AndroidNotificationCenter.Initialize();
        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);*/
    }

    private bool IsValidPlatform() {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#endif
        return false;
    }
    
    private AndroidNotificationChannel CreateDefaulNotificationChannel() {
        return new AndroidNotificationChannel()
        {
            Id = CHANNEL_DEFAULT,
            Name = "Default Channel",
            Description = "For Generic notifications",
            Importance = Importance.Default,
        };
    }

    #endregion
   
    
    private void OnApplicationPause(bool pause) {
        if (!IsValidPlatform())
            return;

        if (pause)
            SetupAndCreateNotifications();
        else
            ReportAndClearOldNotifications();
    }


    #region CreateNotifications

     private void SetupAndCreateNotifications() {
        if (!Game.isInitialized)
            return;

        this.CreateComebackNotification(REM_TIME_HOURS_NOTIFICATION_LONG);
        this.CreateResearchNotification();
        
        bool dailyRewardNotificationCreated = this.CreateDailyRewardNotification();
        if (!dailyRewardNotificationCreated)
            this.CreateComebackNotification(REM_TIME_HOURS_NOTIFICATION_SHORT);
    }
    
    private bool CreateComebackNotification(int remainingTimeHours) {
        var notificationData = new AndroidNotificationDataComeback(remainingTimeHours);
        if (notificationData.allowToCreate) {
            this.CreateNotification(notificationData);
            return true;
        }

        return false;
    }
    
    private bool CreateResearchNotification() {
        var dataInteractor = Game.GetInteractor<ResearchObjectDataInteractor>();
        var researchNotification = new AndroidNotificationDataResearch(dataInteractor);
        
        if (researchNotification.allowToCreate) {
            this.CreateNotification(researchNotification);
            return true;
        }

        return false;
    }
    
    private bool CreateDailyRewardNotification() {
        var dailyRewardInteractor = Game.GetInteractor<DailyRewardInteractor>();
        var dailyRewardNotificationData = new AndroidNotificationDataDailyReward(dailyRewardInteractor);

        if (dailyRewardNotificationData.allowToCreate) {
            this.CreateNotification(dailyRewardNotificationData);
            return true;
        }

        return false;
    }

    private void CreateNotification(AndroidNotificationData androidNotificationData, bool repeat = true) {

        AndroidNotification notification = new AndroidNotification()
        {
            Title = androidNotificationData.title,
            Text = androidNotificationData.text,
            SmallIcon = androidNotificationData.smallIconId,
            LargeIcon = androidNotificationData.largeIconId,
            FireTime = androidNotificationData.fireTime,
        };

        if (repeat)
            notification.RepeatInterval = repeatInterval;

        int idForStatus = AndroidNotificationCenter.SendNotification(notification, CHANNEL_DEFAULT);
        SaveNotificationInfo(idForStatus, androidNotificationData.id, androidNotificationData.GetPrefKey());
    }

    private void SaveNotificationInfo(int idForStatus, string idForAnalytics, string prefKey) {
        var data = new AndroidNotificationSaveData();
        data.idForStatus = idForStatus;
        data.idForAnalytics = idForAnalytics;
        Storage.SetCustom(prefKey, data);
    }

    #endregion


    #region Clear and report

    private void ReportAndClearOldNotifications() {
        ReportNotificationStatuses();
        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void ReportNotificationStatuses() {
        this.ReportComebackNotificationEvent(REM_TIME_HOURS_NOTIFICATION_LONG);
        this.ReportComebackNotificationEvent(REM_TIME_HOURS_NOTIFICATION_SHORT);
        this.ReportResearchNotificationEvent();
        this.ReportDailyRewardNotificationEvent();
    }
    
    private void ReportComebackNotificationEvent(int remainingTimeHours) {
        var prefKey = $"{AndroidNotificationDataComeback.PREFKEY_PREFIX}{remainingTimeHours}";
        this.TryToReportNotificationEvent(prefKey);
    }
    
    private void ReportResearchNotificationEvent() {
        var prefKey = AndroidNotificationDataResearch.PREFKEY;
        this.TryToReportNotificationEvent(prefKey);
    }
    
    private void ReportDailyRewardNotificationEvent() {
        var prefKey = AndroidNotificationDataDailyReward.PREFKEY;
        this.TryToReportNotificationEvent(prefKey);
    }
    
    private void TryToReportNotificationEvent(string prefKey) {
        if (!Storage.HasObject(prefKey))
            return;
        
        var loadedData = LoadNotificationData(prefKey);
        var idForAnalytics = loadedData.idForAnalytics;
        var idForStatus = loadedData.idForStatus;
        
        if (!AndroidNotificationCenter.Initialize())
            return;

        var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(idForStatus);
        CommonAnalytics.LogNotificationHandled(idForAnalytics, status.ToString());
    }
    
    private AndroidNotificationSaveData LoadNotificationData(string prefKey) {
        return Storage.GetCustom(prefKey, AndroidNotificationSaveData.GetDefault());
    }

    #endregion
}
#endif