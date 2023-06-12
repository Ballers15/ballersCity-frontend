using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIElementEcoClicker : UIElement {

        protected UIController uiController;

        protected override void Start() {
            base.Start();

            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            uiController = uiInteractor.uiController;
        }

        protected virtual void OnEnable() {
            InputManager.OnBackKeyClicked += OnBackKeyClicked;
        }

        protected virtual void OnBackKeyClicked() {
            this.Hide();
        }

        protected virtual bool CanHideByBackKeyClicked() {
            bool isSettingsActive = uiController.IsActiveUIElement<UIPopupSettings>();
            return !isSettingsActive;
        }

        protected virtual void OnDisable() {
            InputManager.OnBackKeyClicked -= OnBackKeyClicked;
        }
    }
}