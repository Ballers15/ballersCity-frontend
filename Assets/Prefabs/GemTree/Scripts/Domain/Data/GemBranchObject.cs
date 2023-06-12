using SinSity.Core;
using SinSity.Repo;

namespace SinSity.Domain
{
    public sealed class GemBranchObject
    {
        public GemBranchObjectInfo info { get; }

        public GemBranchObjectState state { get; }

        public GemBranchObject(GemBranchObjectInfo info, GemBranchObjectState state)
        {
            this.info = info;
            this.state = state;
        }

        public GemBranchData ToData()
        {
            return new GemBranchData
            {
                id = this.info.id,
                isOpened = this.state.isOpened,
                isReady = this.state.isReady,
                remainingTimeSec = this.state.remainingTimeSec
            };
        }
    }
}