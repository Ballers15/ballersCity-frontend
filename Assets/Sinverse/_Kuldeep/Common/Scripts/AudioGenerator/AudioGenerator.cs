using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGenerator : MonoBehaviour
{
    public static AudioGenerator Instance;

    public List<AudioSource> sources;
    private void Start()
    {
        Instance = this;
    }

    public static void GenerateAudio(AudioClip clip)
    {
        if (Instance.sources.Exists(x => !x.isPlaying))
        {
            var s = Instance.GetFreeSource();
            s.clip = clip;
            s.Play();
        }
        else
        {
            var Go = Instantiate(new GameObject(), Instance.transform);
            var s = Go.AddComponent<AudioSource>();
            Instance.sources.Add(s);
            s.clip = clip;
            s.Play();
        }

    }

    private AudioSource GetFreeSource()
    {
        var source = sources.Find(x => !x.isPlaying);
         return source;
    }
}