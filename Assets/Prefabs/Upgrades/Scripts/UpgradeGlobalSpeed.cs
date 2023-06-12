using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker.Upgrades {
	[CreateAssetMenu(fileName = "UpgradeGlobalSpeed", menuName = "Upgrade/GlobalSpeed")]
	public class UpgradeGlobalSpeed : Upgrade {
//		public override void Apply(IdleObject idleObject) {
//			IdleObject[] idleObjects = IdleManager.instance.GetBuildedIdleObjectsPool();
//			foreach (IdleObject io in idleObjects)
//				io.ReduceIncomePeriod(multiplicator);
//		}

		protected override Sprite GetIconSpriteUpgrade() {
			return Icons.iconUpgradeSpeed;
		}

		public override bool IsGlobal() {
			return true;
		}
	}
}