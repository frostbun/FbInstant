const Social = {

    _invite: function (params) {
        FBInstant
            .inviteAsync(JSON.parse(UTF8ToString(params)))
            .catch(error => console.error(error));
    },

    _share: function (params) {
        FBInstant
            .shareAsync(JSON.parse(UTF8ToString(params)))
            .catch(error => console.error(error));
    },
};

mergeInto(LibraryManager.library, Social);
