using System;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using VavilichevGD.Tools;

#endif


namespace VavilichevGD.LocalizationFramework {
    [Serializable]
    public class LocalizationLanguageSettings {
        
        [SerializeField] protected string m_code = "ENG";
        [SerializeField] protected SystemLanguage m_language = SystemLanguage.English;
        [SerializeField] protected string m_languageTitle;
        [SerializeField] protected TextAsset m_tableAsset;
        [SerializeField] private string m_urlGoogleSpreadsheet;

        public SystemLanguage language => m_language;
        public string code => this.m_code;
        public string languageTitle => m_languageTitle;
        public TextAsset tableAsset => m_tableAsset;
        private string urlGoogleSpreadsheet => $"{m_urlGoogleSpreadsheet}/gviz/tq?tqx=out:csv";


#if UNITY_EDITOR
        public void UpdateSpreadsheetsInEditorMode() {
            WWW www = new WWW(urlGoogleSpreadsheet);
            www.MoveNext();

            while (!www.isDone)
                www.MoveNext();

            if (!string.IsNullOrEmpty(www.error))
                Logging.LogError(string.Format("Dictionary ERROR: {www.error}, with object {m_languageTitle}"));
            else
                UpdateTextAsset(www.text);
        }

        private void UpdateTextAsset(string text) {
            if (m_tableAsset == null) {
                TextAsset textAsset = new TextAsset();
                string path = $"Assets/VavilichevGD/Localization/Spreadsheets/LocalizationTable_{languageTitle}.csv";
                
                if (File.Exists(path))
                    m_tableAsset = (TextAsset) AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
                else {
                    AssetDatabase.CreateAsset(textAsset, path);
                    m_tableAsset =
                        (TextAsset) AssetDatabase.LoadAssetAtPath(
                            AssetDatabase.GetAssetPath(textAsset), typeof(TextAsset));
                }
            }

            string textAssetPath = UnityEditor.AssetDatabase.GetAssetPath(m_tableAsset);
            WriteTextToFile(textAssetPath, text);
            Debug.Log($"LOCALIZATION: Tables updated");
        }

        private void WriteTextToFile(string path, string text) {
            if (!string.IsNullOrEmpty(text)) {
                File.CreateText(path).Dispose();
                using (TextWriter writer = new StreamWriter(path, false)) {
                    writer.Write(text);
                    writer.Close();
                }
            }
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(this.m_tableAsset);
        }
#endif
    }
}