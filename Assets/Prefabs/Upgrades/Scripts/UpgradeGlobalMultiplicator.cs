using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker.Upgrades {
	[CreateAssetMenu(fileName = "UpgradeGlobalMultiplicator", menuName = "Upgrade/GlobalMultiplicator")]
	public class UpgradeGlobalMultiplicator : Upgrade {
//		public override void Apply(IdleObject idleObject) {
//			IdleManager.instance.IncreaseGlobalIncomeMultiplicator(multiplicator);
//		}

		protected override Sprite GetIconSpriteUpgrade() {
			return Icons.iconUpgradeIncome;
		}

		public override bool IsGlobal() {
			return true;
		}
	}
}