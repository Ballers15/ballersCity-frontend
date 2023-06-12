using UnityEngine;

namespace Sinverse
{
    public class AudioController : MonoBehaviour
    {
        private void Start()
        {
            SwitchToggle.OnAtiveAudio += OnEnableAudio;
        }

        private void OnDestroy()
        {
            SwitchToggle.OnAtiveAudio -= OnEnableAudio;
        }

        private void OnEnableAudio(bool status)
        {
            if (status)
            {
                AudioListener.volume = 0.5F;
            }
            else
            {
                AudioListener.volume = 0.0F;
            }
        }
    }
}
