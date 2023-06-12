using System;

namespace VavilichevGD.Monetization {
    [Serializable]
    public class ADSState {
        public bool isActive;

        public static ADSState GetDefault() {
            ADSState state = new ADSState();
            state.isActive = true;
            return state;
        }
    }
}