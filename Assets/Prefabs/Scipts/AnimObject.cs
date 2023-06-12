using UnityEngine;

namespace VavilichevGD {
	public class AnimObject : MonoBehaviour {

		[SerializeField]
		protected Animator animator;

		protected virtual void Reset() {
			if (!animator)
				animator = GetComponentInChildren<Animator>(true);
		}

		protected void SetTrigger(string triggerName) {
			animator.SetTrigger(triggerName);
		}

		protected void SetTrigger(int id) {
			animator.SetTrigger(id);
		}

		protected void ResetTrigger(string triggerName) {
			this.animator.ResetTrigger(triggerName);
		}

		protected void ResetTrigger(int id) {
			this.animator.ResetTrigger(id);
		}

		protected void SetBoolTrue(string boolName) {
			animator.SetBool(boolName, true);
		}

		protected void SetBoolTrue(int id) {
			animator.SetBool(id, true);
		}

		protected void SetBoolFalse(string boolName) {
			animator.SetBool(boolName, false);
		}

		protected void SetBoolFalse(int id) {
			animator.SetBool(id, false);
		}
	}
}