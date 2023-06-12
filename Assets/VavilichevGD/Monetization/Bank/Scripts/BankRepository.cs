using System.Collections;
using System.Numerics;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class BankRepository : Repository {

        public string strSoftCurrencyCount { get; protected set; }
        public int hardCurrencyCount { get; protected set; }
        public float cryptoCurrencyCount { get; protected set; }
        public int globalIncomeMultiplicatorDynamic { get; protected set; }
        public int globalIncomeMultiplicatorPermanent { get; protected set; }

        public BigNumber softCurrencyCount {
            get => new BigNumber(strSoftCurrencyCount);
            set => strSoftCurrencyCount = value.ToString(BigNumber.FORMAT_FULL);
        }

        protected const string PREF_KEY_CURRENCY_DATA = "BANK_REPOSITORY_DATA";

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY_CURRENCY_DATA);
            //using var cryptoAmountRequest = new CryptoCurrencyRequest();
            
            yield return request.Send(RequestType.GET);
            //yield return cryptoAmountRequest.Send();
            
            var data = request.GetGameData(new BankCurrencyData());
            strSoftCurrencyCount = data.strSoftCurrencyCount;
            hardCurrencyCount = data.hardCurrencyCount;
            //cryptoCurrencyCount = cryptoAmountRequest.GetAmount();
            cryptoCurrencyCount = 0;
            globalIncomeMultiplicatorDynamic = data.globalIncomeMultiplicatorDynamic;
            globalIncomeMultiplicatorPermanent = data.globalIncomeMultiplicatorPermanent;
        }

        public override void Save() {
            base.Save();
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }

        private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            var data = new BankCurrencyData {
                strSoftCurrencyCount = strSoftCurrencyCount,
                hardCurrencyCount = hardCurrencyCount,
                cryptoCurrencyCount = cryptoCurrencyCount,
                globalIncomeMultiplicatorDynamic = globalIncomeMultiplicatorDynamic,
                globalIncomeMultiplicatorPermanent = globalIncomeMultiplicatorPermanent
            };
            using var request = new GameDataRequest(PREF_KEY_CURRENCY_DATA, data);
            
            yield return request.Send();

            isSavingInProcess = false;
        }
        
        public void AddSoftCurrency(BigNumber count) {
            softCurrencyCount += count;
            Save();
        }

        public void SpendSoftCurrency(BigNumber count) {
            softCurrencyCount -= count;
            Save();
        }

        public void AddHardCurrency(int count) {
            hardCurrencyCount += count;
            Save();
        }

        public void SpendHardCurrency(int count) {
            hardCurrencyCount = Mathf.Max(hardCurrencyCount - count, 0);
            Save();
        }

        public void AddCryptoCurrency(float amount) {
            cryptoCurrencyCount += amount;
            Save();
            //Coroutines.StartRoutine(SaveCryptoCurrencyAmount());
        }

        /*private IEnumerator SaveCryptoCurrencyAmount() {
            using var cryptoCurrencyRequest = new CryptoCurrencyRequest(cryptoCurrencyCount);
            yield return cryptoCurrencyRequest.Send();
        }*/

        public void SpendCryptoCurrency(float amount) {
            cryptoCurrencyCount = Mathf.Max(cryptoCurrencyCount - amount, 0);
            Save();
            //Coroutines.StartRoutine(SaveCryptoCurrencyAmount());
        }
        
        public void AddGlobalIncomeMultiplicatorDynamic(int count) {
            globalIncomeMultiplicatorDynamic += count;
            Save();
        }

        public void ResetGlobalIncomeMultiplicatorDynamic() {
            globalIncomeMultiplicatorDynamic = 1;
            Save();
        }

        public void AddGlobalIncomeMultiplicatorPermanent(int count) {
            globalIncomeMultiplicatorPermanent += count;
            Save();
        }
    }
}