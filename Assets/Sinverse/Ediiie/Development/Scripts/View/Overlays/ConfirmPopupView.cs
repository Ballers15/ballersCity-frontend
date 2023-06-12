using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ediiie.Screens
{
    public class ConfirmPopupView : GameUI
    {
        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private UnityEvent OnOKClicked; //event to be called is set from inspector

        private void Awake()
        {
            okButton.onClick.AddListener(OKButtonClicked);
            cancelButton.onClick.AddListener(CancelButtonClicked);
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OKButtonClicked);
            cancelButton.onClick.RemoveListener(CancelButtonClicked);
        }

        private void OKButtonClicked()
        {
            OnOKClicked?.Invoke();
            Close();
        }

        private void CancelButtonClicked()
        {
            Close();
        }

        private void Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}