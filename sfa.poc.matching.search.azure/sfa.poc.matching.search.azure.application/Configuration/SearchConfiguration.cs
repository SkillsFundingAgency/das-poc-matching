
namespace sfa.poc.matching.search.azure.application.Configuration
{
    public class SearchConfiguration : ISearchConfiguration
    {
        public AzureSearchConfiguration AzureSearchConfiguration { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

        public string SqlConnectionString { get; set; }
    }
}
