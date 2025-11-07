namespace BasicNtierTemplate.Service.Contracts
{
    public sealed class Result
    {
        public bool IsSuccess { get; }
        public string? Data { get; } // Data can hold error message or other info

        private Result(bool success, string? data = null)
        {
            IsSuccess = success;
            Data = data;
        }

        public static Result Ok(string data) => new(true, data);
        public static Result Fail(string data) => new(false, data);

        public static implicit operator bool(Result result) => result.IsSuccess;

        public override string ToString() => IsSuccess ? "Success" : $"Error: {Data}";
    }

}
