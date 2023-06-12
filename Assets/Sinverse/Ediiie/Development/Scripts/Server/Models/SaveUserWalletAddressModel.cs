using Notification;
using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class SaveUserWalletAddressModel : BaseModel<SaveUserWalletAddressRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(SaveUserWalletAddressRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            SetForm(data);
            CallAPI(url, wwwForm, api_name);
            NotificationController.ShowLoading(true);
        }

        protected override void SetForm(SaveUserWalletAddressRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("user_id", User.id);
            wwwForm.AddField("wallet_address", data.wallet_address);
        }

        protected override void APIResultHandler<T1>(STATUS _status, object data)
        {
            base.APIResultHandler<T1>(_status, data);
        }

        protected override void APISuccessHandler(object data)
        {
            Debug.Log(data.ToString());

            SaveUserWalletAddressResponse userWalletLoginResponse = JsonUtility.FromJson<SaveUserWalletAddressResponse>(data.ToString());
            Debug.Log(data.ToString());

            NotificationController.ShowLoading(false);
            CallBack?.Invoke();
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class SaveUserWalletAddressRequest
    {
        public string wallet_address;
    }

    [Serializable]
    public class SaveUserWalletAddressResponse : APIResult<int>
    {

    }
}
