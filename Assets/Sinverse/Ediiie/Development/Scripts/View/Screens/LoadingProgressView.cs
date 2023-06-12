using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Sinverse
{
    public class LoadingProgressView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider slider;

        private void OnEnable()
        {
            loadingText.text = "Loading...";
            slider.value = 0f;
        }

        public void SetLoading(float progress, string loadingMsg)
        {        
            int progressInt = Mathf.RoundToInt(progress * 100f);        
            loadingText.text = loadingMsg + "..." + (progress*100).ToString("F0") + "%";
            slider.DOValue(progress, 0.5f);
        }         
    }
}