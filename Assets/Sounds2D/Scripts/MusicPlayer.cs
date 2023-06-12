using System.Collections;
using UnityEngine;

namespace VavilichevGD.Audio {
    public class MusicPlayer : MusicPlayerBase {

        [SerializeField] protected bool playOnAwake = true;
        [SerializeField] protected bool autoPlay = true;
        [Space] 
        [SerializeField] protected MusicPlayList playList;


        protected void Start() {
            if (playOnAwake)
                PlayNextTrack();
        }

        protected void PlayNextTrack() {
            AudioClip nextTrack = playList.GetRandom();
            Stop();
            Play(sourceMusic, nextTrack);
        }

        protected override IEnumerator WaitForTrackOver() {
            WaitForSecondsRealtime frame = new WaitForSecondsRealtime(0.01f);
            
            while (sourceMusic.isPlaying)
                yield return frame;
            
            NotifyAboutMusicStopped(sourceMusic.clip);

            if (autoPlay)
                PlayNextTrack();
        }
    }
}