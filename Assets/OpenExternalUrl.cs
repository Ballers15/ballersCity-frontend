using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExternalUrl : MonoBehaviour
{
    public string websiteUrl = "https://ballers.fun"; // replace this with the URL of the website you want to open

    public void OpenWebsiteUrl()
    {
        Application.OpenURL(websiteUrl);
    }
}
