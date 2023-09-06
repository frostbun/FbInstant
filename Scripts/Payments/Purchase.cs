namespace FbInstant
{
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    public sealed class Purchase
    {
        public string PaymentId        { get; }
        public string ProductId        { get; }
        public string PurchaseToken    { get; }
        public bool?  IsConsumed       { get; internal set; }
        public string DeveloperPayload { get; }

        [Preserve]
        [JsonConstructor]
        internal Purchase(string paymentId, string productId, string purchaseToken, bool? isConsumed, string developerPayload)
        {
            this.PaymentId        = paymentId;
            this.ProductId        = productId;
            this.PurchaseToken    = purchaseToken;
            this.IsConsumed       = isConsumed;
            this.DeveloperPayload = developerPayload;
        }
    }
}