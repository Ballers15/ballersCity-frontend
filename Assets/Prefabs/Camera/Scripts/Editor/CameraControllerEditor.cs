#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace IdleClicker {
	[CustomEditor(typeof(CameraController))]
	public class CameraControllerEditor : Editor{

		private CameraController camera;

		private void OnEnable() {
			Initialize();
		}

		private void Initialize() {
			camera = target as CameraController;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			EditorGUILayout.Space();
			if (GUILayout.Button("Up")) {
				if (Application.isPlaying)
					camera.MoveUp();
				else
					camera.MoveUpInstantly();
			}
			if (GUILayout.Button("Down")) {
				if (Application.isPlaying)
					camera.MoveDown();
				else
					camera.MoveDownInstantly();
			}

			if (GUILayout.Button("Reset position"))
				camera.ResetCamera();
		}
	}
}
#endif