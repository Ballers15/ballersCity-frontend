using UnityEngine;

namespace VavilichevGD.Monetization.AppLovin {
    public class ADSSettingsAppLovin : ScriptableObject {
        [SerializeField] private string m_apiKey;
        [SerializeField] private string m_interstitialAdUnitId = "YOUR_INTERSTITIAL_AD_UNIT_ID";
        [SerializeField] private string m_rewardedVideoAdUnitId = "YOUR_REWARDEDD_VIDEO_AD_UNIT_ID";
        [SerializeField] private int m_maxAttemptCount = 6;
        [SerializeField] private float m_breakTime = 2f;
        


        public string apiKey => this.m_apiKey;
        public string interstitialAdUnitId => this.m_interstitialAdUnitId;
        public string rewardedVideoAdUnitId => this.m_rewardedVideoAdUnitId;
        public int maxAttemptCount => this.m_maxAttemptCount;
        public float breakTime => this.breakTime;
    }
}