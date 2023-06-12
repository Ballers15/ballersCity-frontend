using Orego.Util;
using SinSity.Analytics;
using SinSity.Meta.Rewards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIPopupFortuneWheelReward : UIPopupAnim<UIPopupFortuneWheelReward.Properties, UIPopupArgs> {

        private Reward rewardCurrent;


        #region LIFECYCLE
        private void OnEnable() {
            this.properties.buttonGet.AddListener(this.OnGetButtonClicked);
        }

        private void OnDisable() {
            this.properties.buttonGet.RemoveListener(this.OnGetButtonClicked);
        }
        
        protected override void OnPostShow() {
            base.OnPostShow();
            SFX.PlaySFX(this.properties.sfxAppear);
        }

        
        #endregion


        private void LogPopupClosedCompletely() {
            FortuneWheelAnalytics.LogPopupFortuneWheelRewardClosed();
        }
        

        public void SetupSimple(Reward reward) {
            this.properties.visual.SetupAsSimple();
            this.Setup(reward);
        }

        public void SetupJackpot(Reward reward) {
            this.properties.visual.SetupAsJackpot();
            this.Setup(reward);
        }


        private void Setup(Reward reward) {
            this.rewardCurrent = reward;
            this.properties.imgIcon.sprite = this.rewardCurrent.info.GetSpriteIcon();

            var rewardInfoEcoClicker = this.rewardCurrent.GetRewardInfo<RewardInfoEcoClicker>();
            var countString = rewardInfoEcoClicker.GetCountToString();
            this.properties.textCount.text = $"x{countString}";

            this.SetupTitle(reward);
        }

        private void SetupTitle(Reward reward) {
            var info = reward.GetRewardInfo<RewardInfoTimeBooster>();
            var isTimeBoosterReward = info != null;
            string title;
    
            if (isTimeBoosterReward)
                title = info.GetTitle();
            else
                title = Localization.GetTranslation(reward.info.GetTitle());
            
            this.properties.textTitle.text = title;
        }


        #region EVENTS

        private void OnGetButtonClicked() {
            SFX.PlayClosePopup();
            this.rewardCurrent.Apply(this, true);
            this.LogPopupClosedCompletely();
            this.Hide();
        }

        #endregion
        

        [System.Serializable]
        public class Properties : UIProperties {
            public Image imgIcon;
            public UIPopupFortuneWheelRewardCardVisual visual;
            public Text textTitle;
            public TextMeshProUGUI textCount;
            public Button buttonGet;
           
            
            [Space] 
            public AudioClip sfxAppear;
        }
    }
}