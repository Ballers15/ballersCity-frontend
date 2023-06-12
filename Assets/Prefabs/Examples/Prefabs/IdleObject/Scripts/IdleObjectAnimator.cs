namespace VavilichevGD.IdleClicker {
	public class IdleObjectAnimator : AnimObject {

		private const string BOOL_WORK = "work";

		public void PlayWork() {
			SetBoolTrue(BOOL_WORK);
		}

		public void StopWork() {
			SetBoolFalse(BOOL_WORK);
		}
	}
}