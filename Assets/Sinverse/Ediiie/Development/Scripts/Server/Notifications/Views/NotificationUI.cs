using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Notification
{
    public abstract class NotificationUI : MonoBehaviour
    {
        private NotificationInfo currentNotificationInfo;
        public static Action<Transform> OnAddToCanvas;

        public virtual void ShowNotification(NotificationInfo info)
        {
            currentNotificationInfo = info;
            OnAddToCanvas(this.transform);
        }

        protected void CloseNotification()
        {
            this.gameObject.SetActive(false);
            NotificationController.RemoveNotificationFromQueue(currentNotificationInfo);
        }

        protected IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(4f);
            CloseNotification();
        }

        protected void OnDestroy()
        {            
            CloseNotification();
        }
    }
}