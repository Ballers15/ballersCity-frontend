using System;
using System.Collections;
using SinSity.Analytics;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public delegate void ADSResultsHandler(bool success, ADResult result, string error = "");
    
    public abstract class ADSBehavior {

        #region CONSTANTS

        protected const bool SUCCESS = true;
        protected const bool FAIL = false;

        #endregion


        #region DELEGATES

        public delegate void ADSBehaviorADHandler(ADSBehavior behavior);
        public abstract event ADSBehaviorADHandler OnAdStartedEvent;
        public abstract event ADSBehaviorADHandler OnAdClickedEvent;

        #endregion
        

        public ADSBehavior() {
            Initialize();
        }

        protected abstract void Initialize();

        public abstract bool IsRewardedVideoAvailable();
        public abstract bool IsInterstitialAvailable();

        public virtual void ShowRewardedVideo(ADSResultsHandler callback) {
            Coroutines.StartRoutine(ShowRewardedVideoRoutine(callback));
        }
        
        
        protected abstract IEnumerator ShowRewardedVideoRoutine(ADSResultsHandler callback);


        public virtual void ShowInterstitial(ADSResultsHandler callback = null) {
            Coroutines.StartRoutine(ShowInterstitialRoutine(callback));
        }

        protected abstract IEnumerator ShowInterstitialRoutine(ADSResultsHandler callback);

        
        public virtual void ShowBanner() {
            throw new NotSupportedException();
        }

        public virtual bool IsBannerAvailable() {
            throw new NotSupportedException();
        }
    }
}