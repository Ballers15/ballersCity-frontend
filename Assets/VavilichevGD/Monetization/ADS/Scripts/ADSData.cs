using SinSity.Analytics;

namespace VavilichevGD.Monetization {
    public struct ADSData {
        public string placementId;
        public ADType adType;
        public bool isConnected;

        public ADSData(string placementId, ADType type, bool isConnected) {
            this.placementId = placementId;
            this.adType = type;
            this.isConnected = isConnected;
        }
    }
}