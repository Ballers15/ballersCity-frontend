using Orego.Util;
using System;
using SinSity.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIScreenRenovationLoading : UIScreenAnim<UIProperties> {
        
        #region DELEGATES

        public delegate void RenovationScreenHandler(UIScreenRenovationLoading screen);
        public event RenovationScreenHandler OnScreenReadyForRenovation;

        #endregion

        [SerializeField] private TextMeshProUGUI textRenovationLevel;

        public override void Show() {
            base.Show();

            ModernizationInteractor modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            modernizationInteractor.OnModernizationCompleteEvent += OnModernizationComplete;
        }

        private void Setup()
        {

        }

        private void OnModernizationComplete(ModernizationInteractor interactor, object sender) {
            interactor.OnModernizationCompleteEvent -= OnModernizationComplete;
            this.textRenovationLevel.text = $"{interactor.modernizationData.multiplierInPercent}%";
            
            Transform parent = this.textRenovationLevel.transform.parent;
            parent.Recalculate();
            
            this.Hide();
            //ADS.ShowInterstitial("modernization_completed_ad");
        }

        private void Handle_ReadyForRenovation() {
            this.OnScreenReadyForRenovation?.Invoke(this);
        }
        
    }
}