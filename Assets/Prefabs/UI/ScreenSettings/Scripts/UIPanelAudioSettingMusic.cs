using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public sealed class UIPanelAudioSettingMusic : UIPanelAudioSetting
    {
        [SerializeField] private AudioClip audioClipClick;
        [SerializeField] private Text textField;
        
        protected override void OnBtnClick()
        {
            SFX.PlaySFX(this.audioClipClick);
            Sounds.SwitchStateMusic();
            UpdateState();
        }

        protected override void UpdateState()
        {
            if (Sounds.isEnabledMusic)
                textField.text = "Music On";
            else
                textField.text = "Music Off";
        }
    }
}