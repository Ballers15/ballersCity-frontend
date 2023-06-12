using System;
using Ecorobotics;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    [Serializable]
    public struct RewardWithChanceWeight {
        public RewardInfo reward;
        public int weight;

        public ObjectWithWeight GetObjectWithWeight() {
            return new ObjectWithWeight(reward, weight);
        }
    }
}