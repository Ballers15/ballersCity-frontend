#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace VavilichevGD.Tools.Numerics {
    [CustomPropertyDrawer(typeof(BigNumber))]
    public class BigNumberEditor : PropertyDrawer {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            SerializedProperty value = property.FindPropertyRelative("cutValue");
            SerializedProperty order = property.FindPropertyRelative("order");

            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            int lastIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect valueRect = new Rect(position.x, position.y, position.width - 95, position.height);
            Rect numberTypeRect = new Rect(position.x + valueRect.width + 5, position.y, 90, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(numberTypeRect, order, GUIContent.none);
            BigNumberOrder numberOrder = (BigNumberOrder) order.enumValueIndex;
            float clampedValue = Clamp(value.floatValue, numberOrder);
            value.floatValue = EditorGUI.FloatField(valueRect, clampedValue);

            // Set indent back to what it was
            EditorGUI.indentLevel = lastIndent;

            EditorGUI.EndProperty();

            property.serializedObject.ApplyModifiedProperties();
        }

        private float Clamp(float value, BigNumberOrder order) {
            float clampedValue = Mathf.Clamp(value, 0f, 999.9f);
            if (order < BigNumberOrder.Thousands)
                clampedValue = Mathf.FloorToInt(clampedValue);
            else
                clampedValue = (float) Math.Round(clampedValue, 3);
			
            return clampedValue;
        }
    }
}
#endif