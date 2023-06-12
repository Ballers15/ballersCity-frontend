using Notification;
using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserLoginModel : UserModel<UserAuthRequest>
    {
        private void Awake()
        {
            ServerController.OnServerInit += CheckAutoLogin;
            Instance = this;
        }

        private void OnDestroy()
        {
            ServerController.OnServerInit -= CheckAutoLogin;
        }

        private void CheckAutoLogin()
        {
            string userInfo = PlayerPrefs.GetString(Constants.PP_PLAYER_INFO, "");
            int isAuthenticated = PlayerPrefs.GetInt(Constants.PP_IS_AUTHENTICATED, 0);

            if (isAuthenticated == 0 || Utils.IsEmpty(userInfo))
            {
                isAutoLogin = false;
                RemoveFromInitQueue(this); //use this only if login script is added to queue in project and no other script is added to queue
                return;
            }

            isAutoLogin = true;

            if (ShowLog) Debug.Log(userInfo);

            UserAuthRequest data = JsonUtility.FromJson<UserAuthRequest>(userInfo);
            ProcessRequest(data);

            Debug.Log("process auto login");
        }

        public override void ProcessRequest(UserAuthRequest data, Action callback = null)
        {
            ApiUserData = null;

            if (isAutoLogin)
            {
                AddToInitQueue(this);
            }

            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
                SavePlayerPref(data);
                NotificationController.ShowLoading(true);
            }
        }
        
        public override void ProcessRequest(UserAuthRequest data, Action<string> callback)
        {
            if (isAutoLogin)
            {
                AddToInitQueue(this);
            }

            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
                SavePlayerPref(data);
                NotificationController.ShowLoading(true);
            }
        }

        protected override void SetForm(UserAuthRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("username", data.username);
            wwwForm.AddField("password", data.password);
            wwwForm.AddField("device_id", Utils.DeviceID);
        }

        private void SavePlayerPref<T>(T data)
        {
            if (ShowLog) Debug.Log("player pref data : " + data);
            PlayerPrefs.SetString(Constants.PP_PLAYER_INFO, JsonUtility.ToJson(data));
        }

        private bool IsValidData(UserAuthRequest credentials)
        {
            string msg = "";

            if (Utils.IsEmpty(credentials.username))
            {
                msg = "Please enter valid credentials";
            }
            //else if(!Utils.IsValidEmail(credentials.username))
            //{
            //    msg = "Please enter valid email/username";
            //}
            else if (Utils.IsEmpty(credentials.password))
            {
                msg = "Please enter password";
            }

            if (msg.Length > 0)
            {
                NotificationController.Notify(msg, false);
                return false;
            }

            return true;
        }

        protected override void APIResultHandler<T1>(STATUS _status, object data)
        {
            base.APIResultHandler<T1>(_status, data);
        }

        protected override void APIErrorHandler(string msg)
        {
            base.APIErrorHandler(msg);
            //RemoveFromInitQueue(STATUS.API_ERROR);
            PlayerPrefs.DeleteKey(Constants.PP_PLAYER_INFO);
        }

        protected override void APISuccessHandler(object data)
        {
            if (ShowLog) Debug.Log("User Info "+data.ToString());
            base.APISuccessHandler(data);
            PlayerPrefs.SetInt(Constants.PP_IS_AUTHENTICATED, 1);
            AfterLoginSuccess();
        }
    }

    public class UserAuthRequest : RequestWithClientID
    {
        public string username;
        public string password;
    }

    public class RequestWithClientID : BaseRequest
    {
        public string ClientID;
    
        public RequestWithClientID() : base()
        {
            ClientID = "SINVERSE";
        }
    }

    public class BaseRequest
    {
        public string dateTime;

        public BaseRequest()
        {
            dateTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            Debug.Log(dateTime);
        }
    }
}
