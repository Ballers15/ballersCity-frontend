using System;
using SinSity.Domain;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core {
    public class IdleObjectAnimator : MonoBehaviour {
        private static readonly int boolWork = Animator.StringToHash("work");
        private IdleObject idleObject;
        private Animator animator;
        private IOAnimatorSpeedAllCoefficients allCoefficients;
        
        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            animator = gameObject.GetComponent<Animator>();
            idleObject = gameObject.GetComponentInParent<IdleObject>();
            Game.OnGameInitialized += OnGameInitialized;
        }
        
        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;
            
            this.allCoefficients = new IOAnimatorSpeedAllCoefficients();
            this.allCoefficients.OnStateChangedEvent += this.OnAnyCoefficientChanged;

            this.idleObject.OnStateChangedEvent += this.OnStateChanged;

            this.PlayValidAnimation();
        }

        private void OnAnyCoefficientChanged(IOAnimatorSpeedAllCoefficients allcoefficients) {
            var speed = this.allCoefficients.GetTotalValue();
            this.SetAnimationSpeed(speed);
        }

        public void SetAnimationSpeed(float value) {
            this.animator.speed = value;
        }


        private void OnDestroy() {
            if (this.allCoefficients != null)
                this.allCoefficients.OnStateChangedEvent -= this.OnAnyCoefficientChanged;
        }


        #region EVENTS

        private void OnWorkOver() {
            if (!this.idleObject.state.autoPlayEnabled)
                this.DeactivateWork();
        }
        
        private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state) {
            this.idleObject.OnBuiltEvent -= this.OnIdleObjectBuilt;
            this.ActivateWork();
        }
        
        private void OnCurrencyCollected(object sender, BigNumber collectedcurrency) {
            this.ActivateWork();
        }
        
        private void OnStateChanged(IdleObjectState state) {
            if (state.autoPlayEnabled) {
                this.idleObject.OnStateChangedEvent -= this.OnStateChanged;
                this.ActivateWork();
            }
        }

        #endregion
       
        
        private void OnEnable() {
            if (!this.idleObject.isInitialized)
                return;

            this.PlayValidAnimation();
        }

        private void PlayValidAnimation() {
            
            if (!this.idleObject.isBuilt) {
                this.idleObject.OnBuiltEvent += this.OnIdleObjectBuilt;
                return;
            }
            
            this.idleObject.OnCurrencyCollected -= this.OnCurrencyCollected;
            this.idleObject.OnCurrencyCollected += this.OnCurrencyCollected;
            
            if (this.idleObject.state.autoPlayEnabled) {
                this.ActivateWork();
                return;
            }

            this.idleObject.OnStateChangedEvent -= this.OnStateChanged;
            this.idleObject.OnStateChangedEvent += this.OnStateChanged;

            if (Math.Abs(this.idleObject.state.progressNomalized - 1f) < Mathf.Epsilon)
                this.DeactivateWork();
            else
                this.ActivateWork();
        }

        private void OnDisable() {
            if (this.idleObject.isInitialized) {
                this.idleObject.OnCurrencyCollected -= this.OnCurrencyCollected;
                this.idleObject.OnStateChangedEvent -= this.OnStateChanged;
            }
        }

        private void ActivateWork() {
            this.animator.SetBool(boolWork, true);
            
            this.idleObject.OnWorkOver -= this.OnWorkOver;
            this.idleObject.OnWorkOver += this.OnWorkOver;
        }
        
        private void DeactivateWork() {
            this.animator.SetBool(boolWork, false);
        }
    }
}