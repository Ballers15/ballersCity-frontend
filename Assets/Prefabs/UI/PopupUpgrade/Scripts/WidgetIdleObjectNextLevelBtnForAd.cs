using System;
using System.Collections;
using SinSity.Core;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace SinSity.UI {
    public class WidgetIdleObjectNextLevelBtnForAd : AnimObject {
        #region EVENTS

        public event Action OnUpgradeForAdEndedEvent;
        public event Action OnADStartPlaying;

        #endregion

        [SerializeField] private Button button;
        [SerializeField] private ProgressBarMasked progressBar;
        [SerializeField] private float delayBeforeStarting = 0.5f;
        [SerializeField] private float delay = 0.2f;
        private IdleObject _idleObject;
        private IdleObjectsUpgradeForAdInteractor upgradeForAdInteractor;
        private UIPopupUpgradeForAD popup;
        private UIInteractor uiInteractor;
        private int levelsToUpgrade;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            this.upgradeForAdInteractor = Game.GetInteractor<IdleObjectsUpgradeForAdInteractor>();
            this.uiInteractor = Game.GetInteractor<UIInteractor>();
            this.popup = this.uiInteractor.GetUIElement<UIPopupUpgradeForAD>();
        }
        
        public void Setup(IdleObject idle) {
            _idleObject = idle;
        }

        private void OnEnable() {
            if (_idleObject == null) return;
            
            this.button.onClick.AddListener(this.OnButtonClick);
            this.levelsToUpgrade = this.upgradeForAdInteractor.GetUpgradeLevelsCount(this._idleObject);
        }

        private void OnDisable() {
            if (_idleObject == null) return;
            
            this.button.onClick.RemoveListener(this.OnButtonClick);
            StopCoroutine($"UpgradeRoutine");
        }

        private void OnButtonClick() {
            this.popup.Setup(this._idleObject, this.levelsToUpgrade);
            this.popup.Show();
            this.popup.OnGetButtonClickedEvent += this.OnPopupGetButtonClicked;
            this.popup.OnCloseClickEvent += OnPopupClosed;
        }

        private void OnPopupClosed() {
            this.popup.OnCloseClickEvent -= OnPopupClosed;
            this.popup.OnGetButtonClickedEvent -= this.OnPopupGetButtonClicked;
        }

        private void OnPopupGetButtonClicked() {
            this.popup.OnGetButtonClickedEvent -= this.OnPopupGetButtonClicked;
            this.popup.Hide();
            this.ShowAD();
        }

        private void ShowAD() {
            var popupAdLoading = this.uiInteractor.ShowElement<UIPopupADLoading>();
            popupAdLoading.OnADResultsReceived += this.OnAdResultsReceived;
            popupAdLoading.ShowAD("io_upgrade_lvl");
            this.button.onClick.RemoveListener(this.OnButtonClick);
            this.OnADStartPlaying?.Invoke();
        }

        private void OnAdResultsReceived(UIPopupADLoading popup, bool success, string error) {
            if (!success) return;
            StartCoroutine(this.UpgradeRoutine(levelsToUpgrade));
        }

        private IEnumerator UpgradeRoutine(int times) {
            var curTimes = 0;
            var periodDelay = this.delay;
            var periodDelayMin = 0.03f;
            var periodDelayStep = 0.02f;

            yield return new WaitForSeconds(this.delayBeforeStarting);

            while (curTimes < times) {
                this._idleObject.ForceNextLevel();
                curTimes++;
                yield return new WaitForSeconds(periodDelay);
                periodDelay = Mathf.Max(periodDelay - periodDelayStep, periodDelayMin);
            }

            this.OnUpgradeForAdEndedEvent?.Invoke();
        }
    }
}