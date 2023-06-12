using System.Collections.Generic;

namespace SinSity.Services {
    public abstract class AnalyticsBehavior {

        public abstract void LogEvent(string eventKey, Dictionary<string, object> eventParameters);
        public abstract void LogEvent(string eventKey);

    }
}