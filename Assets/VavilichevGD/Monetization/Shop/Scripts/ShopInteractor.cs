using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization.Analytics;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
    public class ShopInteractor : Interactor {
        
        protected const string PRODUCTS_FOLDER_PATH = "Products";

        protected Dictionary<string, Product> productsMap;
        private PaymentHandler activePaymentHandler;


        public override bool onCreateInstantly { get; } = false;

        protected override void Initialize() {
            base.Initialize();
            
            InitProductsMap();
            Shop.Initialize(this);
        }

        private void InitProductsMap() {
            productsMap = new Dictionary<string, Product>();
            ShopRepository shopRepository = this.GetRepository<ShopRepository>();
            string[] stateJsons = shopRepository.stateJsons;
            ProductInfo[] allProductsInfo = GetAllProductsInfo();
            Logging.Log($"PRODUCT INTERACTOR: Loaded products info. Count = {allProductsInfo.Length}");

            foreach (ProductInfo productInfo in allProductsInfo) {
                bool productCreated = false;
                foreach (string stateJson in stateJsons) {
                    ProductState state = JsonUtility.FromJson<ProductState>(stateJson);
                    if (productInfo.GetId() == state.id) {
                        ProductState specialState = productInfo.CreateState(stateJson);
                        Product product = new Product(productInfo, specialState);
                        productsMap.Add(productInfo.GetId(), product);
                        productCreated = true;
                        break;
                    }
                }

                if (!productCreated) {
                    ProductState state = productInfo.CreateDefaultState();
                    Product product = new Product(productInfo, state);
                    productsMap.Add(product.id, product);
                }
            }
            Resources.UnloadUnusedAssets();
        }

        private ProductInfo[] GetAllProductsInfo() {
            return Resources.LoadAll<ProductInfo>(PRODUCTS_FOLDER_PATH);
        }
        
        
        public Product[] GetAllProducts() {
            return productsMap.Values.ToArray();
        }

        
        public Product[] GetAllRealPaymentProducts() {
            List<Product> productList = new List<Product>();
            foreach (KeyValuePair<string,Product> pair in productsMap) {
                if (pair.Value.info.isRealPayment)
                    productList.Add(pair.Value);
            }

            return productList.ToArray();
        }
        
        
        public Product GetProduct(string productId) {
            return productsMap[productId];
        }

        
        public Product[] GetProducts<T>() where T : ProductInfo {
            List<Product> productList = new List<Product>();
            foreach (KeyValuePair<string,Product> pair in productsMap) {
                if (pair.Value.info as T != null)
                    productList.Add(pair.Value);
            }
            return productList.ToArray();
        }
        
        
        public void SaveAllProducts() {
            List<ProductState> states  = new List<ProductState>();
            foreach (KeyValuePair<string,Product> pair in productsMap)
                states.Add(pair.Value.state);

            ShopRepository shopRepository = this.GetRepository<ShopRepository>();
            shopRepository.Save(states.ToArray());
            Logging.Log("PURCHASE INTERACTOR: All products saved");
        }

        
        public void Purchase(Product product, ProductPurchaseHandler callback = null) {
            this.activePaymentHandler = PaymentsHandlerFactory.CreatePaymentHandler(product);
           
            void OnResults(Product result_product, bool success) {
                Logging.Log($"PURCHASE INTERACTOR: Payment complete with success = {success}");
                if (success) {
                    ForcePurchase(product, callback);
                    this.LogSuccessurchasing(product);
                }
                else {
                    product.PurchaseFail();
                    callback?.Invoke(result_product, false);
                }

                this.activePaymentHandler = null;
            }
            
            this.activePaymentHandler.Pay(product, OnResults);
        }

        private void LogSuccessurchasing(Product product) {
            if (product.info.isRealPayment) {
#if UNITY_ANDROID    
                var realPaymentInteractor = this.GetInteractor<RealPaymentInteractor>();
                var realPaymentBehavior = realPaymentInteractor.GetBehavior<RealPaymentBehaviorUnity>();
                var unityProduct = realPaymentBehavior.GetProduct(product.id);
                var iapType = IAPAnalytics.GetTypeOfProduct(product);
                IAPAnalytics.LogPurchasingUnityIAP(unityProduct, iapType);
#endif
            }
        }

        public void ForcePurchase(Product product, ProductPurchaseHandler callback = null) {
            product.PurchaseSuccess();
            SaveAllProducts();
            callback?.Invoke(product, true);
        }
    }
}