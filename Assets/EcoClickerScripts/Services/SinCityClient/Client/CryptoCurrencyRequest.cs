using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Tools;

namespace EcoClickerScripts.Services {
    public sealed class CryptoCurrencyRequest : SinCityRequest {
        protected override object data { get; set; }

        public CryptoCurrencyRequest() {
            /*var user = User.GetInstance();
            data = new BaseGetRequestPDO {
                login = user.encryptedLogin,
                command = "GET"
            };
            url = "urlGetCryptoCurrency";*/
        }
        
        public CryptoCurrencyRequest(float amount) {
            /*var user = User.GetInstance();
            data = new SetCryptoCurrencyPDO {
                login = user.encryptedLogin,
                command = "SET",
                value = amount
            };
            url = "urlSetCryptoCurrency";*/
        }

        public float GetAmount() {
            var decryptedData = SinCityEncryptor.Decrypt(downloadedData);
            return float.Parse(decryptedData);
        }
    }
    
    public struct BaseGetRequestPDO {
        public string login;
        public string command;
    }
    
    public struct SetCryptoCurrencyPDO {
        public string login;
        public string command;
        public float value;
    }
}