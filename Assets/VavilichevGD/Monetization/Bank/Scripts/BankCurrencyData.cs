using System;

namespace VavilichevGD.Monetization {
    [Serializable]
    public class BankCurrencyData {
        public string strSoftCurrencyCount;
        public int hardCurrencyCount;
        public float cryptoCurrencyCount;
        public int globalIncomeMultiplicatorDynamic;
        public int globalIncomeMultiplicatorPermanent;

        public BankCurrencyData() {
            this.strSoftCurrencyCount = "0";
            this.hardCurrencyCount = 0;
            this.cryptoCurrencyCount = 0f;
            this.globalIncomeMultiplicatorDynamic = 1;
            this.globalIncomeMultiplicatorPermanent = 1;
        }
    }
}