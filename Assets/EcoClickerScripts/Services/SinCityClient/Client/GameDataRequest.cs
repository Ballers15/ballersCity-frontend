using System;
using Newtonsoft.Json;
using UnityEngine;
using VavilichevGD.Tools;
using Object = System.Object;

namespace EcoClickerScripts.Services {
    public sealed class GameDataRequest : SinCityRequest {
        protected override object data { get; set; }

        public GameDataRequest(string prefKey) {
            data = new GetGameDataRequestPDO {
                key = prefKey
            };
            url = "/string/"+prefKey;
        }
        
        public GameDataRequest(string prefKeyCurrencyData, object gameData) {
            var prefKey = prefKeyCurrencyData;
            var dataJson = JsonConvert.SerializeObject(gameData);
            var encryptedData = SinCityEncryptor.Encrypt(dataJson);
            data = new SaveGameDataRequestPDO {
                key = prefKey,
                value = encryptedData
            };
            url = "/string";
        }

        public T GetGameData<T>(T defaultValue) {
            var respond = GetDownloadedData(new GameDataRespondePDO());
            if (string.IsNullOrEmpty(respond.value)) return defaultValue;
            var encryptedData = SinCityEncryptor.Decrypt(respond.value);
            return JsonConvert.DeserializeObject<T>(encryptedData);
        }
    }
    
    [Serializable]
    public struct GetGameDataRequestPDO {
        public string key; 
    }
    [Serializable]
    public struct GameDataRespondePDO {
        public string value; 
    }
    [Serializable]
    public struct SaveGameDataRequestPDO {
        public string key;
        public string value;
    }
}