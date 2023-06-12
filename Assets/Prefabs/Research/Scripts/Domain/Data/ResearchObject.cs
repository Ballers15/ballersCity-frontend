using SinSity.Core;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    public sealed class ResearchObject
    {
        public ResearchObjectInfo info { get; }

        public ResearchObjectState state { get; }

        public ResearchObject(ResearchObjectInfo info, ResearchObjectState state)
        {
            this.info = info;
            this.state = state;
        }

        public ResearchData ToData()
        {
            return new ResearchData
            {
                id = this.info.id,
                isEnabled = this.state.isEnabled,
                remainingTimeSec = this.state.remainingTimeSec,
                isRewardReady = this.state.isRewardReady,
                priceBigNumberStr = this.state.price.ToString(BigNumber.FORMAT_FULL)
            };
        }
    }
}