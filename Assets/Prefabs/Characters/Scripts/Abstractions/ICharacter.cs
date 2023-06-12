using System;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    public interface ICharacter {
        event Action<ICharacter> OnCharacterActiveStateChanged;
        
        string id { get; }
        ICard characterCard { get; }
        IIdleObject idleObject { get; }
        ICharacterInfo info { get; }
        bool isActive { get; }

        void ApplyIncomeMultiplierToIdleObject();
        void RemoveIncomeMultiplierFromIdleObject();
        RewardInfo GetRandomDailyReward();
    }
}