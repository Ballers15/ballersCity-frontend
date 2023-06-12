using Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public class ChangePasswordModel : UserModel<ChangePasswordRequest>
    {
        private ChangePasswordResponse changePasswordResponse;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {

        }

        public override void ProcessRequest(ChangePasswordRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
            }
        }

        protected override void SetForm(ChangePasswordRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("email", data.email);
            wwwForm.AddField("old_password", data.old_password);
            wwwForm.AddField("new_password", data.new_password);
        }

        private bool IsValidData(ChangePasswordRequest data)
        {
            string msg = "";

            if (Utils.IsEmpty(data.email))
            {
                msg = "Please enter email";
            }
            else if (Utils.IsEmpty(data.old_password))
            {
                msg = "Please enter old password";
            }
            else if (Utils.IsEmpty(data.new_password))
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
            changePasswordResponse = JsonUtility.FromJson<ChangePasswordResponse>(data.ToString());
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class ChangePasswordRequest
    {
        public string email;
        public string old_password;
        public string new_password;
    }

    [Serializable]
    public class ChangePasswordResponse : APIResult<int>
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
