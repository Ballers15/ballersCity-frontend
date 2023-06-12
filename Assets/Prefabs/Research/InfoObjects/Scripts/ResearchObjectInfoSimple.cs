using SinSity.Domain.Utils;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ResearchObjectInfoMini",
        menuName = "Game/Research/New ResearchObjectInfoMini"
    )]
    public sealed class ResearchObjectInfoSimple : ResearchObjectInfo
    {
        [SerializeField]
        public BigNumber minPrice = new BigNumber(1000);

        [SerializeField]
        public int priceByIncomePerSeconds = 30;

        public override BigNumber LoadNextPrice()
        {
            var interactor = Game.GetInteractor<IdleObjectsInteractor>();
            var incomePerSec = interactor.GetFullIncomeFromIdleObjects();
            var price = incomePerSec * this.priceByIncomePerSeconds;
            if (price < this.minPrice)
            {
                price = this.minPrice;
            }

            return price;
        }
    }
}