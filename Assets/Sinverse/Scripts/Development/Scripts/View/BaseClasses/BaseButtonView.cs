using Ediiie.Audio;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonView : HoverText
{
    protected Button button;

    protected virtual void Awake()
    {
        button = this.GetComponent<Button>();
       // Debug.Log(button);
        button.onClick.AddListener(OnButtonClicked);
        button.onClick.AddListener(PlayClickSound);
    }

    protected virtual void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
        button.onClick.RemoveListener(PlayClickSound);
    }

    protected abstract void OnButtonClicked();

    private void PlayClickSound()
    {
        AudioManager.PlayAudio(Constants.BUTTON_CLICK_CLIP);
    }

    protected void EnableButton(bool on)
    {
        button.interactable = on;
    }
}
