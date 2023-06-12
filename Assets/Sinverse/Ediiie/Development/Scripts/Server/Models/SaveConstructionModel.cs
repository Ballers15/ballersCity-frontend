using Ediiie.Model;
using System;
using UnityEngine;

namespace Sinverse
{
    public class SaveConstructionModel : BaseModel<SaveConstructionRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(SaveConstructionRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            SetForm(data);
            CallAPI(url, wwwForm, api_name, API_TYPE.POST);
        }

        protected override void SetForm(SaveConstructionRequest data)
        {
            base.SetForm(data);
            wwwForm.AddField("user_id", User.id);
            wwwForm.AddField("x", data.x);
            wwwForm.AddField("y", data.y);
            wwwForm.AddField("constuction_data", data.constuction_data);
        }

        protected override void APISuccessHandler(object data)
        {
            SaveConstructionResponse saveConstructionResponse = JsonUtility.FromJson<SaveConstructionResponse>(data.ToString());
            CallBack?.Invoke();
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class SaveConstructionRequest
    {
        public string x;
        public string y;
        public string constuction_data;
    }

    [Serializable]
    public class SaveConstructionResponse : APIResult<int>
    {
        public string data;
    }
}
