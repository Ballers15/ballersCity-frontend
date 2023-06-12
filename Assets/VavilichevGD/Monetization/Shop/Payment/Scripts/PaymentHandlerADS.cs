using SinSity.Services;
using SinSity.UI;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

namespace VavilichevGD.Monetization {
    public class PaymentHandlerADS : PaymentHandler {
        public override void Pay(Product product, PaymentResultHandler callback) {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            UIPopupADLoading popupAdLoading = uiInteractor.ShowElement<UIPopupADLoading>();

            void OnADResults(UIPopupADLoading popup, bool success, string error = "") {
                popupAdLoading.OnADResultsReceived -= OnADResults;
                callback?.Invoke(product, success);
            }

            popupAdLoading.OnADResultsReceived += OnADResults;
            popupAdLoading.ShowAD($"pay_for_{product.id}");
        }
    }
}