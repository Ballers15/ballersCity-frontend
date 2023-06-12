#if UNITY_EDITOR
using UnityEditor;

namespace SinSity.Meta.Editor {
    [CustomEditor(typeof(FortuneWheelConfig))]
    public class FortuneWheelConfigEditor : UnityEditor.Editor {

        private FortuneWheelConfig config;
        
        private void OnEnable() {
            this.config = this.target as FortuneWheelConfig;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if (!this.config.IsChancesValid())
                EditorGUILayout.HelpBox("Sum of chances must be equal 100%", MessageType.Warning);
        }
    }
}
#endif