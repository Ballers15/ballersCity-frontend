using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionDoubleOfflineCurrency", menuName = "Mission/DoubleOfflineCurrency")]
	public class MissionDoubleEnergy : Mission {

		protected override float GetProgress() {
			return state == MissionState.CompleteNotReceivedReward || state == MissionState.Complete ? 1f : 0f;
		}

		public override string ToStringProgress() {
			int currentValue = state == MissionState.CompleteNotReceivedReward || state == MissionState.Complete ? 1 : 0;
			return string.Format("{0}/{1}", currentValue, 1);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();
				//IdleManager.OnDoubledCollectedOfflineCurrency += IdleManager_OnDoubledCollectedOfflineCurrency;
			}
		}

		private void IdleManager_OnDoubledCollectedOfflineCurrency() {
			Finish();
		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
				//IdleManager.OnDoubledCollectedOfflineCurrency -= IdleManager_OnDoubledCollectedOfflineCurrency;
			}
		}

		public override void CleanInfo() {
		}
	}
}