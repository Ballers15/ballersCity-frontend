using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker.Upgrades{
	[CreateAssetMenu(fileName = "UpgradeIdleObjectSpeed", menuName = "Upgrade/IdleObjectSpeed")]
	public class UpgradeIdleObjectSpeed : Upgrade {

//		public override void Apply(IdleObject idleObject) {
//			idleObject.ReduceIncomePeriod(multiplicator);
//		}

		protected override Sprite GetIconSpriteUpgrade() {
			return Icons.iconUpgradeSpeed;
		}
	}
}