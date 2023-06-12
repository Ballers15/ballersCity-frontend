using Notification;
using System;

namespace Ediiie.Model
{
    public class UserActivationModel : BaseModel<ActivationRequest>
    {
        public static Action<STATUS, string> OnAPICompleted;
  
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
        }
        
        public override void ProcessRequest(ActivationRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            if (IsValidData(data))
            {
                //SetForm(data);
                CallAPI(url, data, api_name);
            }
        }

        protected override void SetForm(ActivationRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("ActivationCode", data.activationCode);
            wwwForm.AddField("PinCode", data.pinCode);
        }

        private bool IsValidData(ActivationRequest credentials)
        {
            string msg = "";

            if (Utils.IsEmpty(credentials.activationCode))
            {
                msg = "Please provide activation code";
            }
            else if (Utils.IsEmpty(credentials.pinCode))
            {
                msg = "Please provide pin code";
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
            //Debug.Log(data.ToString());
            OnAPICompleted?.Invoke(STATUS.SUCCESS, "");
        }

        protected override void APIErrorHandler(string msg)
        {
            NotificationController.Notify(msg, false);
        }
    }


    public class ActivationRequest
    {
        public string activationCode;
        public string pinCode;
    }
}

