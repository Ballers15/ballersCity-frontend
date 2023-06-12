using UnityEngine;
using VavilichevGD.Tools;

namespace IdleClicker {
	public class RewardValue {

		private BigNumber bigNumberValue;
		private int intValue;
//		private ShopProductBooster boosterValue;

		public RewardValue(BigNumber value) {
			bigNumberValue = value;
			intValue = -1;
//			boosterValue = null;
		}

		public RewardValue(int value) {
			intValue = Mathf.Max(value, 0);
			bigNumberValue = new BigNumber();
//			boosterValue = null;
		}

//		public RewardValue(ShopProductBooster booster) {
//			boosterValue = booster;
//			intValue = -1;
//			bigNumberValue = null;
//		}

		public override string ToString() {
			return bigNumberValue.ToString();
		}
	}
}