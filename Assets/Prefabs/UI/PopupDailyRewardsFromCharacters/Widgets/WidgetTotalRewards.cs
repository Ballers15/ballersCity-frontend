using SinSity.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class WidgetTotalRewards : UISceneElement {
        [SerializeField] private Image imageIcon;
        [SerializeField] private Text textFieldAmount;
        
        public void Show() {
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        public void UpdateVisual(Sprite icon, string amount) {
            imageIcon.sprite = icon;
            textFieldAmount.text = amount;
        }
    }
}

