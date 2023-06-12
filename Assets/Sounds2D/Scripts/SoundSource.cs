using UnityEngine;

namespace VavilichevGD.Audio {
    public enum SourceType {
        SFX,
        Music
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour {

        [SerializeField] private SourceType type;
        [SerializeField] private bool ignorePause;

        private AudioSource source;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            source = gameObject.GetComponent<AudioSource>();
        }

        private void Start() {
            Sounds.OnSoundSettingsChanged += SoundsOnOnSoundSettingsChanged;
            Sounds.OnSoundsPaused += SoundsOnOnSoundsPaused;
        }

        private void SoundsOnOnSoundSettingsChanged(SoundSettings settings) {
            switch (type) {
                case SourceType.Music:
                    source.volume = settings.isEnabledMusic ? settings.volumeMusic : 0f;
                    break;
                case SourceType.SFX:
                    source.volume = settings.isEnabledSFX ? settings.volumeSFX : 0f;
                    break;
            }
        }
        
        private void SoundsOnOnSoundsPaused(bool pause) {
            if (!ignorePause) {
                if (pause)
                    source.Pause();
                else
                    source.UnPause();
            }
        }

        private void OnDestroy() {
            Sounds.OnSoundSettingsChanged -= SoundsOnOnSoundSettingsChanged;
            Sounds.OnSoundsPaused -= SoundsOnOnSoundsPaused;
        }
    }
}