using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VavilichevGD.Audio
{
    [SerializeField]
    public class SoundSettings
    {
        public float volumeSFX;
        
        public float volumeMusic;
        
        public bool isEnabledSFX;
        
        public bool isEnabledMusic;

        private const float VOLUME_MUSIC_MAX = 0.40f;
        private const float VOLUME_SFX_MAX = 1f;

        public static SoundSettings GetDefaultValue()
        {
            SoundSettings defaultSettings = new SoundSettings
            {
                volumeSFX = VOLUME_SFX_MAX, volumeMusic = VOLUME_MUSIC_MAX, isEnabledSFX = true, isEnabledMusic = true
            };
            return defaultSettings;
        }
    }
}