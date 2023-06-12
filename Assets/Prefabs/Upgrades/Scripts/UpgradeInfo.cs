using VavilichevGD.IdleClicker;
using UnityEngine;
using VavilichevGD.Tools;

namespace IdleClicker.Upgrades {
	[System.Serializable]
	public class UpgradeInfo {
		[SerializeField]
		private Upgrade m_upgrade;
		[SerializeField]
		private int m_idleObjectIndex;

		public BigNumber price { get { return block.GetPriceOfBlock(blockIndex); } }
		public string descCode { get { return m_upgrade.descCode; } }
		public Sprite iconSpriteUpgrade { get { return m_upgrade.iconSpriteUpgrade; } }
		public int upgradeValue { get { return m_upgrade.upgradeValue; } }

//		public IdleObjectEcoClicker forIdleObject { get; private set; }
		private int blockIndex;
		private UpgradesBlock block;

		public Sprite GetSpriteMain() {
//			if (m_upgrade.IsGlobal())
				return Icons.iconGlobal;
//			else
//				return forIdleObject.iconSprite;
		}

//		public void SetIdleObject(IdleObjectEcoClicker idleObject) {
//			forIdleObject = idleObject;
//		}

		public void SetUpgradeIndex(int blockIndex) {
			this.blockIndex = blockIndex;
		}

		public void SetUpgradesBlock(UpgradesBlock block) {
			this.block = block;
		}

		public void Apply() {
			//m_upgrade.Apply(forIdleObject);
		}

		public void Purchase() {
//			IdleManager.instance.SubtructFromTotal(price);
			Apply();
		}

		public bool CanPurchase() {
			return false;
//			return IdleManager.instance.IsEnoughTotalCurrency(price);
		}
	}
}