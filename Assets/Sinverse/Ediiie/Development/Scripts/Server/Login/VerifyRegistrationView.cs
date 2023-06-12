using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ediiie.Screens;
using Screen = Ediiie.Screens.Screen;

public class VerifyRegistrationView : GameScreenBackForth
{
    [SerializeField] private TMP_InputField txtEmail;
    [SerializeField] private TMP_InputField txtVerificationCode;
    [SerializeField] private Button authenticateButton;


    private void OnEnable()
    {
        txtEmail.interactable = false;
        txtEmail.text = PlayerPrefs.GetString("Temp_Email");
    }

    private void Awake()
    {
        authenticateButton.onClick.AddListener(CallVerificationAPI);
    }

    private void OnDestroy()
    {
        authenticateButton.onClick.RemoveListener(CallVerificationAPI);
    }

    public void CallVerificationAPI()
    {
        VerifyUserRegistrationRequest verifyUserRegistrationRequest = new VerifyUserRegistrationRequest()
        {
            email = txtEmail.text,
            verify_code = txtVerificationCode.text
        };

        ServerController.SendToServer(verifyUserRegistrationRequest, OnVerificationCompleted);
    }

    private void OnVerificationCompleted()
    {
        ScreenManager.ShowScreen(Screen.LOGIN_PANEL);
    }
}
