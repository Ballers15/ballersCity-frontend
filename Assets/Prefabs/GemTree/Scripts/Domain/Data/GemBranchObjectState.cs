using SinSity.Repo;

namespace SinSity.Domain
{
    public sealed class GemBranchObjectState
    {
        public bool isOpened { get; set; }
        
        public bool isReady { get; set; }
        
        public int remainingTimeSec { get; set; }
        
        public GemBranchObjectState(GemBranchData branchData)
        {
            this.isOpened = branchData.isOpened;
            this.isReady = branchData.isReady;
            this.remainingTimeSec = branchData.remainingTimeSec;
        }
    }
}