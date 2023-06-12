using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public class Utils
{
    public static string DeviceID => SystemInfo.deviceUniqueIdentifier;

    public static T ConvertValue<T>(object value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public static bool IsEmpty(string value)
    {
        return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
    }

    public static string ToTitleCase(string value)
    {
        // Convert to proper case.
        CultureInfo culture_info = new CultureInfo(1);
        TextInfo text_info = culture_info.TextInfo;
        value = text_info.ToTitleCase(value);
        return value;
    }

    public static int Negate(int value)
    {
        return value * -1;
    }

    public static void DownloadImage(string url, System.Action<Sprite> callback)
    {
        GameObject obj = new GameObject();
        DownloadImageHandler downloadImageHandler = obj.AddComponent<DownloadImageHandler>();
        downloadImageHandler.StartDownload(url, callback);
    }

    public static void DownloadImage(string url, int index, System.Action<Sprite, int> callback)
    {
        GameObject obj = new GameObject();
        DownloadImageHandler downloadImageHandler = obj.AddComponent<DownloadImageHandler>();
        downloadImageHandler.StartDownload(url, index, callback);
    }

    public static int PercentToInt(float percent, float value)
    {
        return Mathf.RoundToInt(Percent(percent, value));
    }

    public static float Percent(float percent, float value)
    {
        value += value * (percent/100);
        return value;
    }
}
