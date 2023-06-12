using Orego.Util;
using SinSity.Domain;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.Core.Content.Currencies {
    public class UITutorialStageCurrencies : UITutoriaCertainStage<TutorialStageCurrencies> {
        [SerializeField] private int m_collectCurrencyTimes;
        [SerializeField] private Transform m_anticlicker;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupIntro;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupCurrencies;
        [SerializeField] private UITutorialStageUpgradeBuildingPopup m_popupCards;

        private int collectCurrencyCount;
        private UIPopupCardsCollection popupCards;

        private void Awake() {
            m_popupIntro.OnClickEvent.AddListener(OnIntroBtnClicked);
            m_popupCurrencies.OnClickEvent.AddListener(OnCurrenciesBtnClicked);
            m_popupCards.OnClickEvent.AddListener(OnCardsPopupClicked);
            m_anticlicker.SetInvisible();
            m_popupIntro.SetInvisible();
            m_popupCurrencies.SetInvisible();
            m_popupCards.SetInvisible();
        }
        
        protected override void Start() {
            base.Start();
            IdleObject.OnIdleObjectCurrencyCollected += OnIdleObjectCurrencyCollected;
        }

        private void OnIdleObjectCurrencyCollected(object sender, BigNumber collectedcurrency) {
            collectCurrencyCount++;

            if (collectCurrencyCount < m_collectCurrencyTimes) return;
    
            SetCameraRenderLayerToSpecial();
            IdleObject.OnIdleObjectCurrencyCollected -= OnIdleObjectCurrencyCollected;
            m_anticlicker.SetVisible();
            m_popupIntro.SetVisible();
        }
        
        private void OnIntroBtnClicked() {
            m_popupIntro.SetInvisible();
            m_popupCurrencies.SetVisible();
        }
        
        private void OnCurrenciesBtnClicked() {
            m_popupCurrencies.SetInvisible();
            m_popupCards.SetVisible();
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            popupCards = uiInteractor.GetUIElement<UIPopupCardsCollection>();
            popupCards.Show();
        }
        
        private void OnCardsPopupClicked() {
            popupCards.HideInstantly();
            m_popupCards.SetInvisible();
            SetCameraRenderLayerToDefault();
        }
    }
}