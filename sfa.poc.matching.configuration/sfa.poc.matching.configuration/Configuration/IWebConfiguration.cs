
namespace sfa.poc.matching.configuration.Configuration
{
    public interface IWebConfiguration
    {
        AuthenticationConfig Authentication { get; set; }

        string SqlConnectionString { get; set; }
    }
}
