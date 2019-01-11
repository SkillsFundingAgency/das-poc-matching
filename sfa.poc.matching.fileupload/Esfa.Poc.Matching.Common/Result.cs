namespace Esfa.Poc.Matching.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok() =>
            new Result(true, string.Empty);

        public static Result Fail(string message) =>
            new Result(false, message);
    }
}