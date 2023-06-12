using UnityEngine;

namespace VavilichevGD.Audio {
    public class SFX : MonoBehaviour {
        [SerializeField] protected AudioSource sourceSFX;
        [SerializeField] protected AudioSource sourceSFXRandomPitch;
        [SerializeField] protected AudioSource sourceSFXUI;

        [Header("Common SFX")] 
        [SerializeField] protected AudioClip sfxBtnClick;
        [SerializeField] protected AudioClip sfxOpenPopup;
        [SerializeField] protected AudioClip sfxClosePopup;

        private const float PITCH_MIN = 0.8f;
        private const float PITCH_MAX = 1.5f;

        private static SFX instance;
        private static bool isInitialized => instance != null;

        private void Awake() {
            CreateSingleton();
        }

        private void CreateSingleton() {
            if (!isInitialized)
                instance = this;
        }
        
        public virtual void Initialize(SoundSettings settings) {
            sourceSFXUI.ignoreListenerPause = true;
            ApplySettings(settings);
        }

        public static void PlaySFX(AudioClip sfx) {
            instance.sourceSFX.PlayOneShot(sfx);
        }
        
        public static void PlaySFXRandomPitch(AudioClip sfx) {
            float rPitch = Random.Range(PITCH_MIN, PITCH_MAX);
            instance.sourceSFXRandomPitch.pitch = rPitch;
            instance.sourceSFXRandomPitch.PlayOneShot(sfx);
        }

        public static void PlaySFXUI(AudioClip sfx) {
            instance.sourceSFXUI.PlayOneShot(sfx);
        }
        

        public void SetVolumeSFX(float value) {
            sourceSFX.volume = value;
            sourceSFXUI.volume = value;
            sourceSFXRandomPitch.volume = value;
        }

        public void ApplySettings(SoundSettings settings) {
            float volumeSFX = settings.isEnabledSFX ? settings.volumeSFX : 0f;
            SetVolumeSFX(volumeSFX);
        }

        public void Pause() {
            sourceSFX.Pause();
            sourceSFXRandomPitch.Pause();
        }

        public void Unpause() {
            sourceSFX.UnPause();
            sourceSFXRandomPitch.UnPause();
        }

        public static void PlayBtnClick() {
            PlaySFX(instance.sfxBtnClick);
        }

        public static void PlayOpenPopup() {
            PlaySFX(instance.sfxOpenPopup);
        }

        public static void PlayClosePopup() {
            PlaySFX(instance.sfxClosePopup);
        }
    }
}