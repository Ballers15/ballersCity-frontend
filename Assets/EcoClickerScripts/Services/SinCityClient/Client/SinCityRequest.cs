using System;
using System.Collections;
using EcoClickerScripts.Services.SinCityClient;
using Newtonsoft.Json;
using SinSity.Domain;

namespace EcoClickerScripts.Services {
    public abstract class SinCityRequest : IDisposable {
        private const string Host = "https://staging.ballers.fun";
        protected string url;
        protected abstract object data { get; set; }
        protected string downloadedData;
        private int attemptsAmount = 5;
       
        public string result { get; private set; }
        public bool success { get; private set; }
        public bool isEmpty { get; private set; }
        
        public void SetAttemptsAmount(int i) {
            attemptsAmount = i;
        }
        
        public IEnumerator Send(RequestType type = RequestType.POST) {
            var counter = 0;
            while (!success && !isEmpty) {
                yield return SendRequest(type);
                if (counter > attemptsAmount) {
                    ServerErrorScreen.Show(false);
                    throw new Exception($"There was an error while trying to connect with the server: {result}");
                }
                counter++;
            }
        }
        
        private IEnumerator SendRequest(RequestType type) {
            var dataString = PrepareJsonFromData();
            var requestSender = new ClientRequestSender(Host+url, dataString, type);
            var usr = User.GetInstance();
            if(!string.IsNullOrEmpty(usr.authKey)) requestSender.AddHeader("Authorization", usr.authKey);
            
            yield return requestSender.Send();

            downloadedData = requestSender.downloadedJson;
            CheckResponse(requestSender);
        }

        private void CheckResponse(ClientRequestSender requestSender) {
            success = requestSender.result == ServerResponseResults.Success;
            if(success) return;
            var serverError = GetDownloadedData(new ServerResponseError());
            if (serverError.code != 2) return;
            isEmpty = true;
            downloadedData = JsonConvert.SerializeObject(new GameDataRespondePDO());
        }

        private string PrepareJsonFromData() {
            return JsonConvert.SerializeObject(data);
        }

        public T GetDownloadedData<T>(T defaultValue) {
            var decryptedSting = downloadedData;
            try {
                var responde = JsonConvert.DeserializeObject<T>(decryptedSting);
                return (responde == null) ? defaultValue : responde;
            }
            catch (Exception e) {
                return defaultValue;
            }
        }

        public void Dispose() {
            downloadedData = null;
        }
    }
}