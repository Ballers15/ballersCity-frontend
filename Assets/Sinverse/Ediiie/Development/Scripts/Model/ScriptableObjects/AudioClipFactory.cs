using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Audio
{
    [CreateAssetMenu(fileName = "ClipData", menuName = "Scriptable Objects/Clip Data")]
    public class AudioClipFactory : ScriptableObject
    {
        [SerializeField] private List<AudioClipInfo> audioClips;

        public void PlayAudio(string name)
        {
            AudioClipInfo clipInfo = audioClips.Find(x => x.name.ToLower() == name.ToLower());
            if (clipInfo == null) return;

            AudioManager.Instance.PlayAudio(clipInfo.clip, clipInfo.isMusic);
        }

        public void PlayAudio(AudioClip clip, bool isMusic)
        {
            AudioManager.Instance.PlayAudio(clip, isMusic);
        }
    }

    [System.Serializable]
    public class AudioClipInfo
    {
        public string name;
        public AudioClip clip;
        public bool isMusic;
    }
}

