using Sinverse;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ediiie.Model
{
    public class ServerController : MonoBehaviour
    {
        public static ServerController Instance;
        public static Action OnServerInit;
        
        [SerializeField] private SceneProperties sceneProperties;

        private void Awake()
        {
            BaseModel.OnInitCompleted += OnInitCompleted;
        }

        private void OnDestroy()
        {
            BaseModel.OnInitCompleted -= OnInitCompleted;
        }

        // Start is called before the first frame update
        private void Start()
        {
            Instance = this;

            if (OnServerInit != null)
            {
                OnServerInit();
            }
            else 
            {
                OnInitCompleted();
            }
        }

        private void OnInitCompleted()
        {
            sceneProperties.sceneName = (BaseModel.User != null) ? Constants.Scene.HOME_SCENE : Constants.Scene.LOGIN_SCENE;
            SceneController.LoadScene(sceneProperties);
            Notification.NotificationController.ShowLoading(false);
        }

        public static User UserData
        {
            get
            {
                return BaseModel.ApiUserData.data;
            }
        }

        public static void SendToServer<T>(T data, Action callback = null)
        {
            BaseModel<T>.Instance.ProcessRequest(data, callback);          
        }

        public static void SendToServer<T>(T data, Action<string> callback)
        {
            BaseModel<T>.Instance.ProcessRequest(data, callback);          
        }
    }
}