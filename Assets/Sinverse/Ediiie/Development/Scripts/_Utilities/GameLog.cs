using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    public static GameLog Instance;
    public static string debugText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetLog();
    }

    public static void Log(string data)
    {
        debugText += "\n\r" + data; //"\n\r-----------\n\r" + data;

        //Debug.Log(data);
    }

    public static void ResetLog()
    {
        try
        {
            System.IO.File.WriteAllText("E:\\debug.txt", debugText);
        }
        catch (Exception)
        {
            System.IO.File.WriteAllText("D:\\debug.txt", debugText);
        }
    }

    public static void Save()
    {
        try
        {
#if UNITY_EDITOR
            System.IO.File.AppendAllText("E:\\debug.txt", debugText);
#else
        System.IO.File.AppendAllText(Application.persistentDataPath +  "\\debug.txt", debugText);
#endif
        }
        catch (Exception)
        {
#if UNITY_EDITOR
            System.IO.File.AppendAllText("D:\\debug.txt", debugText);
#else
        System.IO.File.AppendAllText(Application.persistentDataPath +  "\\debug.txt", debugText);
#endif
        }

        debugText = "";
    }
}
