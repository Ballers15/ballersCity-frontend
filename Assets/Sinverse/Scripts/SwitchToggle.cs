using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sinverse
{
    public class SwitchToggle : MonoBehaviour
    {
        public static Action<bool> OnAtiveAudio;
        [SerializeField] private Image inactiveImage;
        [SerializeField] private Image activeImage;
        [SerializeField] private Toggle toggle;

        private void Awake()
        {
            inactiveImage.enabled = false;
            toggle.onValueChanged.AddListener(SetAudio);
        }

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(Constants.PP_AUDIO_TOGGLE))
            {
                if (PlayerPrefs.GetInt(Constants.PP_AUDIO_TOGGLE) == 1)
                {
                    SetAudio(true);
                    toggle.isOn = true;
                }
                else
                {
                    SetAudio(false);
                    toggle.isOn = false;
                }
            }
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(SetAudio);
        }

        private void SetAudio(bool status)
        {
            if (status)
            {
                inactiveImage.enabled = false;
                activeImage.enabled = true;
                OnAtiveAudio?.Invoke(true);
                PlayerPrefs.SetInt(Constants.PP_AUDIO_TOGGLE, 1);
            }
            else
            {
                inactiveImage.enabled = true;
                activeImage.enabled = false;
                OnAtiveAudio?.Invoke(false);
                PlayerPrefs.SetInt(Constants.PP_AUDIO_TOGGLE, 0);
            }
        }

    }
}
