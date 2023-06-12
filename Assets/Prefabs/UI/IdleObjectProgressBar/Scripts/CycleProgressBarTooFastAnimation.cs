using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker.UI {
	public class CycleProgressBarTooFastAnimation : MonoBehaviour {

		public float speed = 1f;
		private Material material;

		private void OnEnable() {
			Initialize();
		}

		private void Initialize() {
			Image img = GetComponent<Image>();
			material = img.material;
		}

		private void Update() {
			Vector2 currentOffset = material.mainTextureOffset;
			float newOffsetX = Clamp01(currentOffset.x + speed * Time.deltaTime);
			Vector2 newOffset = new Vector2(newOffsetX, currentOffset.y);
			material.mainTextureOffset = newOffset;
		}

		private float Clamp01(float value) {
			if (value > 1)
				return value - 1f;
			if (value < 0)
				return 1 + value;
			return value;
		}
	}
}