namespace FbInstant.Player
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class FbInstantPlayer
    {
        #region Public

        public string Id     => _getPlayerId();
        public string Name   => _getPlayerName();
        public string Avatar => _getPlayerAvatar();

        public UniTask<Result<string[]>> LoadData(string[] keys) => this._fbInstant.Invoke(keys, _loadPlayerData).Convert<string[]>();

        public UniTask<Result> SaveData(string[] keys, string[] rawDatas) => this._fbInstant.Invoke(ToDictionary(keys, rawDatas), _savePlayerData).WithErrorOnly();

        public UniTask<Result> FlushData() => this._fbInstant.Invoke(_flushPlayerData).WithErrorOnly();

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

        #region Constructor

        private readonly FbInstant _fbInstant;

        [Preserve]
        public FbInstantPlayer()
        {
            this._fbInstant = Object.FindObjectOfType<FbInstant>() ?? FbInstant.Instantiate();
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