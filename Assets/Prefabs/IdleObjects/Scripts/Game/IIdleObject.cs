using VavilichevGD.Tools;

namespace SinSity.Core {
    public interface IIdleObject {
        string id { get; }
        bool isBuilt { get; }
        bool isBuildedAnyTime { get; }
        BigNumber incomeCurrent { get; }
        IdleObjectState state { get; }
        IdleObjectInfo info { get; }

        void AddIncomeMultiplier(string id, Coefficient multiplier);
        void RemoveIncomeMultiplier(string id);
    }
}