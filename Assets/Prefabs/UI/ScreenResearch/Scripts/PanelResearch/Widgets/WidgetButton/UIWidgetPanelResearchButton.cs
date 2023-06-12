namespace SinSity.UI
{
    public class UIWidgetPanelResearchButton : UIWidgetPanelResearch<UIWidgetPanelResearchButtonProperties>
    {
        public virtual bool AllowToUse() {
            return true;
        }

        public void PlaySFX_Error() {
            this.properties.PlayError();
        }
    }
}