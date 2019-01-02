using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.core.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public string SqlConnectionString { get; set; }
    }
}
