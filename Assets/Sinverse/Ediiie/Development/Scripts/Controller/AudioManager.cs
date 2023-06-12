using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ediiie.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundAudioSource;
        [SerializeField] private AudioClipFactory commonClips;

        public static AudioManager Instance;
        public static Action<float> OnAudioVolumeChanged;
        public static Action OnStopAudio;

        public static float SFXVolume => Instance.soundAudioSource.volume;

        private float musicSourceDefaultVolume;
        private float sfxSourceDefaultVolume;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }
            else
            {
                Instance = this;
            }

            musicSourceDefaultVolume = Instance.musicAudioSource.volume;
            sfxSourceDefaultVolume = Instance.soundAudioSource.volume;

            //SettingPopup.OnVolumeChanged += SetAudioVolume;
        }

        private void OnDestroy()
        {
            //SettingPopup.OnVolumeChanged -= SetAudioVolume;
        }

        public static void PlayAudio(string clipName)
        {
            Instance.commonClips.PlayAudio(clipName);
        }

        public void SetAudioVolume(bool isMusic, float value)
        {
            AudioSource source;
            float updatedVolume;

            if (!isMusic)
            {
                source = soundAudioSource;
                updatedVolume = sfxSourceDefaultVolume * value;
                OnAudioVolumeChanged?.Invoke(updatedVolume);
            }
            else
            {
                source = musicAudioSource;
                updatedVolume = musicSourceDefaultVolume * value;
            }

            source.volume = updatedVolume;
        }

        public void PlayAudio(AudioClip clip, bool isMusic)
        {
            if (isMusic)
            {
                PlayBGMusic(clip);
            }
            else
            {
                PlaySound(clip);
            }
        }

        private void PlayBGMusic(AudioClip clip)
        {
            musicAudioSource.clip = clip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

        private void PlaySound(AudioClip clip)
        {
            soundAudioSource.clip = clip;
            soundAudioSource.Play();
        }

        public void MuteClipAudio(bool value)
        {
            soundAudioSource.mute = value;
        }

        public void StopAudio()
        {
            soundAudioSource.Stop();
            musicAudioSource.Stop();
        }

    }
}