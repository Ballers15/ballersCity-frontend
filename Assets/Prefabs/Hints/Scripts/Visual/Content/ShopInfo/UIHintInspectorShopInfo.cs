using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.Core
{
    public sealed class UIHintInspectorShopInfo : UIHintCertainInspector<HintInspectorShopInfo>
    {
        [SerializeField]
        private UIHintShopInfoCase uiHintCaseInfo;

        [SerializeField]
        private UIHintShopInfoTimeBooster uiHintTimeBoosterInfo;

        private void Awake()
        {
            this.uiHintCaseInfo.SetInvisible();
            this.uiHintTimeBoosterInfo.SetInvisible();
            this.uiHintCaseInfo.OnClickedEvent.AddListener(this.OnHideCaseInfo);
            this.uiHintTimeBoosterInfo.OnClickedEvent.AddListener(() =>
            {
                this.uiHintTimeBoosterInfo.SetInvisible();
                
                TutorialAnalytics.LogEventInMin("hint_shop_window_second_hint_close");
            });
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            UIWidgetBtnNavigateShop.OnPopupOpenedEvent += this.OnShopPopupOpened;
        }

        private void OnShopPopupOpened()
        {
            UIWidgetBtnNavigateShop.OnPopupOpenedEvent -= this.OnShopPopupOpened;
            this.uiHintCaseInfo.SetVisible();
            ScrollToCases();
            
            TutorialAnalytics.LogEventInMin("hint_shop_window_first_time");
        }

        private void ScrollToCases() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIPopupShop popupShop = uiInteractor.GetUIElement<UIPopupShop>();
            popupShop.ScrollToCases();
        }

        private void OnHideCaseInfo()
        {
            this.uiHintCaseInfo.SetInvisible();
            this.hintInspector.NotifyAboutShopInfoViewed();
            this.uiHintTimeBoosterInfo.SetVisible();
            
            TutorialAnalytics.LogEventInMin("hint_shop_window_first_hint_close");
        }
    }
}