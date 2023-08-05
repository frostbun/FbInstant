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

        const sendMessage = (data, error = null) => _sendMessage(
            error,
            callbackObj,
            callbackMethod,
            callbackId,
            { data: JSON.stringify(data) }
        );

        FBInstant.player
            .getDataAsync(keys)
            .then(data => sendMessage(keys.map(key => JSON.stringify(data[key]))))
            .catch(error => sendMessage(keys.map(_ => null), error));
    },

    _savePlayerData: function (json, callbackObj, callbackMethod, callbackId) {

        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

        FBInstant.player
            .setDataAsync(JSON.parse(UTF8ToString(json)))
            .then(sendMessage)
            .catch(sendMessage);
    },

    _flushPlayerData: function (callbackObj, callbackMethod, callbackId) {

        const sendMessage = (error = null) => _sendMessage(error, callbackObj, callbackMethod, callbackId);

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

    $_sendMessage: function (error, callbackObj, callbackMethod, callbackId, params = null) {
        SendMessage(
            UTF8ToString(callbackObj),
            UTF8ToString(callbackMethod),
            JSON.stringify({
                error: error ? JSON.stringify(error) : null,
                callbackId: UTF8ToString(callbackId),
                ...params,
            })
        );
    },
};

autoAddDeps(FbInstantPlayerLibrary, "$_getBuffer");
autoAddDeps(FbInstantPlayerLibrary, "$_sendMessage");
mergeInto(LibraryManager.library, FbInstantPlayerLibrary);