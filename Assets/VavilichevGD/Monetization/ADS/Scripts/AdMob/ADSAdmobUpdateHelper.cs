using UnityEngine;

namespace VavilichevGD.Monetization.AdMob {
    public class ADSAdmobUpdateHelper : MonoBehaviour {
        public delegate void UpdateHandler();
        public event UpdateHandler OnUpdate;
        
        private void Update() {
            OnUpdate?.Invoke();
        }
    }
}
