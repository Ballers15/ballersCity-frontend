using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public sealed class UIPanelAudioSettingSFX : UIPanelAudioSetting
    {
        [SerializeField] private AudioClip audioClipClick;
        [SerializeField] private Text textField;

        protected override void OnBtnClick()
        {
            SFX.PlaySFX(this.audioClipClick);
            Sounds.SwitchStateSFX();
            UpdateState();
        }

        protected override void UpdateState() {
            if (Sounds.isEnabledSFX)
                textField.text = "Sound On";
            else
                textField.text = "Sound Off";
        }
    }
}