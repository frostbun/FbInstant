#nullable enable
namespace UniT.FbInstant
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public static partial class FbInstant
    {
        public static class Player
        {
            #region Public

            public static string Id     => _getPlayerId();
            public static string Name   => _getPlayerName();
            public static string Avatar => _getPlayerAvatar();

            public static Task<string[]> LoadDataAsync(string[] keys) => This.InvokeAsync(keys, _loadPlayerData).Convert<string[]>();

            public static Task SaveDataAsync(string[] keys, string[] rawDatas) => This.InvokeAsync(new Dictionary<string, string>(Enumerable.Zip(keys, rawDatas, (key, rawData) => new KeyValuePair<string, string>(key, rawData))), _savePlayerData);

            public static Task FlushDataAsync() => This.InvokeAsync(_flushPlayerData);

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