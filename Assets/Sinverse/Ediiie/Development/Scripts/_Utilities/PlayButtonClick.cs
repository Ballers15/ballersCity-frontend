using CogniHab.Audio;
using UnityEngine.UI;

namespace CogniHab.Audio
{
    public class PlayButtonClick : PlaySound
    {
        Button myButton;

        // Start is called before the first frame update
        void Awake()
        {
            myButton = this.GetComponent<Button>();
            myButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            myButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}
