using SinSity.Repo;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    public sealed class ResearchObjectState
    {
        public bool isEnabled { get; set; }
        
        public bool isRewardReady { get; set; }

        public int remainingTimeSec { get; set; }

        public BigNumber price { get; set; }
        
        public ResearchObjectState(ResearchData researchData)
        {
            this.isEnabled = researchData.isEnabled;
            this.remainingTimeSec = researchData.remainingTimeSec;
            this.isRewardReady = researchData.isRewardReady;
            this.price = new BigNumber(researchData.priceBigNumberStr);
        }
    }
}