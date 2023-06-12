using System.Collections.Generic;
using SinSity.Domain;
using SinSity.Services;
//using Mycom.Tracker.Unity;
using UnityEngine;
using UnityEngine.Accessibility;
using VavilichevGD.Architecture;

namespace VavilichevGD.Monetization.Analytics {
    public static class IAPAnalytics {
        
        [System.Serializable]
        public struct Receipt
        {
            public string Store;
            public string TransactionID;
            public string Payload;
        }
        
        [System.Serializable]
        public struct PayloadAndroid
        {
            public string Json;
            public string Signature;
        }
        
        public enum IAPType {
            hard,
            soft,
            no_ads
        }

        #region CONSTANTS

        private const string PAR_PROFILE_LEVEL = "account_level";

        #endregion
        
        private static ProfileLevelInteractor profileLevelInteractor { get; set; }
        private static bool isInitialized => profileLevelInteractor != null;
        
        private static void InitializeIfNeeded() {
            if (isInitialized)
                return;

            profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }
        

       /* public static void LogPurchasingUnityIAP(UnityEngine.Purchasing.Product product, IAPType iapType)
        {
            LogAppmetricaPurchaseEvent(product);
            //MyTracker.TrackPurchaseEvent(product);
            LogPurchasungUnityIAPAzur(product, iapType);
        }

        private static void LogPurchasungUnityIAPAzur(UnityEngine.Purchasing.Product product, IAPType iapType) {
            InitializeIfNeeded();
            
            var eventName = "payment_succeed";
            var parIAPID = "inapp_id";
            var parCurrency = "currency";
            var parPrice = "price";
            var parIAPTYPE = "inapp_type";

            var parameters = new Dictionary<string, object> {
                {
                    parIAPID, product.definition.id
                }, {
                    parCurrency, product.metadata.isoCurrencyCode
                }, {
                    parPrice, (float) product.metadata.localizedPrice
                }, {
                    parIAPTYPE, iapType.ToString()
                }, {
                    PAR_PROFILE_LEVEL, profileLevelInteractor.currentLevel 
                }
            };
            
            var m_event = new AnalyticsEvent(eventName, parameters);
            CommonAnalytics.Log(m_event);
        }

        private static void LogAppmetricaPurchaseEvent(UnityEngine.Purchasing.Product product)
        {
            string currency = product.metadata.isoCurrencyCode;
            decimal price = product.metadata.localizedPrice;

            // Creating the instance of the YandexAppMetricaRevenue class.
            YandexAppMetricaRevenue revenue = new YandexAppMetricaRevenue((double) price, currency);

            if (product.receipt != null)
            {
                // Creating the instance of the YandexAppMetricaReceipt class.
                YandexAppMetricaReceipt yaReceipt = new YandexAppMetricaReceipt();
                Receipt receipt = JsonUtility.FromJson<Receipt>(product.receipt);
#if UNITY_ANDROID
                PayloadAndroid payloadAndroid = JsonUtility.FromJson<PayloadAndroid>(receipt.Payload);
                yaReceipt.Signature = payloadAndroid.Signature;
                yaReceipt.Data = payloadAndroid.Json;
#elif UNITY_IPHONE
                    yaReceipt.TransactionID = receipt.TransactionID;
                    yaReceipt.Data = receipt.Payload;
#endif
                revenue.Receipt = yaReceipt;
            }

            // Sending data to the AppMetrica server.
            AppMetrica.Instance.ReportRevenue(revenue);
        }*/

        public static IAPType GetTypeOfProduct(Product product) {
            //TODO: if you have another types. Just define it here by product info is PRODUCT_TYPE_INFO
            var type = IAPType.hard;
            return type;
        }
    }
}