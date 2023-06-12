using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace EcoClickerScripts.Services.SinCityClient {
    public class UnityWebRequestStub {
        private string method;
        private string json;
        private string url;
        public DownloadHandlerStub downloadHandler;
        public UnityWebRequest.Result result;

        private UnityWebRequestStub(string url, string method, string json) {
            this.url = url;
            this.method = method;
            this.json = json;
        }

        public static UnityWebRequestStub Post(string url, string json) {
            return new UnityWebRequestStub(url, "POST", json);
        }

        public IEnumerator SendWebRequest() {
            result = UnityWebRequest.Result.InProgress;
            var rndTime = Random.Range(1f, 1f);
            FakeServerRequestsProcessor.ProcessSaveRequest(url, json);
            
            yield return new WaitForSecondsRealtime(rndTime);

            result = UnityWebRequest.Result.Success;
            var fakeDownloadedData = FakeServerResponse.GetFakeServerResponse(url);
            downloadHandler = new DownloadHandlerStub(fakeDownloadedData);
        }
    }

    public class DownloadHandlerStub {
        public string text { get; private set; }
        
        public DownloadHandlerStub(string downloadedData) {
            text = downloadedData;
        }
    }
}