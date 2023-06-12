
using Ediiie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour
{
    private string oldVersion;

    // Start is called before the first frame update
    void Awake()
    {
        CheckVersion();
    }
    
    private void CheckVersion()
    {
        string currentVersion = Application.version;
        oldVersion = PlayerPrefs.GetString(Constants.PP_VERSION, "");
        if (currentVersion != oldVersion)
        {
            PlayerData.ResetData();
            PlayerPrefs.SetString(Constants.PP_VERSION, currentVersion);
        }
    }
}
