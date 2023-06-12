using UnityEngine;

namespace VavilichevGD.Monetization.AdMob {
    public class ADSSettingsAdMob : ScriptableObject {

        [Header("Android:")] 
        [SerializeField] private string m_androidAppId;
        [SerializeField] private string m_androidInterstitialId;
        [SerializeField] private string m_androidRewardedVideoId;
  
        [Header("iOS:")]
        [SerializeField] private string m_iOSAppId;
        [SerializeField] private string m_iOSInterstitialId;
        [SerializeField] private string m_iOSRewardedVideoId;

        [Space]
        [SerializeField] private float m_breakTime = 5f;
        [SerializeField] private bool m_testMode;

        
        private const string REWARDED_TEST_ID_ANDROID = "ca-app-pub-3940256099942544/5224354917";
        private const string INTERSTITIAL_TEST_ID_ANDROID = "ca-app-pub-3940256099942544/1033173712";
        private const string REWADEDRD_TEST_ID_IOS = "ca-app-pub-3940256099942544/1712485313";
        private const string INTERSTITIAL_TEST_ID_IOS = "ca-app-pub-3940256099942544/4411468910";
        

        public string appId => GetAppId();
        public string interstitialId => GetInterstitialId();
        public string rewardedVideoId => GetRewardedVideoId();
        public bool testMode => m_testMode;
        public float breakTime => m_breakTime;


        private string GetAppId() {
#if UNITY_ANDROID
            return m_androidAppId;
#elif UNITY_IOS
            return m_iOSAppId;
#endif
            return "";
        }

        private string GetInterstitialId() {
#if UNITY_ANDROID
            return m_testMode ? INTERSTITIAL_TEST_ID_ANDROID : m_androidInterstitialId;
#elif UNITY_IOS
            return testMode ? INTERSTITIAL_TEST_ID_IOS : m_iOSInterstitialId;
#endif
            return "";
        }

        private string GetRewardedVideoId() {
#if UNITY_ANDROID
            return m_testMode ? REWARDED_TEST_ID_ANDROID : m_androidRewardedVideoId;
#elif UNITY_IOS
            return testMode ? REWADEDRD_TEST_ID_IOS : m_iOSRewardedVideoId;
#endif
            return "";
        }
    }
}