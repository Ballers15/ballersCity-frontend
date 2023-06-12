using UnityEngine;
using VavilichevGD.Extensions;

namespace VavilichevGD.Audio {
    [System.Serializable]
    public class MusicPlayList {
        [SerializeField] private AudioClip[] tracks;

        private AudioClip lastTrack;

        public AudioClip GetRandom() {
            AudioClip nextTrack = tracks.GetRandom(lastTrack);
            lastTrack = nextTrack;
            return nextTrack;
        }
    }
}