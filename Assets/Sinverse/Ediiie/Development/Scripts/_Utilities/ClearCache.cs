using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCache : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetString("version", "") != Constants.VERSION)
        {
            PlayerPrefs.DeleteAll();
            Caching.ClearCache();
        }

        PlayerPrefs.SetString("version", Constants.VERSION);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
