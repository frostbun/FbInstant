#nullable enable
namespace UniT.FbInstant
{
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Advertisements
        {
            #region Public

            public static Task<Result> ShowBannerAdAsync(string adId) => This.InvokeAsync(adId, _showBannerAd).WithErrorOnly();

            public static Task<Result> HideBannerAdAsync() => This.InvokeAsync(_hideBannerAd).WithErrorOnly();

            public static bool IsInterstitialAdReady(string adId) => _isInterstitialAdReady(adId);

            public static Task<Result> LoadInterstitialAdAsync(string adId) => This.InvokeAsync(adId, _loadInterstitialAd).WithErrorOnly();

            public static Task<Result> ShowInterstitialAdAsync(string adId) => This.InvokeAsync(adId, _showInterstitialAd).WithErrorOnly();

            public static bool IsRewardedAdReady(string adId) => _isRewardedAdReady(adId);

            public static Task<Result> LoadRewardedAdAsync(string adId) => This.InvokeAsync(adId, _loadRewardedAd).WithErrorOnly();

            public static Task<Result> ShowRewardedAdAsync(string adId) => This.InvokeAsync(adId, _showRewardedAd).WithErrorOnly();

            #endregion

            #region DllImport

            [DllImport("__Internal")]
            private static extern void _showBannerAd(string adId, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _hideBannerAd(string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern bool _isInterstitialAdReady(string adId);

            [DllImport("__Internal")]
            private static extern void _loadInterstitialAd(string adId, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _showInterstitialAd(string adId, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern bool _isRewardedAdReady(string adId);

            [DllImport("__Internal")]
            private static extern void _loadRewardedAd(string adId, string callbackObj, string callbackMethod, string callbackId);

            [DllImport("__Internal")]
            private static extern void _showRewardedAd(string adId, string callbackObj, string callbackMethod, string callbackId);

            #endregion
        }
    }
}