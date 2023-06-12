using System;
using SinSity.Repo;
using VavilichevGD.Architecture;

namespace IdleClicker.Gameplay {
    public class CoffeeBoostInteractor : Interactor {

        #region DELEGATES

        public event Action<object> OnCoffeeBoostUnlockedEvent;

        #endregion


        private CoffeeBoostRepository coffeeBoostRepository;
        public CoffeeBoost coffeeBoost { get; private set; }
        public bool isCoffeeBoostUnlocked { get; private set; }


        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.coffeeBoost = new CoffeeBoost();
            this.coffeeBoostRepository = this.GetRepository<CoffeeBoostRepository>();
            this.isCoffeeBoostUnlocked = this.coffeeBoostRepository.GetIsUnlocked();
        }


        public void UnlockCoffeeBoost(object sender) {
            this.isCoffeeBoostUnlocked = true;
            this.coffeeBoostRepository.SetInUnlocked(true);
            this.OnCoffeeBoostUnlockedEvent?.Invoke(sender);
        }

    }
}