using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthenticationView : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtActivationCode;
    [SerializeField] private TMP_InputField txtPinCode;
    [SerializeField] private SceneProperties sceneProperties;

    public void OnButtonClicked()
    {
        ActivationRequest activationRequest = new ActivationRequest()
        {
            activationCode = txtActivationCode.text,
            pinCode = txtPinCode.text
        };

        ServerController.SendToServer(activationRequest, OnActivationCompleted);
    }

    private void OnActivationCompleted()
    {
        SceneController.LoadScene(sceneProperties);
    }
}
