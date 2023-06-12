using UnityEngine;
using VavilichevGD.Tools;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionCollectCleanEnergy", menuName = "Mission/CollectCleanEnergy")]
	public class MissionCollectCleanEnegry : Mission {
		[Space]
		[SerializeField]
		private BigNumber trasholdEditor;

		private BigNumber trashold => trasholdEditor;

		protected override float GetProgress() {
			if (isFullyCompleted)
				return 1f;
//			double resultNormalized = BigNumber.DivideToDouble(IdleManager.totalCurrencyCollected, trashold);
			return 1f;
//			return Mathf.Clamp01((float)resultNormalized);
		}

		public override string ToStringProgress() {
//			BigNumber progressValue = IdleManager.totalCurrencyCollected;
//			if (isFullyCompleted)
//				progressValue = trashold;

			return "1";
//			return string.Format("{0}/{1}", progressValue, trashold.ToString());
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
				if (IsMissionAlreadyComplete())
					Finish();
//				else
//					IdleManager.OnTotalCurrencyChanged += IdleManager_OnTotalCurrencyChanged;
			}
		}

		protected override bool IsMissionAlreadyComplete() {
			return false;
//			return IdleManager.totalCurrencyCollected >= trashold;
		}

//		private void IdleManager_OnTotalCurrencyChanged(BigNumber newCollectedTotalCurrency) {
         //			NotifyAboutMissionStateChanged();
         //			if (newCollectedTotalCurrency >= trashold)
         //				Finish();
         //		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
//				IdleManager.OnTotalCurrencyChanged -= IdleManager_OnTotalCurrencyChanged;
			}
		}

		public override void CleanInfo() {
		}

//		public override string GetDescriptionCode() {
//			string translation = Translator.instance.GetTranslate(descCode);
//			return string.Format(translation, trashold);
//		}
	}
}