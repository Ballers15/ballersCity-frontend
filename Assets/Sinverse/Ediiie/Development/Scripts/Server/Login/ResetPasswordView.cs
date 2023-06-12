using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ediiie.Screens;
using Screen = Ediiie.Screens.Screen;

public class ResetPasswordView : GameScreenBackForth
{
    [SerializeField] private TMP_InputField txtEmail;
    [SerializeField] private TMP_InputField txtResetPasswordCode;
    [SerializeField] private TMP_InputField txtPassword;
    [SerializeField] private Button authenticateButton;

    private void Awake()
    {
        authenticateButton.onClick.AddListener(CallVerificationAPI);
    }

    private void OnEnable()
    {
        //txtEmail.text = "";
        txtResetPasswordCode.text = "";
        txtPassword.text = "";
        txtEmail.interactable = false;
        txtEmail.text = PlayerPrefs.GetString("Temp_Email");
    }

    private void OnDestroy()
    {
        authenticateButton.onClick.RemoveListener(CallVerificationAPI);
    }

    public void CallVerificationAPI()
    {
        ResetPasswordRequest resetPasswordRequest = new ResetPasswordRequest()
        {
            email = txtEmail.text,
            reset_password_code = txtResetPasswordCode.text,
            password = txtPassword.text
        };

        ServerController.SendToServer(resetPasswordRequest, OnVerificationCompleted);
        //PlayerPrefs.DeleteKey("Temp_Email");
    }

    private void OnVerificationCompleted()
    {
        ScreenManager.ShowScreen(Screen.LOGIN_PANEL);
    }
}
