namespace VavilichevGD.UI {
    public enum UIPopupResult {
        Close,
        Apply,
        Error,
        Other
    }
    public class UIPopupArgs : UIDialogueArgs {
        public UIPopupResult result;
        public UIPopupArgs(UIElement uiElement) : base(uiElement) { }

        public UIPopupArgs(UIElement uiElement, UIPopupResult result) : base(uiElement) {
            this.result = result;
        }
    }
}