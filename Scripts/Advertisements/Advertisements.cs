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

            public static Task ShowBannerAdAsync(string adId) => This.InvokeAsync(adId, _showBannerAd);

            public static Task HideBannerAdAsync() => This.InvokeAsync(_hideBannerAd);

            public static bool IsInterstitialAdReady(string adId) => _isInterstitialAdReady(adId);

            public static Task LoadInterstitialAdAsync(string adId) => This.InvokeAsync(adId, _loadInterstitialAd);

            public static Task ShowInterstitialAdAsync(string adId) => This.InvokeAsync(adId, _showInterstitialAd);

            public static bool IsRewardedAdReady(string adId) => _isRewardedAdReady(adId);

            public static Task LoadRewardedAdAsync(string adId) => This.InvokeAsync(adId, _loadRewardedAd);

            public static Task ShowRewardedAdAsync(string adId) => This.InvokeAsync(adId, _showRewardedAd);

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