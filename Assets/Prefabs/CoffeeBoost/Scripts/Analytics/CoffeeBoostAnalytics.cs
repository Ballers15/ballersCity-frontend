using System.Collections.Generic;
using SinSity.Services;
using VavilichevGD.Monetization;

namespace IdleClicker.Gameplay {
	public static class CoffeeBoostAnalytics {

		public static void LogCoffeeBoostUnlocked() {
			var eventName = "coffee_boost_unlocked";
			var e = new AnalyticsEvent(eventName);
			CommonAnalytics.Log(e);
		}

		public static void LogCoffeeBoostShown(int level) {
			var eventName = "coffee_boost_widget_show";
			var parameters = new Dictionary<string, object> {
				{ "level", level.ToString() }
			};

			var e = new AnalyticsEvent(eventName, parameters);
			CommonAnalytics.Log(e);
		}

		public static void LogCoffeeBoostPopupResults(int level, PaymentType paymentType, string result) {
			var eventName = "coffee_boost_popup_results";
			var parameters = new Dictionary<string, object> {
				{ "level", level.ToString() }, 
				{ "payment_type", paymentType.ToString() }, 
				{ "result", result }
			};
			
			var e = new AnalyticsEvent(eventName, parameters);
			CommonAnalytics.Log(e);
		}
		
	}
}