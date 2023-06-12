using Notification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public class UserWalletModel : BaseModel<UserWalletRequest>
    {
        private void Awake()
        {
            Instance = this;
        }

        public override void ProcessRequest(UserWalletRequest data, Action callback)
        {
            base.ProcessRequest(data, callback);
            string url = "https://api.vulcanforged.com/" + api_name + data.walletId + "/3";
            CallAPI(url, data, api_name, API_TYPE.GET);
        }

        protected override void APIResultHandler<T>(STATUS _status, object data)
        {
            data = data.ToString().Replace("success", "status");
            data = data.ToString().Replace("msg", "message");

            base.APIResultHandler<string>(_status, data);
            if (CallBack != null)
            {
                //Debug.Log("callback called");
                CallBack();
            }
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
        }

        protected override void APISuccessHandler(object data)
        {
            if (data.ToString().Substring(data.ToString().IndexOf("data")+6,1) == "\"")
            {
                return;
            }
        
            UserWalletResponse walletResponse = JsonUtility.FromJson<UserWalletResponse>(data.ToString());

            //if (User.purchasedCharacters == null)
            //{
            //    User.purchasedCharacters = new Dictionary<string, PurchasedCharacter>();
            //}

            foreach (var item in walletResponse.data)
            {
                //Debug.Log(item.ipfs_data_json);
                item.ipfs_data_json = item.ipfs_data_json.Replace("Name", "Title");
                                       
                item.SetPurchases();                
            }
        }
    }

    [Serializable]
    public class UserWalletRequest
    {
        public string walletId;
    }

    [Serializable]
    public class UserWalletResponse : APIResult<string>
    {
        public List<Purchases> data;
    }

    [Serializable]
    public class Purchases
    {
        public int id;
        public string owner;
        public string ipfshash;
        public string ipfs_data_json;
        public string time;

        public void SetPurchases()
        {
            try
            {
                purchasedCharacter = JsonUtility.FromJson<PurchasedCharacter>(ipfs_data_json);
            }catch (Exception)
            {
                purchasedCharacter = new PurchasedCharacter();
            }
        }

        public PurchasedCharacter purchasedCharacter;
    }

    [Serializable]
    public class PurchasedCharacter
    {
        public int dappid;
        public string author;
        public string Name;
        public string Title;
        public string Land;
        public string image;

        //following variables are used for game play purpose; they are not coming from server
        public int qty;
        public int gameQty;
        public bool isHeroCharacter;
        public bool isFreeQtyUsed;

        public bool HasEnoughQty => gameQty > 0;

        public void ResetGameQty()
        {       
            gameQty = qty;
        }
    }
}

