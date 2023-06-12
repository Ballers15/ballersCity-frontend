using System;
using System.Collections.Generic;

namespace VavilichevGD.Monetization {
    [Serializable]
    public class ProductStates {
        public List<string> listOfStates;

        public static ProductStates empty {
            get {
                ProductStates states = new ProductStates();
                return states;
            }
        }

        public ProductStates() {
            listOfStates = new List<string>();
        }

        public ProductStates(ProductState[] statesArray) {
            listOfStates = new List<string>();
            foreach (ProductState state in statesArray) {
                string stateJson = state.GetStateJson();
                listOfStates.Add(stateJson);
            }
        }
    }
}