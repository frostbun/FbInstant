namespace FbInstant
{
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Advertisements
        {
            #region Public

            public static UniTask<Result> ShowBannerAd(string adId) => This.Invoke(adId, _showBannerAd).WithErrorOnly();

            public static UniTask<Result> HideBannerAd() => This.Invoke(_hideBannerAd).WithErrorOnly();

            public static bool IsInterstitialAdReady(string adId) => _isInterstitialAdReady(adId);

            public static UniTask<Result> LoadInterstitialAd(string adId) => This.Invoke(adId, _loadInterstitialAd).WithErrorOnly();

            public static UniTask<Result> ShowInterstitialAd(string adId) => This.Invoke(adId, _showInterstitialAd).WithErrorOnly();

            public static bool IsRewardedAdReady(string adId) => _isRewardedAdReady(adId);

            public static UniTask<Result> LoadRewardedAd(string adId) => This.Invoke(adId, _loadRewardedAd).WithErrorOnly();

            public static UniTask<Result> ShowRewardedAd(string adId) => This.Invoke(adId, _showRewardedAd).WithErrorOnly();

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