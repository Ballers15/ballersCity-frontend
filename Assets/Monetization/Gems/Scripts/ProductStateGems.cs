using UnityEngine;
using VavilichevGD.Monetization;

namespace SinSity.Monetization {
    public class ProductStateGems : ProductState {
        
        public ProductStateGems(string stateJson) : base(stateJson) { }

        public ProductStateGems(ProductInfo info) : base(info) { }
        
        public override string GetStateJson() {
            return JsonUtility.ToJson(this);
        }
        
        public override void SetState(string stateJson) {
            ProductStateGems state = JsonUtility.FromJson<ProductStateGems>(stateJson);
            this.id = state.id;
            this.isPurchased = state.isPurchased;
            this.isViewed = state.isViewed;
        }

        public override void SetState(ProductState state) {
            ProductStateGems stateGems = state as ProductStateGems;
            this.id = stateGems.id;
            this.isPurchased = stateGems.isPurchased;
            this.isViewed = stateGems.isViewed;
        }
    }
}