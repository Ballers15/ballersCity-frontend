#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Orego.Editor
{
    [CustomPropertyDrawer(typeof(ScriptableObject), true)]
    public sealed class OregoScriptableObjectPropertyDrawner : PropertyDrawer
    {
        private UnityEditor.Editor editor = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.DrawLabel(position, property, label);
            if (property.objectReferenceValue != null)
            {
                this.DrawArrow(position, property);
            }

            if (!property.isExpanded)
            {
                return;
            }

            this.DrawBody(property);
        }

        private void DrawBody(SerializedProperty property)
        {
            EditorGUI.indentLevel++;
            const string style = "box";
            GUILayout.BeginVertical(style);
            if (!this.editor)
            {
                UnityEditor.Editor.CreateCachedEditor(property.objectReferenceValue, null, ref this.editor);
            }

            EditorGUI.BeginChangeCheck();
            if (this.editor)
            {
                this.editor.OnInspectorGUI();
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        private void DrawLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        private void DrawArrow(Rect position, SerializedProperty property)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        }
    }
}
#endif