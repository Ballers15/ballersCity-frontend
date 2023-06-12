using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Ediiie.Model
{
    public class APIHandler : MonoBehaviour
    {
        private string m_url;
        private WWWForm m_form;
        private API_TYPE m_type;
        private string m_data;

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private IEnumerator ProcessAPI(Action<STATUS, object> OnCoroutineEnded)
        {
            CheckServerTimeOut(OnCoroutineEnded);
            UnityWebRequest webrequest = null;

            //  Debug.Log(m_url +", "+ m_type.ToString());
            //  Debug.Log(m_form.data.Length);

            switch (m_type)
            {
                case API_TYPE.POST:
                    webrequest = UnityWebRequest.Post(m_url, m_form);
                    break;
                case API_TYPE.GET:
                    webrequest = UnityWebRequest.Get(m_url);
                    break;
                case API_TYPE.PUT:
                    webrequest = UnityWebRequest.Put(m_url, m_data);
                    webrequest.method = UnityWebRequest.kHttpVerbPOST;
                    webrequest.SetRequestHeader("Content-Type", "application/json");
                    webrequest.SetRequestHeader("Accept", "application/json");
                    break;
                default:
                    break;
            }

           
            //user access token is send in all apis that are called after login
            //if (BaseModel.User != null)
            //{
            //    webrequest.SetRequestHeader("AccessToken", BaseModel.User.accessToken);
            //    webrequest.SetRequestHeader("authorization", ServerConfig.AUTH_KEY);
            //}
          
            webrequest.downloadHandler = new DownloadHandlerBuffer();
	    
            
           
            yield return webrequest.SendWebRequest();

#if UNITY_EDITOR
            //  Debug.Log(webrequest.isNetworkError);
            //  Debug.Log(webrequest.isHttpError);
#endif

            object response = null;
            STATUS status = STATUS.SUCCESS;

            if (webrequest.isHttpError || webrequest.isNetworkError)
            {
                response = webrequest.error;
                status = STATUS.NETWORK_ERROR;
            }
            else
            {
                if (ServerConfig.SHOW_LOGS) Debug.Log(webrequest.downloadHandler.text);
                response = webrequest.downloadHandler.text;
                status = STATUS.SUCCESS;
            }

            OnCoroutineEnded(status, response);
            DestroyHandler();
        }

        private IEnumerator StartTimeOutTimer(Action<STATUS, object> OnCoroutineEnded)
        {
            yield return new WaitForSeconds(20f);
            OnCoroutineEnded(STATUS.NETWORK_ERROR, "Server Time Out");
            DestroyHandler();
        }

        private void CheckServerTimeOut(Action<STATUS, object> OnCoroutineEnded)
        {
            StartCoroutine("StartTimeOutTimer", OnCoroutineEnded);
        }

        private void DestroyHandler()
        {
            Destroy(this.gameObject);
        }

        public void SetData(string url, WWWForm form, API_TYPE type, string data= "")
        {
            m_url = url;
            m_form = form;
            m_type = type;
            m_data = data;
        }

        public void CallAPI(Action<STATUS, object> OnCoroutineEnded)
        {
            StartCoroutine(ProcessAPI(OnCoroutineEnded));
        }
    }
}
