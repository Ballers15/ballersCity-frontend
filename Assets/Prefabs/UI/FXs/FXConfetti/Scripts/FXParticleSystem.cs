using UnityEngine;

namespace SinSity.Core {
    public class FXParticleSystem {

        public ParticleSystem particleSystem { get; }
        protected ParticleSystemConfiguration configuration;
        
        public FXParticleSystem(ParticleSystem particleSystem) {
            this.particleSystem = particleSystem;
            this.SaveConfiguration();
        }

        private void SaveConfiguration() {
            this.configuration = new ParticleSystemConfiguration();
            this.configuration.emissionEnabled = this.particleSystem.emission.enabled;
            this.configuration.shapeEnabled = this.particleSystem.shape.enabled;
            this.configuration.velocityOverLifetimeEnabled = this.particleSystem.velocityOverLifetime.enabled;
            this.configuration.limitVelocityOverLifetimeEnabled = this.particleSystem.limitVelocityOverLifetime.enabled;
            this.configuration.inheritVelocityEnabled = this.particleSystem.inheritVelocity.enabled;
            this.configuration.forceOverLifetimeEnabled = this.particleSystem.forceOverLifetime.enabled;
            this.configuration.colorOverLifetimeEnabled = this.particleSystem.colorOverLifetime.enabled;
            this.configuration.colorBySpeedEnabled = this.particleSystem.colorBySpeed.enabled;
            this.configuration.rotationOverLifetimeEnabled = this.particleSystem.rotationOverLifetime.enabled;
            this.configuration.rotationBySpeedEnabled = this.particleSystem.rotationBySpeed.enabled;
            this.configuration.externalForcesEnabled = this.particleSystem.externalForces.enabled;
            this.configuration.noiseEnabled = this.particleSystem.noise.enabled;
            this.configuration.collisionsEnabled = this.particleSystem.collision.enabled;
            this.configuration.triggersEnabled = this.particleSystem.trigger.enabled;
            this.configuration.subEmittersEnabled = this.particleSystem.subEmitters.enabled;
            this.configuration.textureSheetAnimationEnabled = this.particleSystem.textureSheetAnimation.enabled;
            this.configuration.lightsEnabled = this.particleSystem.lights.enabled;
            this.configuration.trailsEnabled = this.particleSystem.trails.enabled;
            this.configuration.customDataEnabled = this.particleSystem.customData.enabled;
            this.configuration.rendererEnabled = this.particleSystem.GetComponent<Renderer>().enabled;
        }

        protected void ApplyConfiguration(ParticleSystemConfiguration newConfiguration) {
            ParticleSystem.EmissionModule emissionModule = this.particleSystem.emission;
            emissionModule.enabled = newConfiguration.emissionEnabled;

            ParticleSystem.ShapeModule shapeModule = this.particleSystem.shape;
            shapeModule.enabled = newConfiguration.shapeEnabled;


            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = this.particleSystem.velocityOverLifetime;
            velocityOverLifetimeModule.enabled = newConfiguration.velocityOverLifetimeEnabled;

            ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule =
                this.particleSystem.limitVelocityOverLifetime;
            limitVelocityOverLifetimeModule.enabled = newConfiguration.limitVelocityOverLifetimeEnabled;

            ParticleSystem.InheritVelocityModule inheritVelocityModule = this.particleSystem.inheritVelocity;
            inheritVelocityModule.enabled = newConfiguration.inheritVelocityEnabled;

            ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = this.particleSystem.forceOverLifetime;
            forceOverLifetimeModule.enabled = newConfiguration.forceOverLifetimeEnabled;

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = this.particleSystem.colorOverLifetime;
            colorOverLifetimeModule.enabled = newConfiguration.colorOverLifetimeEnabled;

            ParticleSystem.ColorBySpeedModule colorBySpeedModule = this.particleSystem.colorBySpeed;
            colorBySpeedModule.enabled = newConfiguration.colorBySpeedEnabled;

            ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule =
                this.particleSystem.rotationOverLifetime;
            rotationOverLifetimeModule.enabled = newConfiguration.rotationOverLifetimeEnabled;

            ParticleSystem.RotationBySpeedModule rotationBySpeedModule = this.particleSystem.rotationBySpeed;
            rotationBySpeedModule.enabled = newConfiguration.rotationBySpeedEnabled;

            ParticleSystem.ExternalForcesModule externalForcesModule = this.particleSystem.externalForces;
            externalForcesModule.enabled = newConfiguration.externalForcesEnabled;

            ParticleSystem.NoiseModule noiseModule = this.particleSystem.noise;
            noiseModule.enabled = newConfiguration.noiseEnabled;

            ParticleSystem.CollisionModule collisionModule = this.particleSystem.collision;
            collisionModule.enabled = newConfiguration.collisionsEnabled;

            ParticleSystem.TriggerModule triggerModule = this.particleSystem.trigger;
            triggerModule.enabled = newConfiguration.triggersEnabled;

            ParticleSystem.SubEmittersModule subEmittersModule = this.particleSystem.subEmitters;
            subEmittersModule.enabled = newConfiguration.subEmittersEnabled;

            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule =
                this.particleSystem.textureSheetAnimation;
            textureSheetAnimationModule.enabled = newConfiguration.textureSheetAnimationEnabled;

            ParticleSystem.LightsModule lightsModule = this.particleSystem.lights;
            lightsModule.enabled = newConfiguration.lightsEnabled;

            ParticleSystem.TrailModule trailModule = this.particleSystem.trails;
            trailModule.enabled = newConfiguration.trailsEnabled;

            ParticleSystem.CustomDataModule customDataModule = this.particleSystem.customData;
            customDataModule.enabled = newConfiguration.customDataEnabled;

            particleSystem.GetComponent<Renderer>().enabled = newConfiguration.rendererEnabled;
        }
        
        public void Play() {
            this.ApplyConfiguration(this.configuration);
            this.particleSystem.Play(false);
        }

        public void Stop() {
            this.particleSystem.Stop(false);
            this.ApplyConfiguration(ParticleSystemConfiguration.Off);
        }
    }
}