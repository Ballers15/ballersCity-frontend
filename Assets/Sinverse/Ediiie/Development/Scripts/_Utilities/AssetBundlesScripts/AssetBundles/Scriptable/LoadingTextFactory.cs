using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CogniHab.Utils.Data
{
    [CreateAssetMenu(fileName = "LoadingText", menuName = "Scriptable Objects/Loading Text Data")]
    public class LoadingTextFactory : ScriptableObject
    {
        public List<LoadingText> loadingTexts;

        public string GetLoadingText(string keyword)
        {
            LoadingText loading = loadingTexts.Find(x => x.keyword.ToLower() == keyword.ToLower());

            if(loading == null)
            {
                return keyword;
            }

            return loading.loadingText;
        }
    }

    [System.Serializable]
    public class LoadingText
    {
        public string keyword;
        public string loadingText;
    }
}
