using Orego.Util;
using SinSity.Meta;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetButtonFortuneWheel : UIWidget<UIWidgetButtonFortuneWheel.Properties> {

        #region DELEGATES

        public delegate void UIWidgetButtonFortuneWheelHandler(UIWidgetButtonFortuneWheel widget);

        public event UIWidgetButtonFortuneWheelHandler OnClickedEvent;

        #endregion

        private FortuneWheelInteractor interactor;
        private UIController uiController;


        #region LIFECYCLE

        protected override void OnStart() {
            base.OnStart();

            var uiInteractor = this.GetInteractor<UIInteractor>();
            this.uiController = uiInteractor.uiController;

            this.interactor = this.GetInteractor<FortuneWheelInteractor>();
            if (this.interactor.dataWasDefined)
                this.Setup();
            else
                this.interactor.OnDataDefinedEvent += this.OnInteractorDataDefined;
        }

        

        private void Setup() {
            var isWheelUnlocked = this.interactor.isUnlocked;
            if (!isWheelUnlocked) {
                this.HideInstantly();
                this.interactor.OnFortuneWheelUnlockedEvent += this.OnFortuneWheelUnlocked;
            }
            else {
                this.properties.button.AddListener(this.OnClick);
            }
        }

        private void OnDestroy() {
            this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelUnlocked;
            this.properties.button.RemoveListener(this.OnClick);
        }

        #endregion


        #region EVENTS
        
        private void OnInteractorDataDefined(FortuneWheelInteractor fortuneWheelInteractor) {
            this.interactor.OnDataDefinedEvent -= this.OnInteractorDataDefined;
            this.Setup();
        }
        
        private void OnFortuneWheelUnlocked(FortuneWheelInteractor fortuneWheelInteractor) {
            this.interactor.OnFortuneWheelUnlockedEvent -= this.OnFortuneWheelUnlocked;
            this.Show();
        }

        private void OnClick() {
            SFX.PlayOpenPopup();
            this.uiController.Show<UIPopupFortuneWheel>();
            this.OnClickedEvent?.Invoke(this);
        }

        #endregion
        
        
        [System.Serializable]
        public class Properties : UIProperties {
            public Button button;
        }
    }
}