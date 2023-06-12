using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Tools;

namespace EcoClickerScripts.Services.SinCityClient {
    public class ClientRequestSender {
        private UnityWebRequest request;
        
        public ServerResponseResults result { get; private set; }
        public string downloadedJson { get; private set; }

        public ClientRequestSender(string url, string json, RequestType type) {
            //var encryptedJson = SinCityEncryptor.Encrypt(json);
            var encryptedJson = Encoding.UTF8.GetBytes(json);
            request = new UnityWebRequest(url, type.ToString()) {
                uploadHandler = new UploadHandlerRaw(encryptedJson),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");
        }
        
        public IEnumerator Send() {
            result = ServerResponseResults.InProgress;
            
            yield return request.SendWebRequest();

            downloadedJson = request.downloadHandler.text;
            Enum.TryParse<ServerResponseResults>(request.result.ToString(), out var parsedEnum);
            result = parsedEnum;
            //CheckErrors();
        }

        public void AddHeader(string key, string value) {
            request.SetRequestHeader(key, value);
        }

        private void CheckErrors() {
            if (result == ServerResponseResults.Success) return;
            
            throw new Exception($"There was an error while trying to connect with the server: {result.ToString()}");
        }
    }

    public enum ServerResponseResults {
        InProgress,
        Success,
        ConnectionError,
        ProtocolError,
        DataProcessingError, 
    }

    public enum RequestType {
        POST,
        GET
    }

    [Serializable]
    public struct ServerResponseError {
        public int code;
        public string message;
        public object details;
    }
}