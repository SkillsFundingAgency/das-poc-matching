using sfa.poc.matching.functions.core.Interfaces;

namespace sfa.poc.matching.functions.core.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }

        //public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

        public string SqlConnectionString { get; set; }

    }
}
