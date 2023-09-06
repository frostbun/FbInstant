const Payments = {

    _getCatalog: function (callbackObj, callbackMethod, callbackId) {
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (products, error) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: JSON.stringify(products),
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.payments
            .getCatalogAsync()
            .then(products => sendMessage(products, null))
            .catch(error => sendMessage([], error));
    },

    _getPurchases: function (callbackObj, callbackMethod, callbackId) {
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (purchases, error) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: JSON.stringify(purchases),
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.payments
            .getPurchasesAsync()
            .then(purchases => sendMessage(purchases, null))
            .catch(error => sendMessage([], error));
    },

    _purchase: function (productId, developerPayload, callbackObj, callbackMethod, callbackId) {
        productId = UTF8ToString(productId);
        developerPayload = UTF8ToString(developerPayload);
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (purchase, error) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                data: JSON.stringify(purchase),
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.payments
            .purchaseAsync({
                productID: productId,
                developerPayload: developerPayload,
            })
            .then(purchase => sendMessage(purchase, null))
            .catch(error => sendMessage(null, error));
    },

    _consumePurchase: function (purchaseToken, callbackObj, callbackMethod, callbackId) {
        purchaseToken = UTF8ToString(purchaseToken);
        callbackObj = UTF8ToString(callbackObj);
        callbackMethod = UTF8ToString(callbackMethod);
        callbackId = UTF8ToString(callbackId);

        const sendMessage = (error = null) => SendMessage(
            callbackObj,
            callbackMethod,
            JSON.stringify({
                error: error ? JSON.stringify(error) : null,
                callbackId: callbackId,
            }),
        );

        FBInstant.payments
            .consumePurchaseAsync(purchaseToken)
            .then(sendMessage)
            .catch(sendMessage);
    },
};

mergeInto(LibraryManager.library, Payments);
