using IdleClicker.Gameplay;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    [CreateAssetMenu(
        fileName = "CoffeeBoostLevelHandler",
        menuName = "Domain/CoffeeBoost/New CoffeeBoostLevelHandler"
    )]
    public class CoffeeBoostLevelHandler : ScriptableProfileLevelHandler {
        #region OnProfileLevelRisen

        public override void OnProfileLevelRisen()
        {
            var coffeeBoostInteractor = Game.GetInteractor<CoffeeBoostInteractor>();
            if (coffeeBoostInteractor.isCoffeeBoostUnlocked)
                return;
#if DEBUG
            Debug.Log("<color=green>UNLOCK COFFEE BOOST</color>");
#endif
            coffeeBoostInteractor.UnlockCoffeeBoost(this);
            this.ReceiveReward();
        }

        #endregion
    }
}