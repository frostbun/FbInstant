namespace FbInstant
{
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;

    internal static class FbInstantExtensions
    {
        public static UniTask<Result<T>> Convert<T>(this UniTask<Result<string>> task)
        {
            return task.ContinueWith(result => new Result<T>(JsonConvert.DeserializeObject<T>(result.Data), result.Error));
        }

        public static UniTask<Result> WithErrorOnly(this UniTask<Result<string>> task)
        {
            return task.ContinueWith(result => (Result)result);
        }
    }
}