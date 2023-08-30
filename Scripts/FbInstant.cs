namespace FbInstant
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.Scripting;

    internal sealed class FbInstant : MonoBehaviour
    {
        public static FbInstant Instantiate()
        {
            var instance = new GameObject(nameof(FbInstant)).AddComponent<FbInstant>();
            DontDestroyOnLoad(instance);
            return instance;
        }

        private readonly Dictionary<string, UniTaskCompletionSource<Result<string>>> _tcs = new();

        public UniTask<Result<string>> Invoke(object data, FbInstantAction<string> action) => this.Invoke(JsonConvert.SerializeObject(data), action);

        public UniTask<Result<string>> Invoke(string data, FbInstantAction<string> action) => this.Invoke((callbackObj, callbackMethod, callbackId) => action(data, callbackObj, callbackMethod, callbackId));

        public async UniTask<Result<string>> Invoke(FbInstantAction action)
        {
            var callbackId = Guid.NewGuid().ToString();
            this._tcs.Add(callbackId, new());
            try
            {
                action(this.gameObject.name, nameof(this.Callback), callbackId);
                return await this._tcs[callbackId].Task;
            }
            finally
            {
                this._tcs.Remove(callbackId);
            }
        }

        private void Callback(string json)
        {
            var message = JsonConvert.DeserializeObject<Message>(json);
            this._tcs[message.CallbackId].TrySetResult(message);
        }

        private sealed class Message : Result<string>
        {
            public string CallbackId { get; }

            [Preserve]
            [JsonConstructor]
            public Message(string data, string error, string callbackId) : base(data, error)
            {
                this.CallbackId = callbackId;
            }
        }
    }
}