using System;
using System.Collections;
using EcoClickerScripts.Services;
using Ediiie.Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;
using Random = System.Random;
using User = SinSity.Domain.User;

public class LoginForm : MonoBehaviour {
    [SerializeField] private Text errorText;
    [SerializeField] private Image splashScreen;
    [SerializeField] private string gameSceneName;
    
    private bool isRequestNotInProcess = true;
    private string userAccessToken;

    private const string PLAYER_ID = "playerId";
    //private Ediiie.Model.User sinCityMetaverseUser;
    
    private void OnEnable() {
        //sinCityMetaverseUser = BaseModel.User;
        //Debug.Log($"LOGIN USERID: {sinCityMetaverseUser.id}");
        if (HasCredentials()) {
            Coroutines.StartRoutine(LoadGameWithSavedCredentials());
            return;
        }

        GenerateAndSaveId();
        //errorText.gameObject.SetActive(true);
    }
    
    private bool HasCredentials() {
        var playerId = PlayerPrefs.GetString(PLAYER_ID);
        return !string.IsNullOrEmpty(playerId);
    }

    private IEnumerator LoadGameWithSavedCredentials() {

        #if UNITY_EDITOR
             PlayerPrefs.SetString("Account","0xd946F28962A96C45d9Bc16F16ca50d8350296A4E");
             var playerId = PlayerPrefs.GetString("Account");//PlayerPrefs.GetString(PLAYER_ID);
             Debug.Log("playerId : " + playerId);
        #else
            var playerId = PlayerPrefs.GetString("Account");
            Debug.Log("playerId : " + playerId);
        #endif

        var request = new UserLoginRequest(playerId);

        yield return request.Send();
        var authRespondePdo = request.GetDownloadedData(new AuthRespondePDO());
        if (!request.success || string.IsNullOrEmpty(authRespondePdo.accessToken)) {
            splashScreen.gameObject.SetActive(false);
            errorText.gameObject.SetActive(true);
            yield break;
        }
        userAccessToken = authRespondePdo.accessToken;
        Debug.Log("userAccessToken : " + userAccessToken);
        var gameUser = User.GetInstance();
        gameUser.SetUserId(playerId);
        gameUser.SetAuthKey(userAccessToken);

        string url = "https://dev-api.ballers.fun/users/v1/token/balance?walletAddress="+PlayerPrefs.GetString("Account");
        WWW www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            Root root = JsonConvert.DeserializeObject<Root>(www.text);
            float dataFloat = root.GetFloatData();
            Debug.Log("Data: " + dataFloat.ToString("0.00"));
            PlayerPrefs.SetFloat("tokens",root.GetFloatData());
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }

        LoadGameScene();
    }

    [Serializable]
    public class Root
    {
        [SerializeField]
        public string action { get; set; }

        [SerializeField]
        public int status { get; set; }

        [SerializeField]
        public string message { get; set; }

        [SerializeField]
        public string data { get; set; }

        [SerializeField]
        public bool error { get; set; }

        public float GetFloatData()
        {
            if (float.TryParse(data, out float result))
            {
                //return (float)Math.Round(result, 2);
                return result;
            }
            return 0f;
        }
    }

    private void GenerateAndSaveId() {
        var rndId = UnityEngine.Random.Range(111111, 999999);
        PlayerPrefs.SetString(PLAYER_ID, rndId.ToString());
        Coroutines.StartRoutine(LoadGameWithSavedCredentials());
    }

    private void LoadGameScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }
}

[Serializable]
internal struct UserCredentials {
    public string email;
    public string authCode;
    public string token;
}
