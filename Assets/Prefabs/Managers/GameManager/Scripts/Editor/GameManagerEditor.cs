//using UnityEngine;
//using UnityEditor;
//
//namespace EcoClicker {
//	[CustomEditor(typeof(GameManager))]
//	public class GameManagerEditor : Editor {
//		private GameManager gameManager;
//
//		private void OnEnable() {
//			Initialize();
//		}
//
//		private void Initialize() {
//			gameManager = target as GameManager;
//		}
//
//		public override void OnInspectorGUI() {
//			base.OnInspectorGUI();
//
//			EditorGUILayout.Space();
//			//if (GameManager.instance.testMode) {
//			//	if (GUILayout.Button("Deacitvate test mode"))
//			//		GameManager.instance.DeacivateTestMode();
//			//}
//			//else {
//			//	if (GUILayout.Button("Activate test mode"))
//			//		GameManager.instance.ActivateTestMode();
//			//}
//		}
//	}
//}