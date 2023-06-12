using System;
using System.Collections;
using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Audio {
    [RequireComponent(typeof(AudioSource))]
    public abstract class MusicPlayerBase : MonoBehaviour {
        
        #region Static variables and properties
        
        public static bool isPlaying => isInitialized && instance.sourceMusic.isPlaying;

        protected static MusicPlayerBase instance { get; private set; }
        protected static bool isInitialized => instance != null;

        protected static CoroutineDispatcher coroutineDispatcher;

        public delegate void TrackStartedHandler(AudioClip track);
        public static event TrackStartedHandler OnTrackStarted;

        public delegate void TrackOverHandler(AudioClip track);
        public static event TrackOverHandler OnTrackOver;

        #endregion
       
        [SerializeField] protected bool ignorePause = true;
        
        protected AudioSource sourceMusic;

        public virtual void Initialize(SoundSettings settings) {
            CreateSingleton();
            
            sourceMusic = gameObject.GetComponent<AudioSource>();
            coroutineDispatcher = new CoroutineDispatcher(this);
            
            sourceMusic.ignoreListenerPause = ignorePause;
            ApplySettings(settings);
        }
        
        protected void CreateSingleton() {
            if (!isInitialized)
                instance = this;
        }

        public virtual void SetVolumeMusic(float value) {
            sourceMusic.volume = value;
        }

        public virtual void ApplySettings(SoundSettings settings) {
            float volumeMusic = settings.isEnabledMusic ? settings.volumeMusic : 0f;
            SetVolumeMusic(volumeMusic);
        }

        public virtual void Pause() {
            if (!ignorePause)
                sourceMusic.Pause();
        }

        public virtual void Unpause() {
            if (!ignorePause)
                sourceMusic.UnPause();
        }

        public static void Play(AudioClip track) {
            instance.Play(instance.sourceMusic, track);
        }

        protected virtual void Play(AudioSource source, AudioClip track) {
            source.clip = track;
            source.loop = false;
            source.Play();
            
            coroutineDispatcher.StartForce(WaitForTrackOver());
            OnTrackStarted?.Invoke(track);
        }

        public static void Stop() {
            NotifyAboutMusicStopped(instance.sourceMusic.clip);
            instance.sourceMusic.Stop();
            coroutineDispatcher.Stop();
        }

        protected static void NotifyAboutMusicStopped(AudioClip track) {
            OnTrackOver?.Invoke(track);
        }
        
        protected virtual IEnumerator WaitForTrackOver() {
            AudioSource source = instance.sourceMusic;
            WaitForSecondsRealtime frame = new WaitForSecondsRealtime(0.01f);
            
            while (source.isPlaying)
                yield return frame;
            
            NotifyAboutMusicStopped(source.clip);
        }
    }
}