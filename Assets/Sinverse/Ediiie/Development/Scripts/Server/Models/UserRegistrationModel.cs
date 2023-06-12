using Notification;
using Sinverse;
using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserRegistrationModel : UserModel<RegisterUserRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
        }

        public override void ProcessRequest(RegisterUserRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
                NotificationController.ShowLoading(true);
            }
        }

        protected override void SetForm(RegisterUserRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("email", data.email);
            wwwForm.AddField("username", data.username);
            wwwForm.AddField("password", data.password);
            wwwForm.AddField("first_name", data.first_name);
            wwwForm.AddField("last_name", data.last_name);
            wwwForm.AddField("referral_code", "");
        }

        private bool IsValidData(RegisterUserRequest data)
        {
            string msg = "";

            if (Utils.IsEmpty(data.first_name))
            {
                msg = "Please enter first name";
            }
            else if (Utils.IsEmpty(data.last_name))
            {
                msg = "Please enter last name";
            }
            else if (Utils.IsEmpty(data.username))
            {
                msg = "Please enter username";
            }
            else if (Utils.IsEmpty(data.email))
            {
                msg = "Please enter email";
            }
            else if(!SinverseUtils.IsValidEmail(data.email))
            {
                msg = "Please enter valid email";
            }
            else if (Utils.IsEmpty(data.password))
            {
                msg = "Please enter password";
            }

            if (msg.Length > 0)
            {
                NotificationController.Notify(msg, false);
                return false;
            }

            PlayerPrefs.SetString("Temp_Email", data.email);
            return true;
        }

        protected override void APIResultHandler<T1>(STATUS _status, object data)
        {
            base.APIResultHandler<T1>(_status, data);
        }

        protected override void APIErrorHandler(string msg)
        {
            base.APIErrorHandler(msg);
        }

        protected override void APISuccessHandler(object data)
        {
            base.APISuccessHandler(data);
            Debug.Log(data.ToString());

            NotificationController.ShowLoading(false);
            CallBack?.Invoke();
        }
    }

    public class RegisterUserRequest
    {
        public string email;
        public string username;
        public string password;
        public string first_name;
        public string last_name;
        public string referral_code;
    }
}