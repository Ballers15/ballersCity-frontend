using SinSity.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

public class FXCleanEnergy : FXUIFlyingReward<BigNumber>, IBankStateWithoutNotification {
    
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
        this.uiBank.AddSoftCurrency(this, rewardCurrent);
//        Bank.SendNotification_ReceivedSoftCurrency(rewardCurrent, this);
    }

    #region EVEMTS

    private void OnGameInitialized(Game game) {
        Game.OnGameInitialized -= this.OnGameInitialized;
        this.LocalInitialize();
    }

    #endregion
}
