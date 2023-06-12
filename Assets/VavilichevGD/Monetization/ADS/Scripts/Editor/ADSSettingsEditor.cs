#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VavilichevGD.Monetization.AdMob;
using VavilichevGD.Monetization.AppLovin;
using VavilichevGD.Monetization.Unity;

namespace VavilichevGD.Monetization {
    public class ADSSettingsEditor : MonoBehaviour {

        [MenuItem("VavilichevGD/ADS/SetupAdMob")]
        public static void SetupSettingsADSAdMob() {
            string path = "Assets/VavilichevGD/Monetization/ADS/Resources/ADSSettingsAdMob.asset";
            LoadOrCreateAsset<ADSSettingsAdMob>(path);
        }

        
        [MenuItem("VavilichevGD/ADS/SetupUnity")]
        public static void SetupSettingsADSUnity() {
            string path = "Assets/VavilichevGD/Monetization/ADS/Resources/ADSSettingsUnity.asset";
            LoadOrCreateAsset<ADSSettingsUnity>(path);
        }
        
        [MenuItem("VavilichevGD/ADS/SetupAppLovin")]
        public static void SetupSettingsADSAppLovin() {
            string path = "Assets/VavilichevGD/Monetization/ADS/Resources/ADSSettingsAppLovin.asset";
            LoadOrCreateAsset<ADSSettingsAppLovin>(path);
        }
        
        
        private static void LoadOrCreateAsset<T>(string path) where T : ScriptableObject{
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