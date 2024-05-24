#nullable enable
namespace UniT.FbInstant
{
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Payments
        {
            #region Public

            public static Task<Result<Product[]>> GetCatalogAsync() => This.InvokeAsync(_getCatalog).Convert<Product[]>();

            public static Task<Result<Purchase[]>> GetPurchasesAsync() => This.InvokeAsync(_getPurchases).Convert<Purchase[]>();

            public static Task<Result<Purchase>> PurchaseAsync(Product product, string? developerPayload = null) => PurchaseAsync(product.ProductId, developerPayload);

            public static Task<Result<Purchase>> PurchaseAsync(string productId, string? developerPayload = null) => This.InvokeAsync((callbackObj, callbackMethod, callbackId) => _purchase(productId, developerPayload, callbackObj, callbackMethod, callbackId)).Convert<Purchase>();

            public static Task<Result> ConsumePurchaseAsync(Purchase purchase)
            {
                return purchase.IsConsumed switch
                {
                    null  => Task.FromResult(new Result("Purchase is not consumable")),
                    true  => Task.FromResult(new Result("Purchase is already consumed.")),
                    false => ConsumePurchaseAsync(),
                };

                async Task<Result> ConsumePurchaseAsync()
                {
                    var result                                = await This.InvokeAsync(purchase.PurchaseToken, _consumePurchase);
                    if (result.IsSuccess) purchase.IsConsumed = true;
                    return result;
                }
            }

            #endregion

            #region DllImport

            [DllImport("__Internal")]
            private static extern void _getCatalog(string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _getPurchases(string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _purchase(string productId, string? developerPayload, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _consumePurchase(string purchaseToken, string callbackObj, string callbackMethod, string callbackId);

            #endregion
        }
    }
}