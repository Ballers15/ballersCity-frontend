using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionOpenCases", menuName = "Mission/OpenCases")]
	public class MissionOpenCases : Mission {

		[Space]
		[SerializeField]
		private int count;

		private string prefKeyCasesOpenedCount { get { return string.Format("CASES_OPENED_{0}", titleCode); } }
		private int openedCases { get { return Loader.LoadInteger(prefKeyCasesOpenedCount, 0); } set { Loader.SetInteger(prefKeyCasesOpenedCount, value); } }

		protected override float GetProgress() {
			return Mathf.Clamp01((float)openedCases / count);
		}

		public override string ToStringProgress() {
			return string.Format("{0}/{1}", openedCases, count);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
//				ShopProductCase.OnCaseOpened += ShopProductCase_OnCaseOpened;
			}
		}

//		private void ShopProductCase_OnCaseOpened(ShopProductCase shopProductCase) {
//			openedCases += 1;
//			if (openedCases >= count)
//				Finish();
//		}

		public override void Finish() {
			base.Finish();
//			ShopProductCase.OnCaseOpened -= ShopProductCase_OnCaseOpened;
		}

		public override void CleanInfo() {
			Loader.Clean(prefKeyCasesOpenedCount);
		}
	}
}