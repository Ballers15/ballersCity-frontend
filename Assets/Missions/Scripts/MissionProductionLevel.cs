using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionProductionLevel", menuName = "Mission/ProductionLevel")]
	public class MissionProductionLevel : Mission {
		[Space]
		[SerializeField]
		private int numberOfProduction;
		[SerializeField]
		private int levelNumber;

//		private IdleObject targetIdleObject;

		protected override float GetProgress() {
			return 1f;
			//return Mathf.Clamp01((float)targetIdleObject.level / levelNumber);
		}

		public override string ToStringProgress() {
			return "1";
//			return string.Format("{0}/{1}", Mathf.Min(targetIdleObject.level, levelNumber), levelNumber);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();

				//targetIdleObject = IdleManager.instance.GetIdleObject(numberOfProduction);
				if (IsMissionAlreadyComplete())
					Finish();
//				else
				//	targetIdleObject.OnLevelRaised += TargetIdleObject_OnLevelRaised; ;
			}
		}

		protected override bool IsMissionAlreadyComplete() {
			return false;
//			return targetIdleObject.level >= levelNumber;
		}

//		private void TargetIdleObject_OnLevelRaised(LevelRaisingArgs e) {
//			if (e.newLevel >= levelNumber)
//				Finish();
//		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
//				targetIdleObject.OnLevelRaised -= TargetIdleObject_OnLevelRaised; ;
			}
		}

		public override void CleanInfo() {
		}
	}
}