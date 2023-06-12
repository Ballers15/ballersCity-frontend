using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIPopupFortuneWheelRewardCardVisual : MonoBehaviour {

        [SerializeField] private Image imgBackground;
        [SerializeField] private Sprite spriteBackBlue;
        [SerializeField] private Sprite spriteBackGold;
        [SerializeField] private Image imgFlare;
        [Header("Glow")] 
        public Image imgGlow;
        public Color colorGlowWhite = Color.white;
        public Color colorGlowGold = Color.yellow;


        public void SetupAsSimple() {
            this.imgBackground.sprite = spriteBackBlue;
            this.imgFlare.gameObject.SetActive(false);
            this.SetGlowColor(this.colorGlowWhite);
        }

        public void SetupAsJackpot() {
            this.imgBackground.sprite = spriteBackGold;
            this.imgFlare.gameObject.SetActive(true);
            this.SetGlowColor(this.colorGlowGold);
        }
        
        private void SetGlowColor(Color color) {
            this.imgGlow.color = color;
        }
    }
}