using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButtonView : BaseButtonView
{
    [SerializeField] private string url;

    protected override void OnButtonClicked()
    {
        Application.OpenURL(url);
    }
}
