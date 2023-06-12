using System.Collections.Generic;

namespace SinSity.Services {
    public struct AnalyticsEvent {
        public string eventName;
        public Dictionary<string, object> parameters;

        public bool isEmpty => this.parameters == null;

        public AnalyticsEvent(string eventName) {
            this.eventName = eventName;
            this.parameters = null;
        }

        public AnalyticsEvent(string eventName, Dictionary<string, object> parameters) {
            this.eventName = eventName;
            this.parameters = parameters;
        }
    }
}