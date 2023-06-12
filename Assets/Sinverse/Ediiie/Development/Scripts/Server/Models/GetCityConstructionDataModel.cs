using Ediiie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sinverse
{
    public class GetCityConstructionDataModel : BaseModel<CityConstructionDataRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(CityConstructionDataRequest data, Action<string> callback)
        {
            base.ProcessRequest(data, callback);
            SetForm(data);
            CallAPI(url, wwwForm, api_name, API_TYPE.POST);
        }

        protected override void SetForm(CityConstructionDataRequest data)
        {
            base.SetForm(data);          
            wwwForm.AddField("city", data.city);
        }

        protected override void APISuccessHandler(object data)
        {
            DataCallBack?.Invoke(data.ToString());
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }
    }

    public class CityConstructionDataRequest
    {
        public string city;
    }

    [Serializable]
    public class CityConstructionDataResponse : APIResult<int>
    {
        public List<CityConstructionData> data;
    }    
    
    [Serializable]
    public class CityConstructionData
    {
        public int x;
        public int y;
        public string id;
        public string title;
        public string status;
        public string owner;
        public string userData;
        public string constuction_data;
    }
}
