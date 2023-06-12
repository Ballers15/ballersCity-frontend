using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Notification
{
    [CreateAssetMenu(fileName = "NotificationData", menuName = "Scriptable Objects/Notification Data")]
    public class NotificationModel : ScriptableObject
    {
        [SerializeField] private GameObject autoHidePopup;
        [SerializeField] private GameObject confirmPopup;
        [SerializeField] private GameObject loadingPopup;

        public List<NotificationInfo> data;
        private int playerScore;

        public NotificationInfo GetInfo(Scenario scenario)
        {
            NotificationInfo info = data.Find(x => x.scenario == scenario);
            return info;
        }

        public void ShowNotification(NotificationInfo info)
        {
            if (info.isAutoHide)
            {
                CreateAutoHidePopup(info);
            }
            else
            {
                CreateConfirmPopup(info);
            }
        }

        private void CreateAutoHidePopup(NotificationInfo info)
        {
//            Debug.LogError("instantiated");

            GameObject popupObj = Instantiate(autoHidePopup) as GameObject;
            popupObj.name = "Notification";
            popupObj.GetComponent<NotificationUI>().ShowNotification(info);
        }

        private void CreateConfirmPopup(NotificationInfo info)
        {
            GameObject popupObj = Instantiate(confirmPopup) as GameObject;
            popupObj.name = "Confirm";
            popupObj.GetComponent<NotificationUI>().ShowNotification(info);
        }

        public GameObject CreateLoading()
        {
            GameObject popupObj = Instantiate(loadingPopup) as GameObject;
            popupObj.name = "Loading";
            popupObj.GetComponent<NotificationUI>().ShowNotification(null);
            return popupObj;
        }
    }

    [System.Serializable]
    public class NotificationInfo
    {
        public Scenario scenario;
        public string message;
        public Sprite image;
        public bool isSuccess;
        public bool isAutoHide;
        public UnityAction callBack;
    }

    public enum Scenario
    {
        NONE,
        INTERNET_CONNECTIVITY
    };
}