using Esfa.Poc.Matching.Common;

namespace Esfa.Poc.Matching.Application
{
    public class ReturnResult<T>
    {
        public T Object { get; }
        public Result Result { get; }

        public ReturnResult() { }

        public ReturnResult(T t, Result result)
        {
            Object = t;
            Result = result;
        }

        public ReturnResult(Result result)
        {
            Result = result;
        }
    }
}