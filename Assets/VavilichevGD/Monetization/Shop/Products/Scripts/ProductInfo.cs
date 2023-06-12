using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Monetization.Analytics;

namespace VavilichevGD.Monetization {
    public abstract class ProductInfo : ScriptableObject {
        
        [SerializeField] protected string id;
        [SerializeField] protected string titleCode;
        [SerializeField] protected string descriptionCode;
        [SerializeField] protected Sprite spriteIcon;
        [SerializeField] protected PaymentType m_paymentType;
        [SerializeField] protected float m_price;
        [SerializeField] protected bool m_isConsumable = true;

        public PaymentType paymentType => m_paymentType;
        public bool isConsumable => m_isConsumable;
        public bool isRealPayment => m_paymentType == PaymentType.Real;
        public bool isADSPayment => m_paymentType == PaymentType.ADS;
        public float price => m_price;

        public virtual string GetTitle() {
            return titleCode;
        }

        public virtual string GetDesctiption() {
            return descriptionCode;
        }

        public virtual Sprite GetSpriteIcon() {
            return spriteIcon;
        }

        public virtual string GetPriceToString() {
            if (!isRealPayment && !isADSPayment)
                return m_price.ToString();
            
            if (isRealPayment) {
                RealPaymentInteractor paymentInteractor = Game.GetInteractor<RealPaymentInteractor>();
                //return paymentInteractor.GetPriceOfProduct(this);
                return "";
            }

            return "0";
        }

        public string GetId() {
            return id;
        }

        public abstract ProductHandler CreateHandler(Product product);
        public abstract ProductState CreateState(string stateJson);
        public abstract ProductState CreateDefaultState();
    }
}