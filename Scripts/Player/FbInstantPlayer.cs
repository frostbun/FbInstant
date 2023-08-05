namespace FbInstant.Player
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;
    using UniT.Extensions;
    using UnityEngine;

    public class FbInstantPlayer : MonoBehaviour
    {
        #region Public

        public static FbInstantPlayer Instantiate()
        {
            return new GameObject(nameof(FbInstantPlayer) + Guid.NewGuid())
                   .AddComponent<FbInstantPlayer>()
                   .DontDestroyOnLoad();
        }

        public string Id     => _getPlayerId();
        public string Name   => _getPlayerName();
        public string Avatar => _getPlayerAvatar();

        public UniTask<(string[], string)> LoadData(string[] keys) => this.Invoke(JsonConvert.SerializeObject(keys), _loadPlayerData).ContinueWith((data, error) => (JsonConvert.DeserializeObject<string[]>(data), error));

        public UniTask<string> SaveData(string[] keys, string[] rawDatas) => this.Invoke(JsonConvert.SerializeObject(IterTools.Zip(keys, rawDatas).ToDictionary()), _savePlayerData).ContinueWith((_, error) => error);

        public UniTask<string> FlushData() => this.Invoke(_flushPlayerData).ContinueWith((_, error) => error);

        #endregion

        #region Private

        private readonly Dictionary<string, UniTaskCompletionSource<(string, string)>> _tcs = new();

        private UniTask<(string, string)> Invoke(string data, Action<string, string, string, string> action) => this.Invoke((callbackObj, callbackMethod, callbackId) => action(data, callbackObj, callbackMethod, callbackId));

        private UniTask<(string, string)> Invoke(Action<string, string, string> action)
        {
            var callbackId = Guid.NewGuid().ToString();
            this._tcs.Add(callbackId, new());
            action(this.gameObject.name, nameof(this.Callback), callbackId);
            return this._tcs[callbackId].Task.Finally(() => this._tcs.Remove(callbackId));
        }

        private void Callback(string message)
        {
            var @params    = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            var data       = @params["data"];
            var error      = @params["error"];
            var callbackId = @params["callbackId"];
            this._tcs[callbackId].TrySetResult((data, error));
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