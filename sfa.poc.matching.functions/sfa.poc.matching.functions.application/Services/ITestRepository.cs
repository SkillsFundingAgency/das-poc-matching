using System.Collections.Generic;

namespace sfa.poc.matching.functions.application.Services
{
    public interface ITestRepository
    {
        IEnumerable<string> GetData();
    }
}
