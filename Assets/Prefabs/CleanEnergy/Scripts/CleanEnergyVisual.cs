using System.Collections;
using UnityEngine;

namespace IdleClicker {
	public class CleanEnergyVisual : MonoBehaviour {

		public AnimationClip animationAppear;
		public AnimationCurve speedCurve;
		public float speedMultiplicator = 1f;

		private Transform targetPoint;
		private Transform myTransform;

		private void Start() {
			LateInitialize();
			StartCoroutine(LifeRoutine());
		}

		private void LateInitialize() {
			myTransform = transform;
		}

		private IEnumerator LifeRoutine() {
			float delay = animationAppear.averageDuration;
			yield return new WaitForSeconds(delay - 0.1f);

			Vector3 originalDirection = (targetPoint.position - myTransform.position);
			Vector3 originalDirectionNormalized = originalDirection.normalized;
			float originalDistanceSqr = originalDirection.sqrMagnitude;

			while (true) {
				Vector3 currentDirection = targetPoint.position - myTransform.position;
				if (Vector3.Angle(currentDirection, originalDirection) > 90f)
					break;

				float currentDistanceSqr = currentDirection.sqrMagnitude;
				float distanceNormalized = (1 - currentDistanceSqr / originalDistanceSqr);
				float finalSpeed = speedMultiplicator * speedCurve.Evaluate(distanceNormalized) * Time.deltaTime;
				myTransform.position += originalDirection * finalSpeed;
				yield return null;
			}
			Destroy(gameObject);
		}

		public void SetTargetPoint(Transform targetPoint) {
			this.targetPoint = targetPoint;
		}
	}
}