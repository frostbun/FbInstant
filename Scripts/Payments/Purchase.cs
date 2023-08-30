namespace FbInstant.Payments
{
    using Newtonsoft.Json;

    public sealed class Purchase
    {
        [JsonProperty] public string PaymentId        { get; }
        [JsonProperty] public string ProductId        { get; }
        [JsonProperty] public string PurchaseToken    { get; }
        [JsonProperty] public bool   IsConsumed       { get; internal set; }
        [JsonProperty] public string DeveloperPayload { get; }
    }
}