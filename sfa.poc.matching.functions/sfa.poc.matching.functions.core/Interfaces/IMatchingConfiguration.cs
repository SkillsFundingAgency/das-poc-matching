
using sfa.poc.matching.functions.core.Configuration;

namespace sfa.poc.matching.functions.core.Interfaces
{
    public interface IMatchingConfiguration
    {
        AuthenticationConfig Authentication { get; set; }

        //NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        string PostcodeRetrieverBaseUrl { get; set; }

        string SqlConnectionString { get; set; }
    }
}
