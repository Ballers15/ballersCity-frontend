using SinSity.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public class IdleObjectBtnBuild : MonoBehaviour {

        [SerializeField] private Button btnBuild;
        [Space] 
        [SerializeField] private Image imgButton;
        [SerializeField] private Sprite spriteActive;
        [SerializeField] private Sprite spriteInactive;
        [SerializeField] private Animator animator;


        private IdleObject idleObject;
        private static readonly int BOOL_READY_GOR_BUILD = Animator.StringToHash("ready");

        private void OnEnable() {
            BluePrint.OnBluePrintStateChanged += OnBluePrintStateChanged;

            if (Game.isInitialized) {
                this.Setup();
            }
            else {
                void OnGameInitialized(Game game) {
                    Game.OnGameInitialized -= OnGameInitialized;
                    this.Setup();
                }

                Game.OnGameInitialized += OnGameInitialized;
            }
        }

        private void Setup() {
            this.UpdateState();
            Bank.uiBank.OnStateChangedEvent += this.OnBankStateChanged;
        }

        private void OnBluePrintStateChanged(bool isActive) {
            UpdateState();
        }

        public void AddListener(UnityAction callback) {
            if (!idleObject)
                Initialize();
            btnBuild.onClick.AddListener(callback);
            idleObject.OnInitialized += OnInitialized;
        }

        private void OnInitialized() {
            idleObject.OnInitialized -= OnInitialized;
            UpdateState();
        }


        private void Initialize() {
            idleObject = GetComponentInParent<IdleObject>();
        }
        
        public void RemoveListener(UnityAction callback) {
            if (!idleObject)
                Initialize();
            btnBuild.onClick.RemoveListener(callback);
            if (Bank.uiBank == null) return;
            Bank.uiBank.OnStateChangedEvent -= OnBankStateChanged;
        }

        private void OnBankStateChanged(object sender) {
            UpdateState();
        }

        private void UpdateState() {
            if (idleObject == null || !idleObject.isInitialized)
                return;
            
            if (Bank.isEnoughtSoftCurrency(idleObject.info.priceToBuild))
                SetVisualAsReadyForBuild();
            else
                SetVisualAsNOTReadyForBuild();
        }

        private void SetVisualAsReadyForBuild() {
            imgButton.sprite = spriteActive;
            animator.SetBool(BOOL_READY_GOR_BUILD, true);
        }

        private void SetVisualAsNOTReadyForBuild() {
            imgButton.sprite = spriteInactive;
            animator.SetBool(BOOL_READY_GOR_BUILD, false);
        }
        
        private void OnDisable() {
            BluePrint.OnBluePrintStateChanged += OnBluePrintStateChanged;
            if (Bank.uiBank == null) return;
            Bank.uiBank.OnStateChangedEvent -= OnBankStateChanged;
        }
    }
}