using VavilichevGD.Architecture;

namespace VavilichevGD.Monetization
{
    public delegate void ProductPurchaseHandler(Product product, bool success);

    public delegate void ProductStateChangeHandler(Product product, ProductState newState);

    public class Product
    {
        #region Events

        public static event ProductPurchaseHandler OnPurchasedResults;
        public static event ProductStateChangeHandler OnProductStateChanged;

        #endregion

        protected const bool SUCCESS = true;
        protected const bool FAIL = false;

        public ProductInfo info { get; }
        public ProductState state { get; }

        public T GetInfo<T>() where T : ProductInfo
        {
            return (T) this.info;
        }
        
        public T GetState<T>() where T : ProductState
        {
            return (T) this.state;
        }

        public bool isPurchased => state.isPurchased;
        public bool isConsumable => info.isConsumable;
        public bool isViewed => state.isViewed;
        public string id => info.GetId();
        public float price => info.price;

        public Product(ProductInfo info, ProductState state)
        {
            this.info = info;
            this.state = state;
        }


        public void PurchaseSuccess()
        {
            Distribute();
            NotifyAboutPurchaseResults(SUCCESS);
            NotifyAboutProductStateChanged();
        }

        public void Distribute()
        {
            ProductHandler handler = info.CreateHandler(this);
            handler.DistributeProduct();
        }

        public virtual void Save()
        {
            ShopInteractor shopInteractor = Game.GetInteractor<ShopInteractor>();
            shopInteractor.SaveAllProducts();
        }

        private void NotifyAboutPurchaseResults(bool success)
        {
            OnPurchasedResults?.Invoke(this, success);
        }

        private void NotifyAboutProductStateChanged()
        {
            OnProductStateChanged?.Invoke(this, state);
        }


        public void PurchaseFail()
        {
            NotifyAboutPurchaseResults(FAIL);
        }


        public void MarkAsViewed()
        {
            state.MarkAsViewed();
            Save();
            NotifyAboutProductStateChanged();
        }


        public virtual void SetState(ProductState newState)
        {
            this.state.SetState(newState);
            NotifyAboutProductStateChanged();
        }


        public string GetPriceToString()
        {
            return info.GetPriceToString();
        }
    }
}