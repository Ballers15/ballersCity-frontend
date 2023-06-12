//using System;
//using System.Collections;
//using System.Linq;
//using System.Reflection;
//using UnityEditor;
//using UnityEngine;
//
//namespace VavilichevGD {
//	[CustomPropertyDrawer(typeof(BigNumberOld))]
//	public class BigNumberOldEditor : PropertyDrawer {
//
//		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//			SerializedProperty oneOfProperty = property.FindPropertyRelative("editorValue");
//			BigNumberOld numberOldCurrent = GetParent(oneOfProperty) as BigNumberOld;
//
//			EditorGUI.BeginProperty(position, label, property);
//
//			// Draw label
//			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//
//			// Don't make child fields be indented
//			var indent = EditorGUI.indentLevel;
//			EditorGUI.indentLevel = 0;
//
//			// Calculate rects
//			Rect valueRect = new Rect(position.x, position.y, position.width - 95, position.height);
//			Rect numberTypeRect = new Rect(position.x + valueRect.width + 5, position.y, 90, position.height);
//
//			// Draw fields - passs GUIContent.none to each so they are drawn without labels
//			int tempValue = EditorGUI.IntField(valueRect, numberOldCurrent.editorValue);
//			numberOldCurrent.editorValue = Mathf.Clamp(tempValue, 0, 999);
//			numberOldCurrent.editorNumberType = (NumberType)EditorGUI.EnumPopup(numberTypeRect, numberOldCurrent.editorNumberType);
//
//			BigNumberOld result = new BigNumberOld(numberOldCurrent.editorNumberType, numberOldCurrent.editorValue);
//
//			numberOldCurrent.bigIntegerValue = result.bigIntegerValue;
//
//			// Set indent back to what it was
//			EditorGUI.indentLevel = indent;
//
//			EditorGUI.EndProperty();
//
//			EditorUtility.SetDirty(property.serializedObject.targetObject);
//		}
//
//		public object GetParent(SerializedProperty prop) {
//			var path = prop.propertyPath.Replace(".Array.data[", "[");
//			object obj = prop.serializedObject.targetObject;
//			var elements = path.Split('.');
//			foreach (var element in elements.Take(elements.Length - 1)) {
//				if (element.Contains("[")) {
//					var elementName = element.Substring(0, element.IndexOf("["));
//					var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
//					obj = GetValue(obj, elementName, index);
//				}
//				else {
//					obj = GetValue(obj, element);
//				}
//			}
//			return obj;
//		}
//
//		public object GetValue(object source, string name) {
//			if (source == null)
//				return null;
//			var type = source.GetType();
//			var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
//			if (f == null) {
//				var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
//				if (p == null)
//					return null;
//				return p.GetValue(source, null);
//			}
//			return f.GetValue(source);
//		}
//
//		public object GetValue(object source, string name, int index) {
//			var enumerable = GetValue(source, name) as IEnumerable;
//			var enm = enumerable.GetEnumerator();
//			while (index-- >= 0)
//				enm.MoveNext();
//			return enm.Current;
//		}
//	}
//}