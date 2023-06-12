using Ediiie.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutButtonView : BaseButtonView
{
    protected override void OnButtonClicked()
    {
        BaseModel.Logout();
        SceneController.LoadScene(new SceneProperties(Constants.Scene.LOGIN_SCENE));
    }
}
