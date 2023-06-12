using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Notification;

namespace CogniHab.Notification
{
    /// <summary>
    /// Used to display AutoPopup panel with provided data
    /// At present no icon is there, in case we add icon to show success or failure then just uncomment the code place in ShowNotification function
    /// </summary>
    public class AutoPopupView : NotificationUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite overrideSprite;

        public override void ShowNotification(NotificationInfo info)
        {
            base.ShowNotification(info);
            messageText.text = info.message;
            //icon.overrideSprite = (info.isSuccess) ? null : overrideSprite; 
            StartCoroutine(AutoHide());
        }
    }
}