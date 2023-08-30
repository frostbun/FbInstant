namespace FbInstant
{
    internal delegate void FbInstantAction(string callbackObj, string callbackMethod, string callbackId);

    internal delegate void FbInstantAction<in T>(T data, string callbackObj, string callbackMethod, string callbackId);
}