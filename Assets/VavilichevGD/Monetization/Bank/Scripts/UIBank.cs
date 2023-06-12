using UnityEngine;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class UIBank {

        #region DELEGATES

        public delegate void UIBankStateHandler(object sender);
        public event UIBankStateHandler OnStateChangedEvent;

        public delegate void UIBankHandlerHardCurrency(object sender, int value);
        public event UIBankHandlerHardCurrency OnHardCurrencyReceivedEvent;
        public event UIBankHandlerHardCurrency OnHardCurrencySpentEvent;
        
        public delegate void UIBankHandlerCryptoCurrency(object sender, float value);
        public event UIBankHandlerCryptoCurrency OnCryptoCurrencyReceivedEvent;
        public event UIBankHandlerCryptoCurrency OnCryptoCurrencySpentEvent;

        public delegate void UIBankHandlerSoftCurrency(object sender, BigNumber value);
        public event UIBankHandlerSoftCurrency OnSoftCurrencyReceivedEvent;
        public event UIBankHandlerSoftCurrency OnSoftCurrencySpentEvent;

        #endregion
        
        public BigNumber softCurrency;
        public int hardCurrency;
        public float cryptoCurrency;

        public UIBank(BigNumber softCurrency, int hardCurrency, float cryptoCurrency) {
            this.softCurrency = softCurrency;
            this.hardCurrency = hardCurrency;
            this.cryptoCurrency = cryptoCurrency;
        }
        
        public void AddSoftCurrency(object sender, BigNumber value) {
            this.softCurrency += value;
            this.OnSoftCurrencyReceivedEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }

        public void SpendSoftCurrency(object sender, BigNumber value) {
            this.softCurrency -= value;
            this.OnSoftCurrencySpentEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }

        public void AddHardCurrency(object sender, int value) {
            this.hardCurrency += value;
            this.OnHardCurrencyReceivedEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }

        public void SpendHardCurrency(object sender, int value) {
            this.hardCurrency -= value;
            this.OnHardCurrencySpentEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }
        
        public void AddCryptoCurrency(object sender, float value) {
            this.cryptoCurrency += value;
            this.OnCryptoCurrencyReceivedEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }

        public void SpendCryptoCurrency(object sender, float value) {
            this.cryptoCurrency -= value;
            this.OnCryptoCurrencySpentEvent?.Invoke(sender, value);
            this.OnStateChangedEvent?.Invoke(sender);
        }

        public bool IsEnoughtMoney(Product product) {
            var info = product.info;
            if (info.isRealPayment || info.isADSPayment)
                return true;

            if (info.paymentType == PaymentType.HardCurrency)
                return IsEnoughtHardCurrency(info.price);

            return false;
        }
        
        public bool IsEnoughtHardCurrency(float value) {
            return value <= this.hardCurrency;
        }

        public bool IsEnoughtSoftCurrency(BigNumber value) {
            return value <= this.softCurrency;
        }
    }
}