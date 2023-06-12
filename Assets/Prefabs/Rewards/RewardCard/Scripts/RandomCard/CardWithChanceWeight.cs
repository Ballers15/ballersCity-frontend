using System;
using Ecorobotics;
using SinSity.Core;

namespace SinSity.Meta.Rewards {
    [Serializable]
    public struct CardWithChanceWeight {
        public CardInfo card;
        public int weight;
        
        public ObjectWithWeight GetObjectWithWeight() {
            return new ObjectWithWeight(card, weight);
        }
    }
}