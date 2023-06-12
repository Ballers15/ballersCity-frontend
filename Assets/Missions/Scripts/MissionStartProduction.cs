using UnityEngine;

namespace IdleClicker {
	[CreateAssetMenu(fileName = "MissionStartProduction", menuName = "Mission/StartProduction")]
	public class MissionStartProduction : Mission {

		[Space]
		[SerializeField]
		private int numberOfProduction;

//		private IdleObject targetIdleObject;

		protected override float GetProgress() {
			return isFullyCompleted ? 1f : 0f;
		}

		public override string ToStringProgress() {
			int currentValue = isFullyCompleted ? 1 : 0;
			return string.Format("{0}/{1}", currentValue, 1);
		}

		public override void Start() {
			if (!isFullyCompleted) {
				base.Start();

//				IdleObject[] idleObjects = IdleManager.instance.GetIdleObjectsPoolToArray();
//				targetIdleObject = idleObjects[numberOfProduction];
//				if (IsMissionAlreadyComplete())
//					Finish();
//				else
//					IdleObject.OnIdleObjectBuilded += IdleObject_OnIdleObjectBuilded;
			}
		}

		protected override bool IsMissionAlreadyComplete() {
			return false;
//			return targetIdleObject.isBuilded;
		}

//		private void IdleObject_OnIdleObjectBuilded(IdleObject idleObject) {
//			if (idleObject == targetIdleObject)
//				Finish();
//		}

		public override void Finish() {
			if (!isFullyCompleted) {
				base.Finish();
//				IdleObject.OnIdleObjectBuilded -= IdleObject_OnIdleObjectBuilded;
			}
		}

		public override void CleanInfo() {
		}
	}
}