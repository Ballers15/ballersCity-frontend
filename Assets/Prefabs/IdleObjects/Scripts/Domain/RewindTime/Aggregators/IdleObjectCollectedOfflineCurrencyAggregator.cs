using System.Collections.Generic;
using Orego.Util;
using SinSity.Core;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    public sealed class IdleObjectCollectedOfflineCurrencyAggregator
    {
        private readonly Dictionary<IdleObject, BigNumber> idleObjectVsCurrencyMap;

        public IdleObjectCollectedOfflineCurrencyAggregator()
        {
            this.idleObjectVsCurrencyMap = new Dictionary<IdleObject, BigNumber>();
        }

        public void Aggregate(IdleObject idleObject, BigNumber algorithmResult)
        {
            this.idleObjectVsCurrencyMap[idleObject] = algorithmResult;
        }

        public Dictionary<IdleObject, BigNumber> GetAggregatedOfflineCurrencyMap()
        {
            return this.idleObjectVsCurrencyMap.Clone();
        }

        public BigNumber GetAllCollectedOfflineCurrency()
        {
            var allCollectedOfflineCurrency = new BigNumber(0);
            var collectedCurrencies = this.idleObjectVsCurrencyMap.Values;
            foreach (var collectedCurrency in collectedCurrencies)
            {
                allCollectedOfflineCurrency += collectedCurrency;
            }

            return allCollectedOfflineCurrency;
        }

        public void Reset()
        {
            this.idleObjectVsCurrencyMap.Clear();
        }
    }
}