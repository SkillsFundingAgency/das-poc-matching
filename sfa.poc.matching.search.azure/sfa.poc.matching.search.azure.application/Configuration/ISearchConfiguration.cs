
namespace sfa.poc.matching.search.azure.application.Configuration
{
    public interface ISearchConfiguration
    {
        string AzureSearchConnectionString { get; set; }

        string SqlConnectionString { get; set; }
    }
}
