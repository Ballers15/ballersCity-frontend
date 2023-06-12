using VavilichevGD.UI;

namespace SinSity.UI {
    public class UIWidgetExclamationMark : UIWidget<UIWidgetExclamationMark.Properties> {

        public void Activate() {
            this.SetActive(true);
        }

        public void Deactivate() {
            this.SetActive(false);
        }

        public void SetActive(bool isActive) {
            this.gameObject.SetActive(isActive);
        }
        
        
        [System.Serializable]
        public class Properties : UIProperties {
            
        }
    }
}