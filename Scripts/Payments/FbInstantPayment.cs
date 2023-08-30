namespace FbInstant.Payments
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class FbInstantPayment
    {
        #region Public

        public UniTask<Result<Product[]>> GetCatalog() => this._fbInstant.Invoke(_getCatalog).Convert<Product[]>();

        public UniTask<Result<Purchase[]>> GetPurchases() => this._fbInstant.Invoke(_getPurchases).Convert<Purchase[]>();

        public UniTask<Result<Purchase>> Purchase(Product product, Dictionary<string, object> payload = null) => this.Purchase(product.ProductId, payload);

        public UniTask<Result<Purchase>> Purchase(string productId, Dictionary<string, object> payload = null) => this._fbInstant.Invoke((callbackObj, callbackMethod, callbackId) => _purchase(productId, JsonConvert.SerializeObject(payload), callbackObj, callbackMethod, callbackId)).Convert<Purchase>();

        public UniTask<Result> ConsumePurchase(Purchase purchase)
        {
            if (purchase.IsConsumed) return UniTask.FromResult(new Result("Purchase is already consumed."));
            return this._fbInstant.Invoke(purchase.PurchaseToken, _consumePurchase)
                .ContinueWith(result =>
                {
                    if (result.IsSuccess) purchase.IsConsumed = true;
                    return (Result)result;
                });
        }

        #endregion

        #region Constructor

        private readonly FbInstant _fbInstant;

        [Preserve]
        public FbInstantPayment()
        {
            this._fbInstant = Object.FindObjectOfType<FbInstant>() ?? FbInstant.Instantiate();
        }

        #endregion

        #region DllImport

        [DllImport("__Internal")]
        private static extern void _getCatalog(string callbackObj, string callbackMethod, string callbackId);

        [DllImport("__Internal")]
        private static extern void _getPurchases(string callbackObj, string callbackMethod, string callbackId);

        [DllImport("__Internal")]
        private static extern void _purchase(string productId, string payload, string callbackObj, string callbackMethod, string callbackId);

        [DllImport("__Internal")]
        private static extern void _consumePurchase(string purchaseToken, string callbackObj, string callbackMethod, string callbackId);

        #endregion
    }
}