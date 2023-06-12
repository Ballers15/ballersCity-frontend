using System;
using System.Collections;
using SinSity.Services;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public delegate void BankStateChangeHandler(object sender);
    
    public class BankInteractor : Interactor {

        public event BankStateChangeHandler OnBankStateChanged;

        public int hardCurrencyCount => bankRepository.hardCurrencyCount;
        public BigNumber softCurrencyCount => bankRepository.softCurrencyCount;
        public float cryptoCurrencyCount => bankRepository.cryptoCurrencyCount;
        public int globalIncomeMultiplicatorDynamic => bankRepository.globalIncomeMultiplicatorDynamic;
        public int globalIncomeMultiplicatorPermanent => bankRepository.globalIncomeMultiplicatorPermanent;
        
        public UIBank uiBank { get; private set; }
        
        protected BankRepository bankRepository;

        public override bool onCreateInstantly { get; } = true;

        protected override void Initialize() {
            base.Initialize();
            this.bankRepository = this.GetRepository<BankRepository>();
            Bank.Initialize(this);
            this.uiBank = new UIBank(this.bankRepository.softCurrencyCount, this.bankRepository.hardCurrencyCount, this.bankRepository.cryptoCurrencyCount);
        }

        
        public bool IsEnoughtMoney(Product product) {
            return this.uiBank.IsEnoughtMoney(product);
        }
        
        public bool IsEnoughtHardCurrency(float count) {
            return this.uiBank.IsEnoughtHardCurrency(count);
        }

        public bool IsEnoughtSoftCurrency(BigNumber count) {
            return this.uiBank.IsEnoughtSoftCurrency(count);
        }
        
        public void AddSoftCurrency(BigNumber count, object sender) {
            bankRepository.AddSoftCurrency(count);
            NotifyAboutStateChanged();
        }

        public void SpendSoftCurrency(BigNumber count, object sender) {
            this.uiBank.SpendSoftCurrency(sender, count);
            bankRepository.SpendSoftCurrency(count);
            NotifyAboutStateChanged();
        }

        public void AddHardCurrency(int count, object sender) {
            bankRepository.AddHardCurrency(count);
            NotifyAboutStateChanged();
            CommonAnalytics.LogGemsReceived(count, this.bankRepository.hardCurrencyCount);
        }

        public void SpendHardCurrency(int count, object sender) {
            this.uiBank.SpendHardCurrency(sender, count);
            bankRepository.SpendHardCurrency(count);
            NotifyAboutStateChanged();
            CommonAnalytics.LogGemsSpent(count, this.bankRepository.hardCurrencyCount);
        }

        public void AddCryptoCurrency(float count, object sender) {
            bankRepository.AddCryptoCurrency(count);
            NotifyAboutStateChanged();
        }
        
        public void SpendCryptoCurrency(float count, object sender) {
            uiBank.SpendCryptoCurrency(sender, count);
            bankRepository.SpendCryptoCurrency(count);
            NotifyAboutStateChanged();
        }

        public void AddGlobalIncomeMultiplicatorDynamic(int count) {
            bankRepository.AddGlobalIncomeMultiplicatorDynamic(count);
            NotifyAboutStateChanged();
        }

        public void ResetGlobalIncomeMultiplicatorDynamic() {
            bankRepository.ResetGlobalIncomeMultiplicatorDynamic();
            NotifyAboutStateChanged();
        }

        public void AddGlobalIncomeMultiplicatorPermanent(int count) {
            bankRepository.AddGlobalIncomeMultiplicatorPermanent(count);
            NotifyAboutStateChanged();
        }

        protected void NotifyAboutStateChanged(object sender = null) {
            OnBankStateChanged?.Invoke(sender);
        }
    }
}