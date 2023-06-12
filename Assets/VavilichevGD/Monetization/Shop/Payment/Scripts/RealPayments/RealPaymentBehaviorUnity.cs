using SinSity.Services;
using UnityEngine;
//using UnityEngine.Purchasing;
//using UnityEngine.Purchasing.Security;
using VavilichevGD.Monetization.Analytics;
using VavilichevGD.Tools;

namespace VavilichevGD.Monetization {
   /* public class RealPaymentBehaviorUnity : RealPaymentBehavior, IStoreListener {

	    #region CONSTANTS

	    private const string GPSECRET =
		    "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtuPcuUhaZDds9FUN0zr6/onYuyuUF+Janr9yaAfetGQqqQUL290mInBUdyPiGPmH4Y7Cxat+OxNmQ2INOdEh0AebHuxGc2fgP2bqdPhG5TIgYFQpggoyTa/zz+eCmLPmd3jDry4Qt1OWc9EWNdw5FOCLeUwKbp1/Ifj+w/47DMhyFB1fuAIcnvrStmzK9uvWqnmpKFOoadOy9vo7gw+Cb+i7l5NQTLntS7vKCb/9epCl6bogRAh+mYI7t9C04L/8B9hycZw9kCrsjwLU4b0XubC4gKNdRQ1J4kjHxyLiazMHqIwFkzvTg2AeD4Dr5pzrGcN8P+3x4toKvmbrP1LZfQIDAQAB";

	    private const string ASSECRET = "LOL";

	    #endregion
        
		private bool isInitialized => m_StoreController != null && m_StoreExtensionProvider != null;
		
		private IStoreController m_StoreController;
		private IExtensionProvider m_StoreExtensionProvider;
		private PaymentHandler.PaymentResultHandler callback;
		private Product product;
		private Product[] products;

		public RealPaymentBehaviorUnity(Product[] _products) : base(_products) {
			Initialize(_products);
		}
		
		private void Initialize(Product[] _products) {
			if (isInitialized)
				return;
			
			ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			InitProducts(builder, _products);
			UnityPurchasing.Initialize(this, builder);
		}

		private void InitProducts(ConfigurationBuilder builder, Product[] _products) {
			this.products = _products;
			foreach (Product product in _products) {
				if (product.isConsumable)
					builder.AddProduct(product.id, ProductType.Consumable);
				else
					builder.AddProduct(product.id, ProductType.NonConsumable);
			}
		}

		
		public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
			m_StoreController = controller;
			m_StoreExtensionProvider = extensions;

			foreach (var p in m_StoreController.products.all)
				m_StoreController.ConfirmPendingPurchase(p);
			
			Logging.Log("RealPaymentBehaviorUnity: Initialized");
		}
		
		public void OnInitializeFailed(InitializationFailureReason error) {
			Logging.Log("RealPaymentBehaviorUnity: OnInitializeFailed (" + error + ")");
		}

		
		public override bool IsPurchasedProduct(Product _product) {
			if (_product.isConsumable)
				return false;
			
			var unityPurchasingProduct = m_StoreController.products.WithID(_product.id);
			if (unityPurchasingProduct != null)
				return unityPurchasingProduct.hasReceipt;
			return false;
		}


		public override void PurchaseProduct(Product _product, PaymentHandler.PaymentResultHandler _callback) {
			if (callback != null) {
				Logging.LogError($"RealPaymentBehaviorUnity: Cannot start payment ({_product.id}) while another one wasnt ended");
				_callback?.Invoke(_product, false);
				return;
			}

			if (!this.isInitialized) {
				Logging.LogError($"RealPaymentBehaviorUnity: Cannot start payment ({_product.id}) because IAP is not initialized!");
				this.TryToReinitialize();
				_callback?.Invoke(_product, false);
				return;
			}
			
			UnityEngine.Purchasing.Product unityPurchasingProduct = m_StoreController.products.WithID(_product.id);
			if (!IsValid(_product, unityPurchasingProduct)) {
				Logging.LogError($"RealPaymentBehaviorUnity: Cannot pay for {_product.id}, NOT VALID");
				_callback?.Invoke(_product, false);
				return;
			}

			Logging.Log($"RealPaymentBehaviorUnity: Try to purchase '{unityPurchasingProduct.definition.id}'");
			callback = _callback;
			product = _product;
			m_StoreController.InitiatePurchase(unityPurchasingProduct);
		}

		private void TryToReinitialize() {
			this.Initialize(this.products);
		}

		private bool IsValid(Product _product, UnityEngine.Purchasing.Product unityPurchasingProduct) {
			return unityPurchasingProduct != null && unityPurchasingProduct.availableToPurchase && !IsPurchasedProduct(_product);
		}
		
		
		public void OnPurchaseFailed(UnityEngine.Purchasing.Product i, PurchaseFailureReason p) {
			Logging.LogError($"RealPaymentBehaviorUnity: Payment failed. Product: {i.definition.id}, reason: {p}");
			callback?.Invoke(product, false);
			ClearData();
		}

		private void ClearData() {
			product = null;
			callback = null;
		}

		
		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {

			bool validPurchase = this.IsPurchaseValid(args);
			
			callback?.Invoke(product, validPurchase);
			
			if (validPurchase) {
				CommonAnalytics.LogPurchase(args);
				Logging.Log($"RealPaymentBehaviorUnity: SUCCESS. Product:'{args.purchasedProduct.definition.id}");
			}
			else {
				Logging.Log($"RealPaymentBehaviorUnity: FAIL. Product:'{args.purchasedProduct.definition.id}");
			}
			
			ClearData();
			return PurchaseProcessingResult.Complete;
		}

		private bool IsPurchaseValid(PurchaseEventArgs args) {
			// Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
			// Prepare the validator with the secrets we prepared in the Editor
			// obfuscation window.
			var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
				AppleTangle.Data(), Application.identifier);

			try {
				// On Google Play, result has a single product ID.
				// On Apple stores, receipts contain multiple products.
				var result = validator.Validate(args.purchasedProduct.receipt);
				// For informational purposes, we list the receipt(s)
				Debug.Log("Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
					Debug.Log(productReceipt.productID);
					Debug.Log(productReceipt.purchaseDate);
					Debug.Log(productReceipt.transactionID);
				}

				return true;
			} catch (IAPSecurityException) {
				Debug.Log("Invalid receipt, not unlocking content");
				return false;
			}
#endif
			return true;
		}
		

		public override string GetPriceOfProduct(Product _product) {
			return GetPriceOfProduct(_product.info);
		}

		public override string GetPriceOfProduct(ProductInfo info) {
			if (!isInitialized) {
				Logging.Log($"RealPaymentBehaviorUnity: Not initialized");
				return $"${info.price}";
			}
			
			UnityEngine.Purchasing.Product unityPurchasingProduct = m_StoreController.products.WithID(info.GetId());
			return unityPurchasingProduct.metadata.localizedPriceString;
		}

		public UnityEngine.Purchasing.Product GetProduct(string iapId) {
			UnityEngine.Purchasing.Product unityPurchasingProduct = m_StoreController.products.WithID(iapId);
			return unityPurchasingProduct;
		}
    }*/
}