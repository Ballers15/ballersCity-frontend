using Ediiie.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Notification
{
    public class NotificationController : MonoBehaviour
    {
        [SerializeField] private NotificationModel notificationData;
        [SerializeField] private GameObject loadingPopup;

        public static NotificationController Instance;
        public static Action<NotificationInfo> OnShowNotification;
        
        public List<NotificationInfo> notificationQueue;

        private bool test = false;

        public static bool IsInternetAvailable
        {
            get
            {
                bool internetNotAvailable = (Application.internetReachability == NetworkReachability.NotReachable);
                if (internetNotAvailable)
                {
                    Show(Scenario.INTERNET_CONNECTIVITY);
                }

                return !internetNotAvailable;
            }
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            notificationQueue = new List<NotificationInfo>();
           // UserLoginModel.OnAPICompleted += TestNotifications;
        }

        private void OnDestroy()
        {
            //UserLoginModel.OnAPICompleted -= TestNotifications;
        }

        private void Start()
        {
        }

        private void TestNotifications(STATUS status, string message)
        {
            //used to test all notifications are working
            if (test)
            {
                foreach (Scenario scenario in Enum.GetValues(typeof(Scenario)))
                {
                    Show(scenario);
                }
            }
        }

        public static void Show(Scenario scenario)
        {
            NotificationInfo info = Instance.notificationData.GetInfo(scenario);

            if (info != null)
            {
                Instance.AddToNotificationQueue(info);
            }
        }

        public static void Confirm(string message, bool isSuccess, UnityAction callback)
        {
            NotificationInfo info = new NotificationInfo();
            info.message = message.Trim();
            info.isAutoHide = false;
            info.isSuccess = isSuccess;
            info.callBack = callback;

            Instance.AddToNotificationQueue(info);
        }

        public static void Notify(string message, bool isSuccess)
        {
            ShowLoading(false);

            if (message.ToLower().Contains("cannot resolve"))
            {
                message = "Check internet connection";
            }

            NotificationInfo info = new NotificationInfo();
            info.message = message.Trim();
            info.isAutoHide = true;
            info.isSuccess = isSuccess;

            Instance.AddToNotificationQueue(info);
        }

        private void AddToNotificationQueue(NotificationInfo info)
        {
            notificationQueue.Add(info);

            if (notificationQueue.Count == 1)
            {
                notificationData.ShowNotification(info);
            }
        }

        public static void RemoveNotificationFromQueue(NotificationInfo info)
        {
            Instance.notificationQueue.Remove(info);

            if (Instance.notificationQueue.Count > 0)
            {
                Instance.notificationData.ShowNotification(Instance.notificationQueue[0]);
            }
        }

        public static void ShowLoading(bool show)
        {
            Instance.loadingPopup.SetActive(show);
        }
 }
}