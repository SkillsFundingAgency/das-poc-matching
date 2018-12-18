using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using sfa.poc.matching.search.azure.application.Configuration;

namespace sfa.poc.matching.search.azure.tests
{
    [TestFixture]
    public class ConfigurationAcceptanceTests
    {
        private IConfigurationRoot _configuration;
        [OneTimeSetUp]
        public async Task TestSetup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
        }

        [Test]
        public async Task WhenConfigurationIsLoadedFromTableStorageThenTheValuesArePopulated()
        {
            var config = await ConfigurationService.GetConfig(
                _configuration["EnvironmentName"],
                _configuration["ConfigurationStorageConnectionString"],
                _configuration["Version"],
                _configuration["ServiceName"]);

            Assert.IsNotNull(config);
            Assert.IsFalse(string.IsNullOrEmpty(config.SqlConnectionString));
            Assert.IsFalse(string.IsNullOrEmpty(config.AzureSearchConnectionString));
        }
    }
}
