using UnityEngine;
using VavilichevGD.IdleClicker;
using VavilichevGD.Tools;

namespace IdleClicker.Upgrades{
	[CreateAssetMenu(fileName = "UpgradeBlock", menuName = "UpgradeBlock")]
	public class UpgradesBlock : ScriptableObject {

		[SerializeField]
		private UpgradeInfo[] upgradeInfoPool;
		[SerializeField]
		private float m_priceMultiplicator = 3f;
		[SerializeField]
		private BigNumber m_priceBase;

		public UpgradeInfo upgradeInfoCurrent {
			get {
				if (currentUpgradeIndex < upgradeInfoPool.Length)
					return upgradeInfoPool[currentUpgradeIndex];
				else
					return null;
			}
		}
		public bool hasUpgrades { get { return currentUpgradeIndex < upgradeInfoPool.Length; } }
		public float priceMultiplicator { get { return m_priceMultiplicator; } }
		public BigNumber priceBase => m_priceBase;

		private string prefKeyCurrentUpgradeIndex { get { return string.Format("{0}_UPGRADE_INDEX", name); } }

		private int currentUpgradeIndex;
//		private IdleObject idleObjectOwner;

//		public void Initialize(IdleObject idleObject) {
//			idleObjectOwner = idleObject;
//			currentUpgradeIndex = Loader.LoadInteger(prefKeyCurrentUpgradeIndex, 0);
//			IdleObjectEcoClicker ioEcoClicker = idleObject as IdleObjectEcoClicker;
//			for(int index = 0; index < upgradeInfoPool.Length; index++) {
//				UpgradeInfo info = upgradeInfoPool[index];
//				info.SetIdleObject(ioEcoClicker);
//				info.SetUpgradeIndex(index);
//				info.SetUpgradesBlock(this);
//			}
//		}

		public void Reset() {
			currentUpgradeIndex = 0;
		}

		public void Apply() {
			upgradeInfoCurrent.Apply();
			NextUpgrade();
		}

		private void NextUpgrade() {
			currentUpgradeIndex++;
			Loader.SetInteger(prefKeyCurrentUpgradeIndex, currentUpgradeIndex);
		}

		public BigNumber GetPriceOfBlock(int blockIndex) {
			return priceBase * Mathf.Pow(m_priceMultiplicator, blockIndex);
		}

		public Sprite GetIconIdleObject() {
			return upgradeInfoCurrent.GetSpriteMain();
		}
	}
}