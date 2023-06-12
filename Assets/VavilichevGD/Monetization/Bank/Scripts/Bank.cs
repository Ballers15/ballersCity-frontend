using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public static class Bank {

        public static bool isInitialized => interactor != null;

        private static BankInteractor interactor;

        public static UIBank uiBank => interactor.uiBank;

        public static int hardCurrencyCount => interactor.hardCurrencyCount;
        public static float cryptoCurrencyCount => interactor.cryptoCurrencyCount;
        public static BigNumber softCurrencyCount => interactor.softCurrencyCount;
        public static int globalIncomeMultiplicatorDynamic => interactor.globalIncomeMultiplicatorDynamic;
        public static int globalIncomeMultiplicatorPermanent => interactor.globalIncomeMultiplicatorPermanent;

        #region Delegates
//        public static event BankStateChangeHandler OnBankStateChanged;

        public delegate void HardCurrencyReceiveHandler(int receivedHardCurrency, object sender);
        public static event HardCurrencyReceiveHandler OnHardCurrencyReceived;
        public static event HardCurrencyReceiveHandler OnHardCurrencyReceivedInstantlyEvent;

        public delegate void HardCurrencySpendHandler(int spentHardCurrency, object sender);
        public static event HardCurrencySpendHandler OnHardCurrencySpent;
        public static event HardCurrencySpendHandler OnHardCurrencySpentInstantlyEvent;

        public delegate void SoftCurrencyReceiveHandler(BigNumber receivedSoftCurrency, object sender);
        public static event SoftCurrencyReceiveHandler OnSoftCurrencyReceived;
        public static event SoftCurrencyReceiveHandler OnSoftCurrencyReceivedInstantlyEvent;

        public delegate void SoftCurrencySpendHandler(BigNumber spentSoftCurrency, object sender);
        public static event SoftCurrencySpendHandler OnSoftCurrencySpent;
        public static event SoftCurrencySpendHandler OnSoftCurrencySpentInstantlyEvent;
        
        public delegate void CryptoCurrencyReceiveHandler(float receivedCryptoCurrency, object sender);
        public static event CryptoCurrencyReceiveHandler OnCryptoCurrencyReceived;
        public static event CryptoCurrencyReceiveHandler OnCryptoCurrencyReceivedInstantlyEvent;

        public delegate void CryptoCurrencySpendHandler(float spentCryptoCurrency, object sender);
        public static event CryptoCurrencySpendHandler OnCryptoCurrencySpent;
        public static event CryptoCurrencySpendHandler OnCryptoCurrencySpentInstantlyEvent;

        #endregion

        public static void Initialize(BankInteractor _interactor) {
            interactor = _interactor;
            
            BankAnalytics.Initialize();
            Logging.Log($"BANK: Initialized.");
        }

        public static void AddSoftCurrency(BigNumber count, object sender) {
            interactor.AddSoftCurrency(count, sender);
            if (!(sender is IBankStateWithoutNotification))
                SendNotification_ReceivedSoftCurrency(count, sender);
            OnSoftCurrencyReceivedInstantlyEvent?.Invoke(count, sender);
        }

        public static void SpendSoftCurrency(BigNumber count, object sender) {
            SendNotification_SpentSoftCurrency(count, sender);
            interactor.SpendSoftCurrency(count, sender);
            OnSoftCurrencySpentInstantlyEvent?.Invoke(count, sender);
        }

        public static void AddHardCurrency(int count, object sender) {
            interactor.AddHardCurrency(count, sender);
            if (!(sender is IBankStateWithoutNotification))
                SendNotification_ReceivedHardCurrency(count, sender);
            OnHardCurrencyReceivedInstantlyEvent?.Invoke(count, sender);
        }

        public static void SpendHardCurrency(int count, object sender) {
            SendNotification_SpentHardCurrency(count, sender);
            interactor.SpendHardCurrency(count, sender);
            OnHardCurrencySpentInstantlyEvent?.Invoke(count, sender);
        }
        
        public static void AddCryptoCurrency(float count, object sender) {
            interactor.AddCryptoCurrency(count, sender);
            if (!(sender is IBankStateWithoutNotification))
                SendNotification_ReceivedCryptoCurrency(count, sender);
            OnCryptoCurrencyReceivedInstantlyEvent?.Invoke(count, sender);
        }

        public static void SpendCryptoCurrency(float count, object sender) {
            SendNotification_SpentCryptoCurrency(count, sender);
            interactor.SpendCryptoCurrency(count, sender);
            OnCryptoCurrencySpentInstantlyEvent?.Invoke(count, sender);
        }

        public static bool isEnoughtMoney(Product product) {
            return interactor.IsEnoughtMoney(product);
        }

        public static bool isEnoughtHardCurrency(int count) {
            return interactor.IsEnoughtHardCurrency(count);
        }

        public static bool isEnoughtSoftCurrency(BigNumber count) {
            return interactor.IsEnoughtSoftCurrency(count);
        }

        public static void AddGlobalMulltiplicatorDynamic(int count) {
            interactor.AddGlobalIncomeMultiplicatorDynamic(count);
        }

        public static void ResetGlobalMultiplicatorDynamic() {
            interactor.ResetGlobalIncomeMultiplicatorDynamic();
        }

        public static void AddGlobalMultiplicatorPermanent(int count) {
            interactor.AddGlobalIncomeMultiplicatorPermanent(count);
        }

        public static void SendNotification_ReceivedSoftCurrency(BigNumber value, object sender) {
            OnSoftCurrencyReceived?.Invoke(value, sender);
        }

        public static void SendNotification_SpentSoftCurrency(BigNumber value, object sender) {
            OnSoftCurrencySpent?.Invoke(value, sender);
        }
        
        public static void SendNotification_ReceivedCryptoCurrency(float value, object sender) {
            OnCryptoCurrencyReceived?.Invoke(value, sender);
        }

        public static void SendNotification_SpentCryptoCurrency(float value, object sender) {
            OnCryptoCurrencySpent?.Invoke(value, sender);
        }

        public static void SendNotification_ReceivedHardCurrency(int value, object sender) {
            OnHardCurrencyReceived?.Invoke(value, sender);
        }

        public static void SendNotification_SpentHardCurrency(int value, object sender) {
            OnHardCurrencySpent?.Invoke(value, sender);
        }
    }
}