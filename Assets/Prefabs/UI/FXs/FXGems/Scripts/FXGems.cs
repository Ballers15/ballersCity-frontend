using VavilichevGD.Architecture;
using VavilichevGD.Monetization;

namespace SinSity.UI {
    public sealed class FXGems : FXUIFlyingReward<int>, IBankStateWithoutNotification {

        private UIBank uiBank;

        protected override void Initialize() {
            base.Initialize();
            
            if (Game.isInitialized)
                this.LocalInitialize();
            else
                Game.OnGameInitialized += this.OnGameInitialized;
        }

        private void LocalInitialize() {
            var bankInteractor = Game.GetInteractor<BankInteractor>();
            this.uiBank = bankInteractor.uiBank;
        }


        protected override void ApplyReward() {
            this.uiBank.AddHardCurrency(this, rewardCurrent);
//            Bank.SendNotification_ReceivedHardCurrency(rewardCurrent, this);
        }

        #region EVEMTS

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= this.OnGameInitialized;
            this.LocalInitialize();
        }

        #endregion
    }
}