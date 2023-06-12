using System;

namespace VavilichevGD.UI
{
    [Serializable]
    public abstract class UIDialogueArgs
    {
        public UIElement uiElement;

        public T GetUIElement<T>() where T : UIElement
        {
            return (T) this.uiElement;
        }
        
        protected UIDialogueArgs(UIElement uiElement)
        {
            this.uiElement = uiElement;
        }
    }
}