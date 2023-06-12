using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace EcoClickerScripts.Services  {
    public sealed class UserLoginRequest : SinCityRequest {
        protected override object data { get; set; }
        
        public UserLoginRequest(string login) {
            data = new LoginRequestPDO {
                userId = login,
            };
            url = "/login";
        }
        
        public UserLoginRequest(string login, string code) {
            data = new AuthRequestPDO {
                email = login,
                code = code
            };
            url = "/login/verify";
        }
    }
    
    [Serializable]
    public struct LoginRequestPDO {
        public string userId;
    }
    [Serializable]
    public struct AuthRequestPDO {
        public string email;
        public string code;
    }
    [Serializable]
    public struct AuthRespondePDO {
        public string accessToken;
    }
}