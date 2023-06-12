using Notification;
using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class ResetPasswordModel : UserModel<ResetPasswordRequest>
    {
        private ResetPasswordRequest resetPasswordRequest;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {

        }

        public override void ProcessRequest(ResetPasswordRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
                NotificationController.ShowLoading(true);
            }
        }

        protected override void SetForm(ResetPasswordRequest data)
        {
            base.SetForm(data);
            //wwwForm.AddField("email", data.email);
            wwwForm.AddField("email", PlayerPrefs.GetString("Temp_Email"));
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"+PlayerPrefs.GetString("Temp_Email"));
            wwwForm.AddField("reset_password_code", data.reset_password_code);
            wwwForm.AddField("password", data.password);
        }

        private bool IsValidData(ResetPasswordRequest data)
        {
            string msg = "";

            //if (Utils.IsEmpty(data.email))
            //{
            //    msg = "Please enter email";
            //}
            if (Utils.IsEmpty(data.reset_password_code))
            {
                msg = "Please enter reset password code";
            }
            else if (Utils.IsEmpty(data.password))
            {
                msg = "Please enter new password";
            }

            if (msg.Length > 0)
            {
                NotificationController.Notify(msg, false);
                return false;
            }

            return true;
        }

        protected override void APISuccessHandler(object data)
        {
            base.APISuccessHandler(data);
            resetPasswordRequest = JsonUtility.FromJson<ResetPasswordRequest>(data.ToString());

            NotificationController.ShowLoading(false);
            NotificationController.Notify("Password changed successfully", false);
            CallBack?.Invoke();
        }

        protected override void APIErrorHandler(string msg)
        {
            NotificationController.ShowLoading(false);
            NotificationController.Notify(msg, false);
            Debug.Log(msg);
        }
    }

    public class ResetPasswordRequest
    {
        public string email;
        public string reset_password_code;
        public string password;
    }

    [Serializable]
    public class ResetPasswordResponse : APIResult<int>
    {
        //{"status":"1",
        //"message":"success",
        //"data":{"id":"2",
        //"username":"username",
        //"first_name":"user",
        //"last_name":"name",
        //"email":"ashwani@lyonstechnologies.co.in",
        //"referral_code":"",
        //"is_active":"1",
        //"created_on":"2022-05-19 19:55:11",
        //"is_verify_registration":"1",
        //"verify_code":null}}
    }
}
