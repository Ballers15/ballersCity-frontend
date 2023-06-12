using UnityEngine;

namespace SinSity.Core {
	public class AirShipBehaviorAD : AirShipBehaviour {
		
		#region CONSTANTS

		private static readonly int triggerFlyAway = Animator.StringToHash("fly_away");

		#endregion

		public void FlyAway() {
			this.animator.SetTrigger(triggerFlyAway);
		}
	}
}