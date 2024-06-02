#nullable enable
namespace UniT.FbInstant
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityEngine.Scripting;

    public static partial class FbInstant
    {
        private sealed class This : MonoBehaviour
        {
            private const string CALLBACK_OBJ    = nameof(FbInstant);
            private const string CALLBACK_METHOD = nameof(Callback);

            static This() => DontDestroyOnLoad(new GameObject(CALLBACK_OBJ).AddComponent<This>());

            private static readonly Dictionary<string, TaskCompletionSource<string?>> Tcs = new Dictionary<string, TaskCompletionSource<string?>>();

            public static Task<string?> InvokeAsync(object data, Action<string> action) => InvokeAsync(JsonConvert.SerializeObject(data), action);

            public static Task<string?> InvokeAsync(string data, Action<string> action) => InvokeAsync((callbackObj, callbackMethod, callbackId) => action(data, callbackObj, callbackMethod, callbackId));

            public static async Task<string?> InvokeAsync(Action action)
            {
                var callbackId = Guid.NewGuid().ToString();
                Tcs.Add(callbackId, new TaskCompletionSource<string?>());
                try
                {
                    action(CALLBACK_OBJ, CALLBACK_METHOD, callbackId);
                    return await Tcs[callbackId].Task;
                }
                finally
                {
                    Tcs.Remove(callbackId);
                }
            }

            private void Callback(string json)
            {
                var result = JsonConvert.DeserializeObject<Result>(json)!;
                if (result.Error is null)
                {
                    Tcs[result.CallbackId].TrySetResult(result.Data);
                }
                else
                {
                    Tcs[result.CallbackId].TrySetException(new Exception(result.Error));
                }
            }

            private sealed class Result
            {
                public string? Data       { get; }
                public string? Error      { get; }
                public string  CallbackId { get; }

                [Preserve]
                public Result(string? data, string? error, string callbackId)
                {
                    this.Data       = data;
                    this.Error      = error;
                    this.CallbackId = callbackId;
                }
            }
        }

        public class Exception : System.Exception
        {
            public Exception(string message) : base(message)
            {
            }
        }

        private static async Task<T> Convert<T>(this Task<string?> task)
        {
            var json = await task;
            return JsonConvert.DeserializeObject<T>(json!)!;
        }

        private delegate void Action(string callbackObj, string callbackMethod, string callbackId);

        private delegate void Action<in T>(T data, string callbackObj, string callbackMethod, string callbackId);
    }
}