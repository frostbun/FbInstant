namespace FbInstant.Advertisements
{
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class FbInstantAdvertisement
    {
        #region Public

        public UniTask<Result> ShowBannerAd(string adId) => this._fbInstant.Invoke(adId, _showBannerAd).WithErrorOnly();

        public UniTask<Result> HideBannerAd() => this._fbInstant.Invoke(_hideBannerAd).WithErrorOnly();

        public bool IsInterstitialAdReady(string adId) => _isInterstitialAdReady(adId);

        public UniTask<Result> LoadInterstitialAd(string adId) => this._fbInstant.Invoke(adId, _loadInterstitialAd).WithErrorOnly();

        public UniTask<Result> ShowInterstitialAd(string adId) => this._fbInstant.Invoke(adId, _showInterstitialAd).WithErrorOnly();

        public bool IsRewardedAdReady(string adId) => _isRewardedAdReady(adId);

        public UniTask<Result> LoadRewardedAd(string adId) => this._fbInstant.Invoke(adId, _loadRewardedAd).WithErrorOnly();

        public UniTask<Result> ShowRewardedAd(string adId) => this._fbInstant.Invoke(adId, _showRewardedAd).WithErrorOnly();

        #endregion

        #region Constructor

        private readonly FbInstant _fbInstant;

        [Preserve]
        public FbInstantAdvertisement()
        {
            this._fbInstant = Object.FindObjectOfType<FbInstant>() ?? FbInstant.Instantiate();
        }

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