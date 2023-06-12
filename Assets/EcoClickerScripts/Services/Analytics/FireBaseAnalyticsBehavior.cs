using System.Collections.Generic;
//using Firebase.Analytics;

namespace SinSity.Services {
    public class FireBaseAnalyticsBehavior : AnalyticsBehavior{
        
        public override void LogEvent(string eventKey, Dictionary<string, object> eventParameters) {
            /*var parameters = new List<Parameter>();
            foreach (var eventParameter in eventParameters) {
                var parameter = new Parameter(eventParameter.Key, eventParameter.Value.ToString());
                parameters.Add(parameter);
            }
            
            FirebaseAnalytics.LogEvent(eventKey, parameters.ToArray());*/
        }

        public override void LogEvent(string eventKey) {
            //FirebaseAnalytics.LogEvent(eventKey);
        }
    }
}