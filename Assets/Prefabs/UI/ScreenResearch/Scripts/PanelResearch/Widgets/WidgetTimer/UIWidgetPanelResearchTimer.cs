namespace SinSity.UI {
    public class UIWidgetPanelResearchTimer : UIWidgetPanelResearch<UIWidgetPanelResearchTimerProperties> {

        public void SetTimerValue(string timerValueText) {
            this.properties.SetTimerValueText(timerValueText);
        }
    }
}