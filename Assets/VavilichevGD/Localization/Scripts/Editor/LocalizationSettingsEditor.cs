#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VavilichevGD.LocalizationFramework {
    [CustomEditor(typeof(LocalizationSettings))]
    public class LocalizationSettingsEditor : Editor {

        private LocalizationSettings settings;

        private void OnEnable() {
            Initialize();
        }

        private void Initialize() {
            settings = target as LocalizationSettings;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update tables"))
                settings.UpdateAllTables();
        }
    }
}
#endif