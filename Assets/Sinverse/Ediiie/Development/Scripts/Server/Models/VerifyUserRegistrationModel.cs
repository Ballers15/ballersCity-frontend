using Ediiie.Screens;
using Notification;
using System;
using UnityEngine;
using Screen = Ediiie.Screens.Screen;

namespace Ediiie.Model
{
    public class VerifyUserRegistrationModel : UserModel<VerifyUserRegistrationRequest>
    {
        private VerifyUserRegistrationResponse verifyUserRegistrationResponse;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {

        }

        public override void ProcessRequest(VerifyUserRegistrationRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                SetForm(data);
                CallAPI(url, wwwForm, api_name);
                NotificationController.ShowLoading(true);
            }
        }

        protected override void SetForm(VerifyUserRegistrationRequest data)
        {
            base.SetForm(data);
            //wwwForm.AddField("email", data.email);
            wwwForm.AddField("email", PlayerPrefs.GetString("Temp_Email"));
            wwwForm.AddField("verify_code", data.verify_code);
        }

        private bool IsValidData(VerifyUserRegistrationRequest data)
        {
            string msg = "";
            data.email = PlayerPrefs.GetString("Temp_Email");
            //if (Utils.IsEmpty(data.email))
            //{
            //    msg = "Please enter email";
            //}
            //else if(!Utils.IsValidEmail(data.email))
            //{
            //    msg = "Please enter valid email/username";
            //}
            if (Utils.IsEmpty(data.verify_code))
            {
                msg = "Please enter verify code";
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

        protected override void APISuccessHandler(object data)
        {
            base.APISuccessHandler(data);
            Debug.Log(data.ToString());

            NotificationController.Notify("You have signed up successfully.", false);
            NotificationController.ShowLoading(false);
            CallBack?.Invoke();
        }

        protected override void APIErrorHandler(string msg)
        {
            base.APIErrorHandler(msg);
            Debug.Log(msg);
        }
    }

    public class VerifyUserRegistrationRequest
    {
        public string email;
        public string verify_code;
    }

    [Serializable]
    public class VerifyUserRegistrationResponse : APIResult<int>
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
