using Newtonsoft.Json;

namespace sfa.poc.matching.configuration.Configuration
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired]
        public AuthenticationConfig Authentication { get; set; }

        [JsonRequired]
        public string SqlConnectionString { get; set; }
    }
}
