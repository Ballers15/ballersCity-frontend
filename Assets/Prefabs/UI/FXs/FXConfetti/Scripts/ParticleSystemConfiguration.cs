 public class ParticleSystemConfiguration {
            public bool emissionEnabled;
            public bool shapeEnabled;
            public bool velocityOverLifetimeEnabled;
            public bool limitVelocityOverLifetimeEnabled;
            public bool inheritVelocityEnabled;
            public bool forceOverLifetimeEnabled;
            public bool colorOverLifetimeEnabled;
            public bool colorBySpeedEnabled;
            public bool rotationOverLifetimeEnabled;
            public bool rotationBySpeedEnabled;
            public bool externalForcesEnabled;
            public bool noiseEnabled;
            public bool collisionsEnabled;
            public bool triggersEnabled;
            public bool subEmittersEnabled;
            public bool textureSheetAnimationEnabled;
            public bool lightsEnabled;
            public bool trailsEnabled;
            public bool customDataEnabled;
            public bool rendererEnabled;

            public static ParticleSystemConfiguration Off => GetOffConfiguration();

            private static ParticleSystemConfiguration GetOffConfiguration() {
                ParticleSystemConfiguration offConfiguration = new ParticleSystemConfiguration();
                offConfiguration.emissionEnabled = false;
                offConfiguration.shapeEnabled = false;
                offConfiguration.velocityOverLifetimeEnabled = false;
                offConfiguration.limitVelocityOverLifetimeEnabled = false;
                offConfiguration.inheritVelocityEnabled = false;
                offConfiguration.forceOverLifetimeEnabled = false;
                offConfiguration.colorOverLifetimeEnabled = false;
                offConfiguration.colorBySpeedEnabled = false;
                offConfiguration.rotationOverLifetimeEnabled = false;
                offConfiguration.rotationBySpeedEnabled = false;
                offConfiguration.externalForcesEnabled = false;
                offConfiguration.noiseEnabled = false;
                offConfiguration.collisionsEnabled = false;
                offConfiguration.triggersEnabled = false;
                offConfiguration.subEmittersEnabled = false;
                offConfiguration.textureSheetAnimationEnabled = false;
                offConfiguration.lightsEnabled = false;
                offConfiguration.trailsEnabled = false;
                offConfiguration.rendererEnabled = false;
                return offConfiguration;
            }
        }