using SinSity.Domain;
using IdleClicker.Gameplay;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using Random = UnityEngine.Random;

namespace SinSity.UI {
    public class UIControllerCoffeeBoostFX : MonoBehaviour {
        
        [SerializeField] private Animator[] fxAnimators;
        [SerializeField] private AudioClip sfxPlay;

        private bool isActive;
        private CoffeeBoost coffeeBoost;

        private void Awake() {
            this.DeactivateLightning();
        }

        private void Start() {
            this.Initialize();
        }

        private void Initialize() {
            var coffeeBoostInteractor = Game.GetInteractor<CoffeeBoostInteractor>();
            this.coffeeBoost = coffeeBoostInteractor.coffeeBoost;
            this.coffeeBoost.OnStartedEvent += boost => { this.ActivateLightning(); };
            this.coffeeBoost.OnEndedEvent += boost => { this.DeactivateLightning(); };
            
            BluePrint.OnBluePrintStateChanged += this.OnBluePrintStateChanged;
        }

        private void OnDestroy() {
            BluePrint.OnBluePrintStateChanged -= this.OnBluePrintStateChanged;
        }

        private void OnBluePrintStateChanged(bool bluePrinsIsActive) {
            if (bluePrinsIsActive) {
                if (this.isActive)
                    this.DeactivateLightning(true);
            }
            else {
                if (this.isActive)
                    this.ActivateLightning();
            }
        }

        private void ActivateLightning() {
            foreach (var fxAnimator in this.fxAnimators) {
                fxAnimator.gameObject.SetActive(true);
                var rTimePosition = Random.Range(0f, 1f);
                fxAnimator.playbackTime = rTimePosition;
            }

            if (!this.isActive) {
                this.PlaySFX();
                this.isActive = true;
            }
        }

        private void DeactivateLightning(bool stayActive = false) {
            foreach (var fxAnimator in this.fxAnimators) {
                fxAnimator.gameObject.SetActive(false);
            }

            this.isActive = stayActive;
        }

        private void PlaySFX() {
            SFX.PlaySFX(sfxPlay);
        }

    }
}