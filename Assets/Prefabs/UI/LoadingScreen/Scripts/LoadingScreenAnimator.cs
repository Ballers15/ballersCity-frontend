using VavilichevGD;

namespace SinSity.UI {
	public class LoadingScreenAnimator : AnimObject {

		public delegate void ScreenHideHandler();
		public event ScreenHideHandler OnScreenHided;

		private const string BOOL_OPEN = "open";

		public void Play(bool showInstantly) {
			animator.gameObject.SetActive(true);
			SetBoolTrue(BOOL_OPEN);
			SetTrigger("already_opened");
		}

		public void Stop() {
			SetBoolFalse(BOOL_OPEN);
		}

		private void Handle_ScreenDisappeared() {
			OnScreenHided?.Invoke();
		}
	}
}