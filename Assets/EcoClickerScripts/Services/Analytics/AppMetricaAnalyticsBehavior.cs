using System.Collections.Generic;

namespace SinSity.Services {
    public class AppMetricaAnalyticsBehavior : AnalyticsBehavior {
        
        public override void LogEvent(string eventKey, Dictionary<string, object> eventParameters) {
            //AppMetrica.Instance.ReportEvent(eventKey, eventParameters);
        }

        public override void LogEvent(string eventKey) {
            //AppMetrica.Instance.ReportEvent(eventKey);
        }
    }
}