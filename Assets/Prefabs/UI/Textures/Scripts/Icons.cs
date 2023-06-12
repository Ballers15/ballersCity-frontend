using UnityEngine;

namespace IdleClicker {
	public static class Icons {

		public static Sprite iconUpgradeIncome {
			get {
				if (!m_iconUpgradeIncome)
					m_iconUpgradeIncome = Resources.Load<Sprite>("IconUpgradeIncome");
				return m_iconUpgradeIncome;
			}
		}
		public static Sprite iconUpgradeSpeed {
			get {
				if (!m_iconUpgradeSpeed)
					m_iconUpgradeSpeed = Resources.Load<Sprite>("IconUpgradeSpeed");
				return m_iconUpgradeSpeed;
			}
		}

		public static Sprite iconGlobal {
			get {
				if (!m_iconGlobal)
					m_iconGlobal = Resources.Load<Sprite>("IconGlobal");
				return m_iconGlobal;
			}
		}

		public static Sprite iconPriceEnergy {
			get {
				if (!m_iconPriceEnergy)
					m_iconPriceEnergy = Resources.Load<Sprite>("IconCleanEnegry");
				return m_iconPriceEnergy;
			}
		}

		public static Sprite iconPriceGems{
			get {
				if (!m_iconPriceEnergy)
					m_iconPriceEnergy = Resources.Load<Sprite>("IconGem");
				return m_iconPriceEnergy;
			}
		}

		private static Sprite m_iconUpgradeIncome;
		private static Sprite m_iconUpgradeSpeed;
		private static Sprite m_iconGlobal;
		private static Sprite m_iconPriceEnergy;
		private static Sprite m_iconPriceGems;
	}
}