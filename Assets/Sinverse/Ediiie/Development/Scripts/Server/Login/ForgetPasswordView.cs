using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ediiie.Screens;
using Screen = Ediiie.Screens.Screen;

public class ForgetPasswordView : GameScreenBackForth
{
    [SerializeField] private TMP_InputField txtEmail;
    [SerializeField] private Button authenticateButton;

    private void Awake()
    {
        authenticateButton.onClick.AddListener(CallVerificationAPI);
    }

    private void OnEnable()
    {
        txtEmail.text = "";
    }

    private void OnDestroy()
    {
        authenticateButton.onClick.RemoveListener(CallVerificationAPI);
    }

    public void CallVerificationAPI()
    {
        ForgetPasswordRequest forgetPasswordRequest = new ForgetPasswordRequest()
        {
            email = txtEmail.text
        };

        ServerController.SendToServer(forgetPasswordRequest, OnVerificationCompleted);
    }

    private void OnVerificationCompleted()
    {
        ScreenManager.ShowScreen(Screen.RESET_PASSWORD_PANEL);
    }
}
