using System.Collections.Generic;
using SinSity.Domain;
using VavilichevGD.Architecture;

namespace SinSity.Core
{
    public interface IIdleObjectExternalMultipliersHolderInteractor : IInteractor
    {
        Dictionary<string, Coefficient> GetStaticMultiplierMapBy(IdleObject idleObject);
        
        Dictionary<string, TimeCoefficient> GetTimeMultiplierMapBy(IdleObject idleObject);
    }
}