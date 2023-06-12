 using UnityEngine.UI;

 namespace SinSity.UI {
     [System.Serializable]
    public class UIWidgetShopButtonProperties : UIProperties {
        public Button btn;
        public Text textPrice;
        public bool canSetPrice = true;
    }
}