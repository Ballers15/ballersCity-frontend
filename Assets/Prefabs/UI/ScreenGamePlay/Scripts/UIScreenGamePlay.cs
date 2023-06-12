using VavilichevGD.UI;

namespace SinSity.UI
{
    public class UIScreenGamePlay : UIScreen<UIScreenGameplayProperties>
    {
        
        private UIWidgetBtnNavigate[] btnNavigates;

        public override void Initialize()
        {
            base.Initialize();
            this.properties.btnSettings.Initialize();
            this.btnNavigates = this.gameObject.GetComponentsInChildren<UIWidgetBtnNavigate>(true);
        }

        public void HideAllBtnNavigatesExcept(UIWidgetBtnNavigate btnNavigateException)
        {
            foreach (UIWidgetBtnNavigate btnNavigate in this.btnNavigates)
            {
                if (btnNavigate != btnNavigateException)
                {
                    btnNavigate.ClosePopup();
                }
            }
        }
    }
}