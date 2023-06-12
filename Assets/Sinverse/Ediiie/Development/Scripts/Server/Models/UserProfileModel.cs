using Notification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserProfileModel : BaseModel<UserProfileRequest>
    {
        public override void ProcessRequest(UserProfileRequest data, Action callback)
        {
            AddToInitQueue(this);
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                CallAPI(url, data, api_name, API_TYPE.POST);
            }
        }

        private bool IsValidData(UserProfileRequest credentials)
        {
            string msg = "";

            if (msg.Length > 0)
            {
                Debug.Log(msg);
                //NotificationController.Notify(msg, false);
                return false;
            }

            return true;
        }

        protected override void APIResultHandler<T>(STATUS _status, object data)
        {
            base.APIResultHandler<T>(_status, data);
            Debug.Log("profile data received");
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log("error profile model : ================= " + msg);
            RemoveFromInitQueue(this);
            //OnDownloadCompleted(PoolHolder.AvatarsPool.GetRandomAvatar());
        }

        protected override void APISuccessHandler(object data)
        {
            //Debug.Log("profile model : ================= "+ data);
            UserProfileResponse profileResponse = JsonUtility.FromJson<UserProfileResponse>(data.ToString());
            User.userProfile = profileResponse.data;
            /*if (Utils.IsEmpty(User.userProfile.image))
            {
                OnDownloadCompleted(PoolHolder.AvatarsPool.GetRandomAvatar());
            }
            else
            {
                string baseImageURL = "https://myforge.vulcanforged.com";
                if (User.userProfile.image.StartsWith("/")) {
                    User.userProfile.image = baseImageURL + User.userProfile.image;
                }
                Utils.DownloadImage(User.userProfile.image, OnDownloadCompleted);
            }*/
            OnDownloadCompleted(null);
        }

        private void OnDownloadCompleted(Sprite sprite)
        {
            User.userProfile.imageSprite = sprite;
            
            if (sprite != null)
            {
                PlayerData.Instance.playerSprite = sprite;
            }

            RemoveFromInitQueue(this);
        }
    }

    public class UserProfileRequest : BaseRequest
    {
    }

    public class UserProfileResponse : APIResult<string>
    {
        public UserProfile data;
    }
}
