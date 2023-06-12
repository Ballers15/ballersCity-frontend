using DG.Tweening;
using Notification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public class GetDeviceInfoModel : BaseModel<RefreshDeviceRequest>
    {
        private const float API_WAIT_TIME = 5f;
        
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(RefreshDeviceRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
          
            StopAllCoroutines();
            StartCoroutine(WaitAndExecuteAPI(ProcessAPI, API_WAIT_TIME));
        }

        private void ProcessAPI()
        {
            Debug.Log(ApiUserData);

            //don't execute api if player has logged out
            if (ApiUserData == null)
            {
                return;
            }

            RefreshDeviceRequest data = new RefreshDeviceRequest();
           
            SetForm(data);
            CallAPI(url, wwwForm, api_name);
        }

        protected override void SetForm(RefreshDeviceRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("user_id", data.user_id);
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
            StartCoroutine(WaitAndExecuteAPI(ProcessAPI, API_WAIT_TIME));
        }

        protected override void APISuccessHandler(object data)
        {
            RefreshDeviceResponse response = JsonUtility.FromJson<RefreshDeviceResponse>(data.ToString());
            string userDeviceId = response.data;

            if (ShowLog)
            {
                Debug.Log("==========access token success");
                Debug.Log(data.ToString());
            }

            if(userDeviceId != Utils.DeviceID)
            {
                //logout
                NotificationController.Notify("You have logged in from another device!", false);
                Logout();
                SceneController.LoadScene(new SceneProperties("Login"));
            }
            else
            {
                StartCoroutine(WaitAndExecuteAPI(ProcessAPI, API_WAIT_TIME));
            }
        }
    }

    [Serializable]
    public class RefreshDeviceRequest
    {
        public string user_id;

        public RefreshDeviceRequest()
        {
            user_id = BaseModel.User.id;
        }
    }

    [Serializable]
    public class RefreshDeviceResponse : APIResult<int>
    {
        public string data;
    }
}
