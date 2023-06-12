using Orego.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public sealed class UIGemFruit : MonoBehaviour, IPointerEnterHandler {
        #region DELEGATES

        public delegate void ClickEventHandler();
        public event ClickEventHandler OnClickEvent;

        #endregion

        [SerializeField] private AudioClip sfxCollect;
        [SerializeField] private UIGemFruitAnimator m_animator;
        [SerializeField] private Image imgLeafs;
        [SerializeField] private Sprite[] spriteLeafs;


        public UIGemFruitAnimator animator => this.m_animator;
        private bool readyForCollect;
        

        public void OnPointerEnter(PointerEventData eventData) {
            if (!this.readyForCollect)
                return;
            
            this.OnClickEvent?.Invoke();
            SFX.PlaySFX(this.sfxCollect);
            this.Collect();
        }

        public void Activate(bool readyForCollect, bool isOpened) {
            this.readyForCollect = readyForCollect;
            if (!isOpened) {
                this.animator.PlayDefaultState();
                return;
            }
            
            if (readyForCollect)
                this.animator.PlayAlreadyBigState();
            else
                this.animator.PlayAlreadySmallState();
            
            this.SetupRandomSpriteLeaf();
        }

        private void SetupRandomSpriteLeaf() {
            var rIndex = Random.Range(0, this.spriteLeafs.Length);
            var rSprite = this.spriteLeafs[rIndex];
            this.imgLeafs.sprite = rSprite;
        }


        public void ActivateFirstTime() {
            this.readyForCollect = true;
            this.animator.PlayGrowBig();
        }
        
        public void ActivateSecondTime() {
            this.readyForCollect = true;
            this.animator.PlayGrowFromSmallToBig();
        }
        
        
        private void Collect() {
            this.readyForCollect = false;
            this.animator.PlayCollect();
        }
    }
}