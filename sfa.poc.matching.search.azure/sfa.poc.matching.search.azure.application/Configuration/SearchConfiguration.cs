
namespace sfa.poc.matching.search.azure.application.Configuration
{
    public class SearchConfiguration : ISearchConfiguration
    {
        public string AzureSearchConnectionString { get; set; }

        public string SqlConnectionString { get; set; }
    }
}
