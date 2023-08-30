namespace FbInstant.Payments
{
    using Newtonsoft.Json;

    public sealed class Product
    {
        [JsonProperty] public string ProductId         { get; }
        [JsonProperty] public string Title             { get; }
        [JsonProperty] public string Description       { get; }
        [JsonProperty] public string ImageUri          { get; }
        [JsonProperty] public string Price             { get; }
        [JsonProperty] public float  PriceAmount       { get; }
        [JsonProperty] public string PriceCurrencyCode { get; }
    }
}