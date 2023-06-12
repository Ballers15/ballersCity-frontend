using System.Collections.Generic;
using SinSity.Services;

namespace SinSity.UI.Analytics {
	public class PopupOfflineCollectedAnalytics {

		#region CONSTANTS

		private const string EVENT_NAME_RESULT = "popup_offline_collected_results";
		private const string PAR_NAME_RESULT = "result";
		public const string RESULT_CANCELED = "canceled";
		public const string RESULT_DOUBLED_FOR_AD = "doubled_for_ad";
		public const string RESULT_TRIPPLED_FOR_GEMS = "trippled_for_gems";

		#endregion

		public void LogOfflineCollectedResults(string result) {
			var parameters = new Dictionary<string, object> {
				{ PAR_NAME_RESULT, result }
			};
			
			var e = new AnalyticsEvent(EVENT_NAME_RESULT, parameters);
			CommonAnalytics.Log(e);
		}
		
	}
}