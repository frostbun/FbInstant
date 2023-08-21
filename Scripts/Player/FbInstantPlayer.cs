namespace FbInstant.Player
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UnityEngine;

    public class FbInstantPlayer : MonoBehaviour
    {
        #region Public

        public static FbInstantPlayer Instantiate()
        {
            var instance = new GameObject(nameof(FbInstantPlayer) + Guid.NewGuid())
                .AddComponent<FbInstantPlayer>();
            DontDestroyOnLoad(instance);
            return instance;
        }

        public string Id     => _getPlayerId();
        public string Name   => _getPlayerName();
        public string Avatar => _getPlayerAvatar();

        public UniTask<(string[] rawDatas, string error)> LoadData(string[] keys) => this.Invoke(JsonConvert.SerializeObject(keys), _loadPlayerData).ContinueWith(tuple => (JsonConvert.DeserializeObject<string[]>(tuple.data), tuple.error));

        public UniTask<string> SaveData(string[] keys, string[] rawDatas) => this.Invoke(JsonConvert.SerializeObject(ToDictionary(keys, rawDatas)), _savePlayerData).ContinueWith(tuple => tuple.error);

        public UniTask<string> FlushData() => this.Invoke(_flushPlayerData).ContinueWith(tuple => tuple.error);

        #endregion

        #region Private

        private readonly Dictionary<string, UniTaskCompletionSource<(string data, string error)>> _tcs = new();

        private UniTask<(string data, string error)> Invoke(string data, Action<string, string, string, string> action) => this.Invoke((callbackObj, callbackMethod, callbackId) => action(data, callbackObj, callbackMethod, callbackId));

        private async UniTask<(string data, string error)> Invoke(Action<string, string, string> action)
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

        private void Callback(string message)
        {
            var @params    = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            var data       = @params["data"];
            var error      = @params["error"];
            var callbackId = @params["callbackId"];
            this._tcs[callbackId].TrySetResult((data, error));
        }

        private static Dictionary<string, string> ToDictionary(string[] keys, string[] values)
        {
            var dictionary = new Dictionary<string, string>();
            for (var i = 0; i < keys.Length; ++i)
            {
                dictionary[keys[i]] = values[i];
            }
            return dictionary;
        }

        #endregion

        #region DllImport

        [DllImport("__Internal")]
        private static extern string _getPlayerId();

        [DllImport("__Internal")]
        private static extern string _getPlayerName();

        [DllImport("__Internal")]
        private static extern string _getPlayerAvatar();

        [DllImport("__Internal")]
        private static extern void _loadPlayerData(string keys, string callbackObj, string callbackMethod, string callbackId);

        [DllImport("__Internal")]
        private static extern void _savePlayerData(string json, string callbackObj, string callbackMethod, string callbackId);

        [DllImport("__Internal")]
        private static extern void _flushPlayerData(string callbackObj, string callbackMethod, string callbackId);

        #endregion
    }
}