#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    public class LocalizationSettingsSOEditor : MonoBehaviour {
        [MenuItem("VavilichevGD/Localization/CreateSettings")]
        public static void SetupSettingsLocalization() {
            string path = "Assets/VavilichevGD/Localization/Resources/LocalizationSettings.asset";
            LoadOrCreateAsset<LocalizationSettings>(path);
        }

        private static void LoadOrCreateAsset<T>(string path) where T : ScriptableObject {
            T loadedAsset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (loadedAsset) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = loadedAsset;
                return;
            }

            T createdAsset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(createdAsset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = createdAsset;
        }
    }
}
#endif