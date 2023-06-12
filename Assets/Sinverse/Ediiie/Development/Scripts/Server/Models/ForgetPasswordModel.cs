using Ediiie.Model;
using Notification;
using Sinverse;
using System;
using UnityEngine;

public class ForgetPasswordModel : UserModel<ForgetPasswordRequest>
{
    private void Awake()
    {
        Instance = this;
    }

    public override void ProcessRequest(ForgetPasswordRequest data, Action callback)
    {
        base.ProcessRequest(data, callback);
        if (IsValidData(data))
        {
            SetForm(data);
            CallAPI(url, wwwForm, api_name);
            NotificationController.ShowLoading(true);
        }
    }

    protected override void SetForm(ForgetPasswordRequest data)
    {
        base.SetForm(data);
        wwwForm.AddField("email", data.email);
    }

    private bool IsValidData(ForgetPasswordRequest data)
    {
        string msg = "";       
        if (Utils.IsEmpty(data.email))
        {
            msg = "Please enter email";
        }
        else if (!SinverseUtils.IsValidEmail(data.email))
        {
            msg = "Please enter valid email";
        }

        if (msg.Length > 0)
        {
            NotificationController.Notify(msg, false);
            return false;
        }

        PlayerPrefs.SetString("Temp_Email", data.email);

        return true;
    }

    protected override void APIResultHandler<T1>(STATUS _status, object data)
    {
        base.APIResultHandler<T1>(_status, data);
    }

    protected override void APISuccessHandler(object data)
    {
        base.APISuccessHandler(data);
        Debug.Log(data.ToString());

        NotificationController.ShowLoading(false);
        CallBack?.Invoke();
    }

    protected override void APIErrorHandler(string msg)
    {
        base.APIErrorHandler(msg);
        Debug.Log(msg);
    }
}

public class ForgetPasswordRequest
{
    public string email;
}

[Serializable]
public class ForgetPasswordResponse : APIResult<int>
{

}
