using Notification;
using System;

namespace Ediiie.Model
{
    public class TwoFactorModel : UserModel<TwoFactorRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
        }
        
        public override void ProcessRequest(TwoFactorRequest data, Action callback)
        {
            data.twoFactorSecretKey = User.twoFactorSecretKey;
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                //SetForm(data);
                CallAPI(url, data, api_name);
            }
        }

        private bool IsValidData(TwoFactorRequest credentials)
        {
            string msg = "";

            if (Utils.IsEmpty(credentials.twoFactorCode))
            {
                msg = "Please provide activation code";
            }
        
            if (msg.Length > 0)
            {
                NotificationController.Notify(msg, false);
                return false;
            }

            return true;
        }

        protected override void APIResultHandler<T>(STATUS _status, object data)
        {
            base.APIResultHandler<string>(_status, data);
        }

        protected override void APISuccessHandler(object data)
        {
            base.APISuccessHandler(data);
            //Debug.Log(data.ToString());
            base.AfterLoginSuccess();
        }

        protected override void APIErrorHandler(string msg)
        {
            NotificationController.Notify(msg, false);
        }
    }


    public class TwoFactorRequest : BaseRequest
    {
        public string twoFactorSecretKey;
        public string twoFactorCode;
    }
}

