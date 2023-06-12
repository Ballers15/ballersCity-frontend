using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ediiie.Screens;
using Screen = Ediiie.Screens.Screen;

/// <summary>
/// of no use at present
/// </summary>
public class RegisterView : GameScreenBackForth, IKeyEventListener
{
    [SerializeField] private TMP_InputField txtEmail;
    [SerializeField] private TMP_InputField txtUsernme;
    [SerializeField] private TMP_InputField txtPassword;
    [SerializeField] private TMP_InputField txtFirstName;
    [SerializeField] private TMP_InputField txtLastName;
    [SerializeField] private TMP_InputField txtReferralCode;
    [SerializeField] private Button registerButton;

    public void ClearFields()
    {
        txtEmail.text = "";
        txtUsernme.text = "";
        txtPassword.text = "";
        txtFirstName.text = "";
        txtLastName.text = "";
        txtReferralCode.text = "";
    }


    private void Awake()
    {
        registerButton.onClick.AddListener(CallRegisterAPI);
    }

    private void OnDestroy()
    {
        registerButton.onClick.RemoveListener(CallRegisterAPI);
    }

    //public void OnRegisterButtonClicked()
    //{
    //    Application.OpenURL("https://myforge.vulcanforged.com/");
    //}

    public void CallRegisterAPI()
    {
        RegisterUserRequest registerUserRequest = new RegisterUserRequest()
        {
            email = txtEmail.text,
            username = txtUsernme.text,
            password = txtPassword.text,
            first_name = txtFirstName.text,
            last_name = txtLastName.text,
            referral_code = txtReferralCode.text
        };

        ServerController.SendToServer(registerUserRequest, OnRegistrationCompleted);
    }


    private void OnRegistrationCompleted()
    {
        ScreenManager.ShowScreen(Screen.VERIFY_REGISTRATION_PANEL);
    }

    public void OnKeyPressed(KeyCode keyCode)
    {
        CallRegisterAPI();
    }
}
