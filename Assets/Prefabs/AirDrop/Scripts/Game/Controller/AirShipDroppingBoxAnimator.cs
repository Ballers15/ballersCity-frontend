using System;
using System.Collections;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Audio;

namespace SinSity.Core
{
    public sealed class AirShipDroppingBoxAnimator : AutoMonoBehaviour
    {
        #region Event

        public delegate void AnimatorEventHandler();
        public event AnimatorEventHandler OnAnimationFinishedEvent;
        public event AnimatorEventHandler OnExploded;
        

        #endregion

        [SerializeField]
        private Params m_params;

        private Animator animator;

        private SpriteRenderer spriteRenderer;

        private ParticleSystem explosionParticleSystem;

        private readonly Routine explosionRoutine;

        public AirShipDroppingBoxAnimator()
        {
            this.explosionRoutine = new Routine(this);
        }

        private void Awake()
        {
            this.animator = this.m_params.m_animator;
            this.spriteRenderer = this.m_params.m_spriteRenderer;
            this.explosionParticleSystem = this.m_params.m_explosionParticleSystem;
        }

        private void Handle_PlaySFX() {
            SFX.PlaySFX(this.m_params.sfxBox);
        }

        public void OnAnimationFinished()
        {
            this.animator.enabled = false;
            this.spriteRenderer.enabled = false;
            this.explosionRoutine.Start(this.WaitChangeStateRoutine);
            OnExploded.Invoke();
        }

        private IEnumerator WaitChangeStateRoutine()
        {
            this.explosionParticleSystem.gameObject.SetActive(true);
            yield return this.explosionParticleSystem.ShowOneShot();
            this.explosionParticleSystem.gameObject.SetActive(false);
            this.OnAnimationFinishedEvent?.Invoke();
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public Animator m_animator;

            [SerializeField]
            public ParticleSystem m_explosionParticleSystem;

            [SerializeField] 
            public SpriteRenderer m_spriteRenderer;

            public AudioClip sfxBox;
        }
    }
}