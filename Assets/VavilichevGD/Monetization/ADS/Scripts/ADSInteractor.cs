using System.Collections;
using SinSity.Analytics;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization.AppLovin;

namespace VavilichevGD.Monetization {
    public class ADSInteractor : Interactor {

        #region DELEGATES

        public delegate void ADSInteractorAdHandler(ADSInteractor interactor);
        public event ADSInteractorAdHandler OnAdStartedEvent;
        public event ADSInteractorAdHandler OnAdClickedEvent;

        #endregion

        protected ADSBehavior behavior;
        protected ADSRepository adsRepository;

        public bool isActive => adsRepository.stateCurrent.isActive;

        public override bool onCreateInstantly { get; } = true;

        private bool adAvailable;

        protected override void Initialize() {
            base.Initialize();
            this.adsRepository = this.GetRepository<ADSRepository>();
            behavior = new ADSBehaviorAppLovin();
            behavior.OnAdStartedEvent += delegate { this.OnAdStartedEvent?.Invoke(this); };
            behavior.OnAdClickedEvent += delegate { this.OnAdClickedEvent?.Invoke(this); };
            
            ADS.Initialize(this);
        }

        protected override IEnumerator InitializeRoutineNew() {
            var checker = new ADSRemoteChecker();
            yield return checker.CheckAdAvailability();
            this.adAvailable = checker.isAvailable;
        }

        public void ShowRewardedVideo(ADSResultsHandler callback) {
            behavior.ShowRewardedVideo(callback);
        }

        public void ShowInterstitial(ADSResultsHandler callback = null) {
            if (!isActive) {
                callback?.Invoke(false, ADResult.ad_disabled, "ADS disabled");
                return;
            }
            
            behavior.ShowInterstitial(callback);
        }

        public void ShowBanner() {
            if (!isActive)
                return;
            
            behavior.ShowBanner();
        }

        public void ActivateADS() {
            adsRepository.ActivateADS();
        }

        public void DeactivateADS() {
            adsRepository.DeactivateADS();
        }

        public bool IsRewardedVideoAvailable() {
            return this.behavior.IsRewardedVideoAvailable() && this.adAvailable;
        }

        public bool IsInterstitialAvailable() {
            return this.behavior.IsInterstitialAvailable() && this.adAvailable;
        }

        public bool IsBannerAvailable() {
            return this.behavior.IsBannerAvailable() && this.adAvailable;
        }
    }
}