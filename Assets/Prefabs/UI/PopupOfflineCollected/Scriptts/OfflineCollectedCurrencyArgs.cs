using VavilichevGD.Tools;

namespace SinSity.UI
{
    public struct OfflineCollectedCurrencyArgs
    {
        public BigNumber offlineCollectedCurrency { get; set; }
        
        public int offlineSeconds { get; set; }
        
        public int x3ForGemsPrice { get; set; }
    }
}