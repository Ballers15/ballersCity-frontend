using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionCollectIncomeCount", menuName = "Mission/CollectIncomeCount")]
	public class MissionCollectIncomeCount : Mission {

		[Space]
		[SerializeField]
		private int count;

		private string prefKeyProgress { get { return string.Format("{0}_{1}_MISSION_PROGRESS", titleCode, count); } }
		private int countCollected {
			get {
				return Loader.LoadInteger(prefKeyProgress, 0);
			}
			set {
				Loader.SetInteger(prefKeyProgress, value);
			}
		}

		protected override float GetProgress() {
			return Mathf.Clamp01((float)countCollected / count);
		}

		public override string ToStringProgress() {
			return string.Format("{0}/{1}", countCollected, count);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
				//IdleObject.OnIdleObjectTapAndReceivedCurrency += IdleObject_OnTaped;
			}
		}

//		private void IdleObject_OnTaped(IdleObject idleObject) {
//			countCollected = countCollected + 1;
//			if (countCollected >= count)
//				Finish();
//		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
				//IdleObject.OnIdleObjectTapAndReceivedCurrency -= IdleObject_OnTaped;
			}
		}

		public override void CleanInfo() {
			Loader.Clean(prefKeyProgress);
		}
	}
}