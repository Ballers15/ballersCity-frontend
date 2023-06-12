using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Ediiie.Screens;

public class TwoFactorAuthenticationView : GameScreen
{
    [SerializeField] private TMP_InputField txtActivationCode;
    [SerializeField] private SceneProperties sceneProperties;
    [SerializeField] private Button authenticateButton;

    private void Awake()
    {
        authenticateButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        authenticateButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void OnButtonClicked()
    {
        TwoFactorRequest activationRequest = new TwoFactorRequest()
        {
            twoFactorCode = txtActivationCode.text
        };

        ServerController.SendToServer(activationRequest, OnActivationCompleted);
    }

    private void OnActivationCompleted()
    {
        SceneController.LoadScene(sceneProperties);
    }
}
