using System;
using System.Runtime.Serialization;

namespace VavilichevGD.Monetization {
    [DataContract]
    [Serializable]
    public class ProductState {
        [DataMember]
        public string id;
        [DataMember]
        public bool isPurchased;
        [DataMember]
        public bool isViewed;


        public ProductState(string stateJson) {
            SetState(stateJson);
        }

        public ProductState(ProductInfo info) {
            this.id = info.GetId();
            this.isPurchased = false;
            this.isViewed = false;
        }

        public virtual void SetState(string stateJson) {
            throw new NotImplementedException();
        }

        public virtual void SetState(ProductState state) {
            throw new NotImplementedException();
        }

        public virtual string GetStateJson() {
            throw new NotImplementedException();
        }

        public void MarkAsPurchased() {
            isPurchased = true;
        }

        public void MarkAsViewed() {
            isViewed = true;
        }
    }
}