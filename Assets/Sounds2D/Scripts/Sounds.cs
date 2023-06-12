using UnityEngine;
using UnityEngine.Serialization;
using VavilichevGD.Tools;

namespace VavilichevGD.Audio {
    public class Sounds : MonoBehaviour {
        
        #region Static variables and properties
        
        public static float volumeSFX => settings?.volumeSFX ?? 0f;
        public static float volumeMusic => settings?.volumeMusic ?? 0f;
        public static bool isEnabledMusic => settings?.isEnabledMusic ?? true;
        public static bool isEnabledSFX => settings?.isEnabledSFX ?? true;
        public static bool isSoundsEnabled => settings.isEnabledMusic || settings.isEnabledSFX;
        
        public delegate void SoundSettingsHandler(SoundSettings settings);
        public static event SoundSettingsHandler OnSoundSettingsChanged;

        public delegate void SoundsPausedHandler(bool pause);
        public static event SoundsPausedHandler OnSoundsPaused;
        
        #endregion
       
        [SerializeField] private SFX m_sfx;
        [SerializeField] private MusicPlayerBase m_musicPlayer;

        private const string PREF_KEY = "SOUND_SETTINGS";

        private static SoundSettings settings;
        private static SFX sfx;
        private static MusicPlayerBase musicPlayer;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            sfx = m_sfx;
            musicPlayer = m_musicPlayer;
            
            SoundSettings settingsDefault = SoundSettings.GetDefaultValue();
            settings = Storage.GetCustom(PREF_KEY, settingsDefault);
            sfx.Initialize(settings);
            musicPlayer.Initialize(settings);
        }


        public static void SetActiveSFX(bool isEnabled) {
            settings.isEnabledSFX = isEnabled;
            ApplySettings();
            NotifyAboutSettingsChanged();
        }

        public static void SetActiveMusic(bool isEnabled) {
            settings.isEnabledMusic = isEnabled;
            ApplySettings();
            NotifyAboutSettingsChanged();
        }

        public static void SetVolumeSFX(float newValue) {
            float clampedValue = Mathf.Clamp01(newValue);
            settings.volumeSFX = clampedValue;
            settings.isEnabledSFX = clampedValue > 0f;
            ApplySettings();
            NotifyAboutSettingsChanged();
        }

        public static void SetVolumeMusic(float newValue) {
            float clampedValue = Mathf.Clamp01(newValue);
            settings.volumeMusic = clampedValue;
            settings.isEnabledMusic = clampedValue > 0f;
            ApplySettings();
            NotifyAboutSettingsChanged();
        }

        public static void IncreaseVolumeSFX(float step = 0.1f) {
            float newValue = settings.volumeSFX + step;
            SetVolumeSFX(newValue);
        }

        public static void DecreaseVolumeSFX(float step = 0.1f) {
            float newValue = settings.volumeSFX - step;
            SetVolumeSFX(newValue);
        }

        public static void IncreaseVolumeMusic(float step = 0.1f) {
            float newValue = settings.volumeMusic + step;
            SetVolumeMusic(newValue);
        }

        public static void DecreaseVolumeMusic(float step = 0.1f) {
            float newValue = settings.volumeMusic - step;
            SetVolumeMusic(newValue);
        }

        public static void SetActiveSounds(bool isEnabled) {
            SetActiveSFX(isEnabled);
            SetActiveMusic(isEnabled);
        }

        public static void SwitchSounds() {
            SetActiveSounds(!isSoundsEnabled);
        }

        public static void SwitchStateSFX() {
            SetActiveSFX(!isEnabledSFX);
        }

        public static void SwitchStateMusic() {
            SetActiveMusic(!isEnabledMusic);
        }

        public static void Pause() {
            sfx.Pause();
            musicPlayer.Pause();
            NotifyAboutPaused(true);
        }

        public static void Unpause() {
            sfx.Unpause();
            musicPlayer.Unpause();
            NotifyAboutPaused(false);
        }

        private static void NotifyAboutPaused(bool pause) {
            OnSoundsPaused?.Invoke(pause);
        }

        private static void ApplySettings() {
            sfx.ApplySettings(settings);
            musicPlayer.ApplySettings(settings);
            SaveSettings();
        }
        
        private static void NotifyAboutSettingsChanged() {
            OnSoundSettingsChanged?.Invoke(settings);
        }

        private static void SaveSettings() {
            Storage.SetCustom<SoundSettings>(PREF_KEY, settings);
        }
    }
}