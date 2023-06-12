using UnityEngine;

namespace Ediiie.Model
{
    public class ServerConfig
    {
        public const string BASE_URL = "https://staging.ediiie.com/sinverse/api/{0}"; //Sinverse base url
        public const bool SHOW_LOGS = true;
        public const bool IS_TEST_MODE = false;
        public static string CheckInteretURL = "https://www.google.co.in";
      
        public static int AppPlatform
        {
            get
            {
                Platform platform = Platform.ANDROID;
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsPlayer: 
                        platform = Platform.WINDOWS;
                        break;

                    case RuntimePlatform.Android:
                        platform = Platform.ANDROID;
                        break;

                    case RuntimePlatform.IPhonePlayer:
                        platform = Platform.IOS;
                        break;
                }

                return (int)platform;
            }
        }
    }

    public enum Platform {ANDROID=1, IOS, WINDOWS};
    public enum STATUS { NETWORK_ERROR, API_ERROR, SUCCESS };
    public enum API_TYPE { POST, GET, PUT };
}
