namespace FbInstant
{
    using Newtonsoft.Json;

    public class Result
    {
        public string Error { get; }

        [JsonIgnore] public bool IsSuccess => this.Error is null;
        [JsonIgnore] public bool IsError   => this.Error is not null;

        internal Result(string error)
        {
            this.Error = error;
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        internal Result(T data, string error) : base(error)
        {
            this.Data = data;
        }
    }
}