
namespace SFA.POC.Matching.Proximity.Infrastructure.Configuration
{
    public interface IMatchingConfiguration
    {
        string CosmosConnectionString { get; set; }

        string SqlConnectionString { get; set; }

        string PostcodeRetrieverBaseUrl { get; set; }

        string GoogleMapsApiKey { get; set; }
    }
}
