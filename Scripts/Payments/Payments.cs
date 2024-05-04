namespace UniT.FbInstant
{
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Payments
        {
            #region Public

            public static UniTask<Result<Product[]>> GetCatalog() => This.Invoke(_getCatalog).Convert<Product[]>();

            public static UniTask<Result<Purchase[]>> GetPurchases() => This.Invoke(_getPurchases).Convert<Purchase[]>();

            public static UniTask<Result<Purchase>> Purchase(Product product, string developerPayload = null) => Purchase(product.ProductId, developerPayload);

            public static UniTask<Result<Purchase>> Purchase(string productId, string developerPayload = null) => This.Invoke((callbackObj, callbackMethod, callbackId) => _purchase(productId, developerPayload, callbackObj, callbackMethod, callbackId)).Convert<Purchase>();

            public static UniTask<Result> ConsumePurchase(Purchase purchase)
            {
                return purchase.IsConsumed switch
                {
                    null => UniTask.FromResult(new Result("Purchase is not consumable")),
                    true => UniTask.FromResult(new Result("Purchase is already consumed.")),
                    false => This.Invoke(purchase.PurchaseToken, _consumePurchase)
                        .ContinueWith(result =>
                        {
                            if (result.IsSuccess) purchase.IsConsumed = true;
                            return (Result)result;
                        }),
                };
            }

            #endregion

            #region DllImport

            [DllImport("__Internal")]
            private static extern void _getCatalog(string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _getPurchases(string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _purchase(string productId, string developerPayload, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _consumePurchase(string purchaseToken, string callbackObj, string callbackMethod, string callbackId);

            #endregion
        }
    }
}