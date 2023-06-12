using Notification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserStatsModel : UserModel<UserStatsRequest>
    {
        public override void ProcessRequest(UserStatsRequest data, Action callback)
        {
            if (callback == null)
            {
                AddToInitQueue(this);
            }

            base.ProcessRequest(data, callback);           
            CallAPI(url, data, api_name, API_TYPE.POST);
        }

        protected override void APIResultHandler<T>(STATUS _status, object data)
        {
            base.APIResultHandler<string>(_status, data);
            if (CallBack == null)
            {
                RemoveFromInitQueue(this);
            }
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log("error profile model : ================= " + msg);
        }

        protected override void APISuccessHandler(object data)
        {
            if (ShowLog) Debug.Log("profile model : ================= "+ data);
            UserStatsResponse statsResponse = JsonUtility.FromJson<UserStatsResponse>(data.ToString());
            User.userStats = statsResponse.data;
        }
    }

    public class UserStatsRequest : BaseRequest
    {
    }

    public class UserStatsResponse : APIResult<string>
    {
        public UserStats data;
    }

    [Serializable]
    public class UserStats
    {
        public float xPBalance;
        public float lavaBalance;
        public float monthlyLava;
        public int lavaRank;
    }
}