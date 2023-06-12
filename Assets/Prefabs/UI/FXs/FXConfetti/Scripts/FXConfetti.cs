using System;
using System.Collections;
using SinSity.Core;
using UnityEngine;

namespace SinSity.UI {
    public class FXConfetti : MonoBehaviour {
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private bool deactivateOnStart = true;

        private FXParticleSystem[] fxParticleSystems;

        private void Start() {
            this.fxParticleSystems = new FXParticleSystem[this.particleSystems.Length];
            int count = fxParticleSystems.Length;
            for (int i = 0; i < count; i++)
                this.fxParticleSystems[i] = new FXParticleSystem(this.particleSystems[i]);

            if (deactivateOnStart) {
                foreach (var particleSystem in this.fxParticleSystems)
                    particleSystem.Stop();
            }
        }

        public void Play() {
            foreach (var fxParticleSystem in fxParticleSystems)
                fxParticleSystem.Play();
            this.StartCoroutine("LifeRoutine");
        }

        private IEnumerator LifeRoutine() {
            yield return new WaitForSeconds(lifeTime);
            this.Stop();
        }

        public void Stop() {
            if (fxParticleSystems == null)
                return;
            
            foreach (var fxParticleSystem in fxParticleSystems)
                fxParticleSystem.Stop();
        }

        private void OnDisable() {
            this.StopCoroutine("LifeRoutine");
            this.Stop();
        }

        private void Reset() {
            if (this.particleSystems.Length == 0)
                this.particleSystems = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        }
    }
}