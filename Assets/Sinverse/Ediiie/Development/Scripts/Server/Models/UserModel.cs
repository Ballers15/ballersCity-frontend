using Notification;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Model
{
    public abstract class UserModel<T> : BaseModel<T>
    {
    
        [SerializeField] private User user; //used to check data coming from server from inspector
        
        private List<string> walletsProcessed;
        private int walletResponseCnt;
        protected bool isAutoLogin = false;
        
        protected override void APIResultHandler<T1>(STATUS _status, object data)
        {
            base.APIResultHandler<int>(_status, data);
        }

        protected override void APISuccessHandler(object data)
        {
            ApiUserData = DeserializeObject<APIUserData>(data.ToString());
           // Debug.Log(ApiUserData.data.userName);           
        }

        protected override void APIErrorHandler(string msg)
        {
            Debug.Log(msg);
            NotificationController.Notify(msg, false);
        }

        public void AfterLoginSuccess()
        {
            user = ApiUserData.data;
            PlayerData.Instance.playerName = ApiUserData.data.username;
            PlayerData.Save();

            walletResponseCnt = 0;
     
            NotificationController.ShowLoading(false);
         
            RemoveFromInitQueue(STATUS.SUCCESS);
        }

        protected void RemoveFromInitQueue(STATUS status)
        {
            if (isAutoLogin)
            {
                isAutoLogin = false;
            }
            else if (status == STATUS.SUCCESS)
            {
                if (CallBack != null)
                {
                    CallBack();
                }
            }

            RemoveFromInitQueue(this);
        }
    }

    [Serializable]
    public class APIUserData : APIResult<string>
    {
        public User data;
    }

    [Serializable]
    public class User
    {
        //new values
        public string accessToken;
        public string twoFactorSecretKey;
        public string id;
        public string username;
        public string email;
        public string isVerified;
        public string createdDate;
        public int score;
        public float credits;
        public bool hasGameAccess;
        public string wallet_address;
        
        public UserProfile userProfile;
        public UserStats userStats;
       
    }

    [Serializable]
    public class Wallet
    {
        public string walletID;
        public string secretType;
        public string walletType;
        public string symbol;
        public string address;
        public string balance;
        public string alias;
        public string description;
    }

    [Serializable]
    public class UserProfile
    {
        public int userID;
        public string userName;
        public string firstName;
        public string lastName;
        public string image;
        public string email;
        public string createdDate;
        public string isVerified;
        public string companionName;
        public string companionImage;

        public Sprite imageSprite;

        public override string ToString()
        {
            return string.Format("Username : {0} \n First Name : {1} \n Last Name : {2} \n Email : {3} \n Created Date : {4} \n Companion Name : {5}", userName, firstName, lastName, email, createdDate, companionName);
        }
    }

    [Serializable]
    public class UserXPData
    {
        public int xPBalance;
        public int lavaBalance;
        public int monthlyLava;
        public int lavaRank;
        public int credits;
    }
        
    public class UserTokenRequest
    {
        public string user_id;
    }
}