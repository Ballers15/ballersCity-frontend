using Ediiie.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadImageHandler : MonoBehaviour
{
    public void StartDownload(string url, int index, Action<Sprite,int> callBack)
    {
        StartCoroutine(DownloadImage(url, index, callBack));
    }

    public void StartDownload(string url, Action<Sprite> callBack)
    {
        StartCoroutine(DownloadImage(url, callBack));
    }

    private IEnumerator DownloadImage(string url, int index, Action<Sprite, int> result)
    {
        Sprite cardSprite = null;
        Debug.Log(url);

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                try
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    cardSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                }
                catch
                {
                    Debug.LogError("Error while getting texture img");
                }
            }
        }

        result.Invoke(cardSprite, index);
    }

    private IEnumerator DownloadImage(string url, Action<Sprite> result)
    {
        Sprite cardSprite = null;
        Debug.Log(url);

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                cardSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }

        result.Invoke(cardSprite);
    }
}
