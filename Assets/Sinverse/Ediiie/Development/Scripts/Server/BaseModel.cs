using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Ediiie.Model
{
    public abstract class BaseModel : MonoBehaviour
    {
        public static Action OnLogout;
        protected static List<object> InitQueue = new List<object>();
        public static Action<STATUS, string> OnDebugLogAPICompleted;      
        public static Action OnInitCompleted;
        public static APIUserData ApiUserData;
        public static string UserToken;
        public static bool IsForceDisconnect;
        
        public static User User => (ApiUserData == null) ? null : ApiUserData.data;

        protected bool ShowLog => ServerConfig.SHOW_LOGS;

        [SerializeField] protected string api_name;
        protected string message;
        protected STATUS status;
        protected Thread _t1;

        protected void SaveOfflineInThread()
        {
            //_t1 = new Thread(SaveOffline);
            //_t1.Start();
            SaveOffline();
        }

        protected virtual void SaveOffline() {} //child class overridden functions call the SaveOffline method of OfflineManager

        protected void AddToInitQueue(object instance)
        {
            if (!InitQueue.Contains(instance))
            {
                InitQueue.Add(instance);
            }
        }

        protected void RemoveFromInitQueue(object instance)
        {
            InitQueue.Remove(instance);

            if (InitQueue.Count == 0)
            {
                OnInitCompleted?.Invoke();
            }
        }

        public static void Logout()
        {
            ApiUserData = null;
            PlayerPrefs.DeleteKey(Constants.PP_IS_AUTHENTICATED);
            PlayerPrefs.DeleteKey(Constants.PP_PLAYER_INFO);
            OnLogout?.Invoke();
        }

        public System.Collections.IEnumerator WaitAndExecuteAPI(Action callback, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            callback?.Invoke();
        }
    }

    public abstract class BaseModel<TRequest> : BaseModel
    {
        public static BaseModel<TRequest> Instance;
        
        protected Action CallBack;
        protected Action<string> DataCallBack;
        protected WWWForm wwwForm;

        protected abstract void APIErrorHandler(string msg);
        protected abstract void APISuccessHandler(object data);

        private APIHandler CreateAPIHandler(string url, WWWForm form, string handlerName = "", API_TYPE type = API_TYPE.POST)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = (handlerName + " API Handler").Trim();
            //Debug.Log(handlerName + " api called");
            APIHandler apiHandler = gameObject.AddComponent<APIHandler>();
            apiHandler.SetData(url, form, type);
            return apiHandler;
        }

        protected virtual void Start()
        {
            Instance = this;
        }
                
        protected string url
        {
            get
            {
                Debug.Log(api_name);
                return string.Format(ServerConfig.BASE_URL, api_name);
            }
        }
                      
        protected void CallAPI(string url, object request, string handlerName, API_TYPE type = API_TYPE.POST)
        {
            CallAPI<string>(url, request, handlerName, type);
        }

        protected void CallAPI<T>(string url, object request, string handlerName, API_TYPE type = API_TYPE.PUT)
        {
            string jsonData = JsonUtility.ToJson(request);
            Debug.Log(handlerName + "," +  jsonData);

            if (ShowLog)
            {
                Debug.Log(jsonData);
                Debug.Log(url);     
            }

    
            WWWForm form = new WWWForm();

            // CallIndirectAPI placed in IndirectCallHelper.cs
            // this is not used now, but kept for future reference
            //url = CallIndirectAPI(isVersion2, jsonData, url, out form);

            APIHandler handler = CreateAPIHandler(url, form, handlerName, type);
            handler.SetData(url, form, type, jsonData);
            handler.CallAPI(APIResultHandler<T>);
        }

        protected void CallAPI(string url, WWWForm form, string handlerName, API_TYPE type = API_TYPE.POST)
        {
            APIHandler handler = CreateAPIHandler(url, form, handlerName, type);
            handler.SetData(url, form, type);
            handler.CallAPI(APIResultHandler<int>);
        }
   
        protected virtual void APIResultHandler<T>(STATUS _status, object data)
        {
            message = "";
            status = _status;
            Debug.Log("this gameobject : " + this.name + ", api executed");
            if (status == STATUS.NETWORK_ERROR)
            {
                message = data.ToString();               
                APIErrorHandler(message);
            }
            else
            {
                status = STATUS.API_ERROR;
                if (!string.IsNullOrEmpty(data.ToString()))
                {
                   // Debug.Log(data.ToString());
                    APIResult<T> result = DeserializeObject<APIResult<T>>(data.ToString());
                    message = result.message;

                    bool isSuccess;
                    if (typeof(T) == typeof(string))
                    {
                        string value = (string)(object)result.status;
                        isSuccess = (value == "true");
                    }
                    else
                    {
                        int value = (int)(object)result.status;
                        isSuccess = (value == 1);
                    }
                
                    if (isSuccess)
                    {
                        APISuccessHandler(data);
                        SaveOfflineInThread();
                        status = STATUS.SUCCESS;
                    }
                    else
                    {
                        APIErrorHandler(result.message);
                    }
                }
                else
                {
                    APIErrorHandler("Empty data received");
                }
            }

            if (OnDebugLogAPICompleted != null)
                OnDebugLogAPICompleted(status, message);
        }

        protected TResponse DeserializeObject<TResponse>(string data)
        {
            return JsonUtility.FromJson<TResponse>(data);
        }

        public virtual void ProcessRequest(TRequest data, Action callback) 
        {
            //if (NetworkDetector.IsNetworkNotReachable)
            //{
            //    NotificationController.Notify("Check Internet Connection", false);
            //    return;
            //}

            this.CallBack = callback;
        }  
        
        public virtual void ProcessRequest(TRequest data, Action<string> callback) 
        {
            //if (NetworkDetector.IsNetworkNotReachable)
            //{
            //    NotificationController.Notify("Check Internet Connection", false);
            //    return;
            //}

            this.DataCallBack = callback;
        }    
        
        protected virtual void SetForm(TRequest data)
        {
            CreateForm();
        }

        private void CreateForm()
        {
            wwwForm = new WWWForm();
        }

    }

    public class APIResult<T>
    {
        public T status;
        public string message;

        protected void SetBase(T status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}

[Serializable]
public class Payload
{
    public string payload;

    public Payload(string encryptedData)
    {
        payload = encryptedData;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
