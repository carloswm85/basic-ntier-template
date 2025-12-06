namespace BasicNtierTemplate.Service.Contracts
{
    public sealed class OperationResult
    {
        public bool IsSuccess { get; }
        public string? Data { get; } // Data can hold error message or other info

        private OperationResult(bool success, string? data = null)
        {
            IsSuccess = success;
            Data = data;
        }

        public static OperationResult Ok(string data) => new(true, data);
        public static OperationResult Fail(string data) => new(false, data);

        public static implicit operator bool(OperationResult result) => result.IsSuccess;

        public override string ToString() => IsSuccess ? "Success" : $"Error: {Data}";
    }

}
