using System.Collections;
using IdleClicker;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
	public class UICaseCard : MonoBehaviour {

		[SerializeField]
		private Transform pivotTransform;
		[SerializeField]
		private Image imgIcon;
		[SerializeField]
		private Text textDescription;

		public bool isActive { get { return gameObject.activeInHierarchy; } }

		private Transform targetTransform;
		private RewardValue rewardValue;

		public void StartAnimationFly(Transform startPoint, float speed) {
			PrepareToFly(startPoint.position);

			StopCoroutine("FlyRoutine");
			StartCoroutine("FlyRoutine", speed);
		}

		public void PrepareToFly(Vector3 startPosition) {
			pivotTransform.localScale = Vector3.zero;
			pivotTransform.position = startPosition;
			targetTransform = transform;
		}

		private IEnumerator FlyRoutine(float speed) {
			while(Vector3.Distance(pivotTransform.position, targetTransform.position) > 0.02f) {
				pivotTransform.position = Vector3.Lerp(pivotTransform.position, targetTransform.position, speed * Time.deltaTime);
				pivotTransform.localScale = Vector3.Lerp(pivotTransform.localScale, Vector3.one, speed * Time.deltaTime);
				yield return null;
			}
			pivotTransform.position = targetTransform.position;
			pivotTransform.localScale = Vector3.one;
		}

		public void Stop() {
			StopCoroutine("FlyRoutine");
		}

		private void Reset() {
			if (!pivotTransform && transform.childCount > 0)
				pivotTransform = transform.GetChild(0);
		}
	}
}