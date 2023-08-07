const FbInstantPlayerLibrary = {

    _getPlayerId: function () {
        return _getBuffer(FBInstant.player.getID());
    },

    _getPlayerName: function () {
        return _getBuffer(FBInstant.player.getName());
    },

    _getPlayerAvatar: function () {
        return _getBuffer(FBInstant.player.getPhoto());
    },

    _loadPlayerData: function (keys, callbackObj, callbackMethod, callbackId) {
        keys = JSON.parse(UTF8ToString(keys));
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (data, error = null) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: JSON.stringify(data),
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.player
            .getDataAsync(keys)
            .then(data => sendMessage(keys.map(key => JSON.stringify(data[key]))))
            .catch(error => sendMessage(keys.map(_ => null), error));
    },

    _savePlayerData: function (json, callbackObj, callbackMethod, callbackId) {
        json = JSON.parse(UTF8ToString(json));
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (error = null) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: null,
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.player
            .setDataAsync(json)
            .then(sendMessage)
            .catch(sendMessage);
    },

    _flushPlayerData: function (callbackObj, callbackMethod, callbackId) {
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (error = null) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: null,
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.player
            .flushDataAsync()
            .then(sendMessage)
            .catch(sendMessage);
    },

    $_getBuffer: function (str) {
        str = str || "";
        const size = lengthBytesUTF8(str) + 1;
        const buffer = _malloc(size);
        stringToUTF8(str, buffer, size);
        return buffer;
    },
};

autoAddDeps(FbInstantPlayerLibrary, "$_getBuffer");
mergeInto(LibraryManager.library, FbInstantPlayerLibrary);
