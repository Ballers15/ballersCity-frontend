using Notification;
using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class ForgetPasswordMode : UserModel<ForgetPasswordRequest>
    {
        private ForgetPasswordResponse forgetPasswordResponse;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {

        }

        public override void ProcessRequest(ForgetPasswordRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
            }
        }

        protected override void SetForm(ForgetPasswordRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("email", data.email);
        }

        private bool IsValidData(ForgetPasswordRequest data)
        {
            string msg = "";

            if (Utils.IsEmpty(data.email))
            {
                msg = "Please enter email";
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
            forgetPasswordResponse = JsonUtility.FromJson<ForgetPasswordResponse>(data.ToString());
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class ForgetPasswordRequest
    {
        public string email;
        public string verify_code;
    }

    [Serializable]
    public class ForgetPasswordResponse : APIResult<int>
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
