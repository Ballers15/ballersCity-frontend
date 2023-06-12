using UnityEngine;
using VavilichevGD.IdleClicker;

namespace IdleClicker.Upgrades {

	[System.Serializable]
	public abstract class Upgrade : ScriptableObject {

		[SerializeField]
		protected int multiplicator = 1;
		[SerializeField]
		protected string m_descCode;

		public string descCode { get { return m_descCode; } }
		public Sprite iconSpriteUpgrade { get { return GetIconSpriteUpgrade(); } }
		public int upgradeValue { get { return GetUpgradeValue(); } }

		//public abstract void Apply(IdleObject idleObject);
		protected abstract Sprite GetIconSpriteUpgrade();

		public virtual bool IsGlobal() {
			return false;
		}

		protected int GetUpgradeValue() {
			return multiplicator;
		}
	}
}