namespace FbInstant.Social
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json;
    using UnityEngine;

    public sealed class FbInstantSocial
    {
        public void Invite(string text, Texture2D texture, Dictionary<string, object> @params = null)
        {
            @params          ??= new();
            @params["text"]  =   text;
            @params["image"] =   "data:image/png;base64," + Convert.ToBase64String(texture.EncodeToPNG());
            _invite(JsonConvert.SerializeObject(@params));
        }

        public void Share(string text, Texture2D texture, Dictionary<string, object> @params = null)
        {
            @params          ??= new();
            @params["text"]  =   text;
            @params["image"] =   "data:image/png;base64," + Convert.ToBase64String(texture.EncodeToPNG());
            _share(JsonConvert.SerializeObject(@params));
        }

        #region DllImport

        [DllImport("__Internal")]
        private static extern void _invite(string @params);

        [DllImport("__Internal")]
        private static extern void _share(string @params);

        #endregion
    }
}