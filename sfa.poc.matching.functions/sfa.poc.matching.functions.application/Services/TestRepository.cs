using System.Collections.Generic;
using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.application.Services
{
    public class TestRepository : ITestRepository
    {
        private IMatchingConfiguration _configuration;

        public TestRepository(IMatchingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> GetData()
        {
            return new List<string> { "Some", "strings"};
        }
    }
}
