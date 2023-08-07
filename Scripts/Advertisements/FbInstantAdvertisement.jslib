const FbInstantAdvertisingLibrary = {

    _showBannerAd: function (adId, callbackObj, callbackMethod, callbackId) {

        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

        FBInstant
            .loadBannerAdAsync(UTF8ToString(adId))
            .then(sendMessage)
            .catch(sendMessage);
    },

    _hideBannerAd: function (callbackObj, callbackMethod, callbackId) {

        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

        FBInstant
            .hideBannerAdAsync()
            .then(sendMessage)
            .catch(sendMessage);
    },

    _isInterstitialAdReady: function (adId) {
        return _isAdReady(adId);
    },

    _loadInterstitialAd: function (adId, callbackObj, callbackMethod, callbackId) {
        _loadAd(adId, true, callbackObj, callbackMethod, callbackId);
    },

    _showInterstitialAd: function (adId, callbackObj, callbackMethod, callbackId) {
        _showAd(adId, callbackObj, callbackMethod, callbackId);
    },

    _isRewardedAdReady: function (adId) {
        return _isAdReady(adId);
    },

    _loadRewardedAd: function (adId, callbackObj, callbackMethod, callbackId) {
        _loadAd(adId, false, callbackObj, callbackMethod, callbackId);
    },

    _showRewardedAd: function (adId, callbackObj, callbackMethod, callbackId) {
        _showAd(adId, callbackObj, callbackMethod, callbackId);
    },

    $_isAdReady: function (adId) {
        return !!(_getCache(UTF8ToString(adId)).loaded.length);
    },

    $_loadAd: function (adId, loadInter, callbackObj, callbackMethod, callbackId) {
        adId = UTF8ToString(adId);
        const cache = _getCache(adId);
        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

        const load = (ad) => ad.loadAsync()
            .then(() => {
                cache.loaded.push(ad);
                sendMessage();
            })
            .catch(error => {
                cache.loading.push(ad);
                sendMessage(error);
            });

        if (cache.loading.length) return load(cache.loading.shift());

        const promise = loadInter ? FBInstant.getInterstitialAdAsync(adId) : FBInstant.getRewardedVideoAsync(adId);

        promise.then(load).catch(sendMessage);
    },

    $_showAd: function (adId, callbackObj, callbackMethod, callbackId) {
        adId = UTF8ToString(adId);
        const cache = _getCache(adId);
        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

        if (!cache.loaded.length) return sendMessage(`Ad "${adId}" not loaded`);

        const ad = cache.loaded.shift();

        ad.showAsync()
            .then(sendMessage)
            .catch(error => {
                cache.loaded.push(ad);
                sendMessage(error);
            });
    },

    $_ads: {},

    $_getCache: function (adId) {
        adId = UTF8ToString(adId);
        if (!(adId in _ads)) {
            _ads[adId] = {
                loading: [],
                loaded: [],
            };
        }
        return _ads[adId];
    },

    $_sendMessage: function (error, callbackObj, callbackMethod, callbackId, params = null) {
        SendMessage(
            UTF8ToString(callbackObj),
            UTF8ToString(callbackMethod),
            JSON.stringify(Object.assign(
                {
                    error: error ? JSON.stringify(error) : null,
                    callbackId: UTF8ToString(callbackId),
                },
                params
            )),
        );
    },
};

autoAddDeps(FbInstantAdvertisingLibrary, "$_isAdReady");
autoAddDeps(FbInstantAdvertisingLibrary, "$_loadAd");
autoAddDeps(FbInstantAdvertisingLibrary, "$_showAd");
autoAddDeps(FbInstantAdvertisingLibrary, "$_ads");
autoAddDeps(FbInstantAdvertisingLibrary, "$_getCache");
autoAddDeps(FbInstantAdvertisingLibrary, "$_sendMessage");
mergeInto(LibraryManager.library, FbInstantAdvertisingLibrary);