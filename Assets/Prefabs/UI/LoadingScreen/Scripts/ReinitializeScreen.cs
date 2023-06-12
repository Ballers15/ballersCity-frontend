using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
	public class ReinitializeScreen : MonoBehaviour {

		#region STATIC

		private static ReinitializeScreen instance { get; set; }
		private static bool isInitialized => instance != null;

		#endregion

		[SerializeField] private Text textVersion;
		
		private LoadingScreenAnimator animator;

		private const string PATH_PREFAB = "[REINITIALIZE SCREEN]";

		private void OnEnable() {
			textVersion.text = $"v. {Application.version}";
		}

		public static void Show(bool instantly) {
			CreateSingletonIfNeed();

			instance.animator.Play(instantly);
			instance.animator.OnScreenHided += OnScreenHided;
		}

		private static void OnScreenHided() {
			instance.animator.OnScreenHided -= OnScreenHided;
			Destroy(instance.gameObject);
		}

		private static void CreateSingletonIfNeed() {
			if (instance == null) {
				var prefab = Resources.Load<ReinitializeScreen>(PATH_PREFAB);
				instance = Instantiate(prefab);
				instance.transform.SetAsLastSibling();
			}
		}

		public static void Hide() {
			if (!isInitialized)
				return;
			instance.animator.Stop();
		}

		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			animator = gameObject.GetComponentInChildren<LoadingScreenAnimator>(true);
		}
	}
}