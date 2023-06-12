using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker.Upgrades {
	[CreateAssetMenu(fileName = "UpgradeIdleObjectMultiplicator", menuName = "Upgrade/IdleObjectMultiplicator")]
	public class UpgradeIdleObjectMultiplicator : Upgrade {

//		public override void Apply(IdleObject idleObject) {
//			idleObject.AddLocalIncomeMultiplicator(multiplicator);
//		}

		protected override Sprite GetIconSpriteUpgrade() {
			return Icons.iconUpgradeIncome;
		}
	}
}