using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class SaveUserCreditModel : UserModel<SaveUserCreditRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(SaveUserCreditRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            SetForm(data);
            CallAPI(url, wwwForm, api_name, API_TYPE.POST);
        }

        protected override void SetForm(SaveUserCreditRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("user_id", User.id);
            wwwForm.AddField("amount", data.amount.ToString());
            wwwForm.AddField("transaction_type", data.transactionType);
        }

        protected override void APISuccessHandler(object data)
        {
            SaveUserCreditResponse saveUserCreditResponse = JsonUtility.FromJson<SaveUserCreditResponse>(data.ToString());
            Debug.Log("Saved credits Successfully"+data);
            User.credits = saveUserCreditResponse.data;
            CallBack?.Invoke();
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class SaveUserCreditRequest
    {
        public float amount;
        public int transactionType;
    }

    [Serializable]
    public class SaveUserCreditResponse : APIResult<int>
    {
        public float data;
    }
}
