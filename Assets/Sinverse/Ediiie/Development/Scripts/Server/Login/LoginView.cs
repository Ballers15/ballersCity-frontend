using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Ediiie.Screens;
using Screen = Ediiie.Screens.Screen;

public class LoginView : GameScreen, IKeyEventListener
{
    [SerializeField] private TMP_InputField txtEmail;
    [SerializeField] private TMP_InputField txtPassword;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button forgetPasswordButton;
    [SerializeField] private SceneProperties sceneProperties;

    [SerializeField] private RegisterView registreView;


    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        registerButton.onClick.AddListener(OnRegisterButtonClicked);
        forgetPasswordButton.onClick.AddListener(OnForgetPasswordButtonClicked);
    }

    private void OnDestroy()
    {
        loginButton.onClick.RemoveListener(OnLoginButtonClicked);
        registerButton.onClick.RemoveListener(OnRegisterButtonClicked);
        forgetPasswordButton.onClick.RemoveListener(OnForgetPasswordButtonClicked);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        txtEmail.text = "";
        txtPassword.text = ""; 
    }

    public void OnRegisterButtonClicked()
    {
        ScreenManager.ShowScreen(Screen.REGISTRATION_PANEL);
        //Application.OpenURL("https://myforge.vulcanforged.com/");
        registreView.ClearFields();
    }
    
    public void OnForgetPasswordButtonClicked()
    {
        ScreenManager.ShowScreen(Screen.FORGET_PASSWORD_PANEL);
    }

    public void OnLoginButtonClicked()
    {
        UserAuthRequest userAuthRequest = new UserAuthRequest()
        {
            username = txtEmail.text,
            password = txtPassword.text,
        };

        ServerController.SendToServer(userAuthRequest, OnLoginComplete);
    }

    private void OnLoginComplete()
    {
        //loginButton.gameObject.SetActive(false);
        loginButton.interactable = false;
        //SceneController.LoadScene(sceneProperties);
    }

    public void OnKeyPressed(KeyCode keyCode)
    {
        OnLoginButtonClicked();
    }
}
