namespace UniT.FbInstant
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Player
        {
            #region Public

            public static string Id     => _getPlayerId();
            public static string Name   => _getPlayerName();
            public static string Avatar => _getPlayerAvatar();

            public static UniTask<Result<string[]>> LoadData(string[] keys) => This.Invoke(keys, _loadPlayerData).Convert<string[]>();

            public static UniTask<Result> SaveData(string[] keys, string[] rawDatas)
            {
                var dictionary = new Dictionary<string, string>();
                for (var i = 0; i < keys.Length; ++i)
                {
                    dictionary[keys[i]] = rawDatas[i];
                }
                return This.Invoke(dictionary, _savePlayerData).WithErrorOnly();
            }

            public static UniTask<Result> FlushData() => This.Invoke(_flushPlayerData).WithErrorOnly();

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
}