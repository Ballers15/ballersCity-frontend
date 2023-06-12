using System;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserLavaModel : UserModel<UserLavaRequest>
    {
        private UserLavaRequest lastRequestData;

        public override void ProcessRequest(UserLavaRequest data, Action callback = null)
        {
            lastRequestData = data;
            base.ProcessRequest(data, callback);           
            CallAPI(url, data, api_name, API_TYPE.POST);
        }
        
        protected override void APIErrorHandler(string msg)
        {
            Debug.Log("error lava model : ================= " + msg);

            if (msg.Contains("Expired"))
            {
                //ServerController.SendToServer(new RefreshTokenRequest(User.accessToken), RetrySaveLava);
            }
        }

        private void RetrySaveLava()
        {
            ProcessRequest(lastRequestData);
        }

        protected override void APISuccessHandler(object data)
        {
            if (ShowLog) Debug.Log("lava model : ================= "+ data);
            //get updated player xp
            ServerController.SendToServer(new UserStatsRequest(), OnStatsReceived);
        }

        protected void OnStatsReceived()
        {
        }
    }

    public class UserLavaRequest : BaseRequest
    {
        public int amount;
        public int source;

        public UserLavaRequest(int amount)
        {
            this.amount = amount;
            this.source = (int)LavaSource.FORGEARENA;
        }
    }

    public enum LavaSource { NONE, XP_APPRECIATION, VULCANVERSE, BERSERK, FORGEARENA, BLOCKBABIES, CHESS }
}