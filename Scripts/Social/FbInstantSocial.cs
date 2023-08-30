namespace FbInstant.Social
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;

    public sealed class FbInstantSocial
    {
        #region Public

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

        #endregion

        #region Constructor

        private readonly FbInstant _fbInstant;

        [Preserve]
        public FbInstantSocial()
        {
            this._fbInstant = Object.FindObjectOfType<FbInstant>() ?? FbInstant.Instantiate();
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