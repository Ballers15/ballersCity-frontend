using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Ediiie.Audio;

public abstract class PlaySound : MonoBehaviour
    {
        protected void OnButtonClick()
        {
            AudioManager.PlayAudio(Constants.BUTTON_CLICK_CLIP);
        }
    }

