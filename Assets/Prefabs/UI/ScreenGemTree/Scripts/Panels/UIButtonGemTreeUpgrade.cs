using System.Collections;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public sealed class UIButtonGemTreeUpgrade : UIWidgetPrice, IPointerDownHandler, IPointerUpHandler
    {
        private GemTreeStateInteractor gemTreeStateInteractor;

        [SerializeField] private UIScreenGemTree myParent;
        [SerializeField] private GameObject myGameObject;
        [SerializeField] private AudioClip sfxError;

        private bool isActive = true;
        private bool pointerInside;
        private UIButtonClickAnimation clickAnimation;

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.gemTreeStateInteractor = this.GetInteractor<GemTreeStateInteractor>();
        }

        public void OnShow()
        {
            this.myParent.OnAnimationStartEvent.AddListener(this.OnAnimationStart);
            this.myParent.OnAnimationFinishedEvent.AddListener(this.OnAnimationFinished);
            this.UpdateState();
        }

        public void OnHide()
        {
            this.myParent.OnAnimationStartEvent.RemoveListener(this.OnAnimationStart);
            this.myParent.OnAnimationFinishedEvent.RemoveListener(this.OnAnimationFinished);
        }

        #region ClickEvents

        public void OnUpgradeClick() {
            if (!this.gemTreeStateInteractor.CanUpgradeTree()) {
                SFX.PlaySFX(this.sfxError);
                return;
            }

            this.gemTreeStateInteractor.UpgradeTree(this);
        }

        #endregion

        private void UpdateState()
        {
            var isLastLevel = this.gemTreeStateInteractor.IsLastLevel();
            this.myGameObject.SetActive(!isLastLevel);
            if (!isLastLevel)
                this.UpdatePrice();
        }

        private void UpdatePrice()
        {
            var upgradePrice = this.gemTreeStateInteractor.GetCurrentUpgradePrice();
            this.SetPriceSoft(upgradePrice);
        }

        #region Animations

        private void OnAnimationStart() {
            this.SetActiveButton(false);
        }

        public void AnimateLevelUp(int newLevel) {
            this.StopCoroutine("LifeRoutine");
            this.UpdateState();
        }

        private void OnAnimationFinished() {
            this.SetActiveButton(true);
        }

        public void AnimateProgressUp(int nextProgress) {
            this.UpdatePrice();
        }

        private void SetActiveButton(bool isAvtive) {
            if (this.clickAnimation == null)
                return;

            this.isActive = isAvtive;
            this.clickAnimation.isActive = isAvtive;
        }
        
        private void OnEnable() {
            this.SetActiveButton(true);
        }

        #endregion
        
        
        public void OnPointerDown(PointerEventData eventData) {
            if (!isActive)
                return;
            
            pointerInside = true;
            this.StartCoroutine("LifeRoutine");
        }
		
        public void OnPointerUp(PointerEventData eventData) {
            this.StopCoroutine("LifeRoutine");
            pointerInside = false;
        }
		
        private IEnumerator LifeRoutine() {
            float periodDelay = 0.2f;
            float periodDelayMin = 0.03f;
            float periodDelayStep = 0.02f;
            while(pointerInside) {
                this.OnUpgradeClick();
                yield return new WaitForSeconds(periodDelay);
                periodDelay = Mathf.Max(periodDelay - periodDelayStep, periodDelayMin);
            }
        }
    }
}