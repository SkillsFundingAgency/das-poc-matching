
namespace SFA.POC.Matching.Proximity.Infrastructure.Configuration
{
    public class MatchingConfiguration : IMatchingConfiguration
    {
        public string CosmosConnectionString { get; set; }

        public string SqlConnectionString { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

    }
}
