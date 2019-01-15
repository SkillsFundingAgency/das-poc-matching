
namespace sfa.poc.matching.search.azure.application.Configuration
{
    public interface ISearchConfiguration
    {
        AzureSearchConfiguration AzureSearchConfiguration { get; set; }

        string PostcodeRetrieverBaseUrl { get; set; }

        string SqlConnectionString { get; set; }
    }
}
