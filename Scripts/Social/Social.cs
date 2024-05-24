#nullable enable
namespace UniT.FbInstant
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json;
    using UnityEngine;

    public static partial class FbInstant
    {
        public static class Social
        {
            #region Public

            public static void Invite(string text, Texture2D texture, Dictionary<string, object>? @params = null)
            {
                @params          ??= new Dictionary<string, object>();
                @params["text"]  =   text;
                @params["image"] =   "data:image/png;base64," + System.Convert.ToBase64String(texture.EncodeToPNG());
                _invite(JsonConvert.SerializeObject(@params));
            }

            public static void Share(string text, Texture2D texture, Dictionary<string, object>? @params = null)
            {
                @params          ??= new Dictionary<string, object>();
                @params["text"]  =   text;
                @params["image"] =   "data:image/png;base64," + System.Convert.ToBase64String(texture.EncodeToPNG());
                _share(JsonConvert.SerializeObject(@params));
            }

            #endregion

            #region DllImport

            [DllImport("__Internal")]
            private static extern void _invite(string @params);

            [DllImport("__Internal")]
            private static extern void _share(string @params);

            #endregion
        }
    }
}