using System.Collections.Generic;
using SinSity.Core;
using SinSity.Meta.Rewards;
using SinSity.Services;
using UnityEngine;

namespace Prefabs.AirDrop.Scripts.Analytics {
	public class AirDropAnalytics {

		#region CONSTANTS

		private const string EVENT_NAME_APPEARED = "airdrop_apear";
		private const string EVENT_NAME_CLICKED = "airdrop_clicked";
		private const string EVENT_NAME_RESULT = "airdrop_result";
		private const string PAR_NAME_TYPE = "type";
		private const string PAR_NAME_RESULT = "result";
		private const string PAR_NAME_REWARD = "reward_type";
		public const string TYPE_REGULAR = "regular";
		public const string TYPE_AD = "video_ad";
		public const string RESULT_NONE = "none";
		public const string RESULT_ADWATCHED = "ad_watched";
		public const string RESULT_CANCELED = "canceled";
		public const string REWARD_GEMS = "gems";
		public const string REWARD_CLEAN_ENERGY = "clean_energy";

		#endregion
		
		
		private AirShipController controller;
		
		public AirDropAnalytics(AirShipController controller) {
			this.controller = controller;
			this.controller.OnAirShipClickedEvent += this.OnAirShipClicked;
			this.controller.OnAirShipCreatedEvent += this.OnAirShipCreated;
		}

		
		private void LogAirDropAppeared(string type) {

			var parameters = new Dictionary<string, object> {
				{ PAR_NAME_TYPE, type }
			};
			
			var e = new AnalyticsEvent(EVENT_NAME_APPEARED, parameters);
			CommonAnalytics.Log(e);
		}

		private void LogAirDropClicked(string type) {
			var parameters = new Dictionary<string, object> {
				{ PAR_NAME_TYPE, type }
			};
			
			var e = new AnalyticsEvent(EVENT_NAME_CLICKED, parameters);
			CommonAnalytics.Log(e);
		}

		public void LogAirDropResult(string type, string result, string rewardType) {
			var parameters = new Dictionary<string, object> {
				{ PAR_NAME_TYPE, type },
				{ PAR_NAME_RESULT, result },
				{ PAR_NAME_REWARD, rewardType }
			};
			
			var e = new AnalyticsEvent(EVENT_NAME_RESULT, parameters);
			CommonAnalytics.Log(e);
		}
		


		#region CALLBACKS

		private void OnAirShipClicked(AirShipBehaviour airShipBehaviour) {
			if (airShipBehaviour is AirShipBehaviorRegular) {
				this.LogAirDropClicked(TYPE_REGULAR);
				var rewardType = airShipBehaviour.rewardInfo is RewardInfoSetupSoftCurrency
					? REWARD_CLEAN_ENERGY
					: REWARD_GEMS;
				this.LogAirDropResult(TYPE_REGULAR, RESULT_NONE, rewardType);
			}
			else if (airShipBehaviour is AirShipBehaviorAD) 
				this.LogAirDropClicked(TYPE_AD);
		}
		
		private void OnAirShipCreated(AirShipBehaviour airShipBehaviour) {
			if (airShipBehaviour is AirShipBehaviorRegular)
				this.LogAirDropAppeared(TYPE_REGULAR);
			else if (airShipBehaviour is AirShipBehaviorAD) 
				this.LogAirDropAppeared(TYPE_AD);
		}

		#endregion
		
	}
}