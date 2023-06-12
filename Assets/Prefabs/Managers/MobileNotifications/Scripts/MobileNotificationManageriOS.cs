#if UNITY_IOS
using System;
using Unity.Notifications.iOS;
using UnityEngine;

public class MobileNotificationManageriOS : MonoBehaviour
{
    public string notificationId = "test_notification";
    private int identifier;

    private void Start()
    {
        iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, 10),
            Repeats = false,
        };

        iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger()
        {
            // Year = 2019,
            // Month = 8,
            //Day = 30,
            Hour = 12,
            Minute = 0,
            // Second = 0
            Repeats = false
        };

        iOSNotificationLocationTrigger locationTrigger = new iOSNotificationLocationTrigger()
        {
            Center = new Vector2(2.294498f, 48.858263f),
            Radius = 250f,
            NotifyOnEntry = true,
            NotifyOnExit = false,
        };

        iOSNotification notification = new iOSNotification()
        {
            Identifier = "test_notification",
            Title = "Test Notification!",
            Subtitle = "A Unity Royale Test Notification",
            Body = "This is a test notification!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);

        iOSNotificationCenter.OnRemoteNotificationReceived += recievedNotification =>
        {
            Debug.Log("Recieved notification " + notification.Identifier + "!");
        };


        iOSNotification notificationIntentData = iOSNotificationCenter.GetLastRespondedNotification();

        if (notificationIntentData != null)
        {
            Debug.Log("App was opened with notification!");
        }

    }

    private void OnApplicationPause(bool pause)
    {

        //If it's scheduled remove it when the app is paused
        iOSNotificationCenter.RemoveScheduledNotification(notificationId);

        //if it's already been delivered, remove the notification from the notification center
        iOSNotificationCenter.RemoveDeliveredNotification(notificationId);

    }
}
#endif