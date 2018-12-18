﻿using System.Threading.Tasks;
using NUnit.Framework;
using sfa.poc.matching.search.azure.application.Configuration;

namespace sfa.poc.matching.search.azure.tests
{
    [TestFixture]
    public class ConfigurationAcceptanceTests
    {
        [Test]
        public async Task WhenConfigurationIsLoadedFromTableStorageThenTheValuesArePopulated()
        {
            var config = await ConfigurationService.GetConfig(
                GlobalSetup.Configuration["EnvironmentName"],
                GlobalSetup.Configuration["ConfigurationStorageConnectionString"],
                GlobalSetup.Configuration["Version"],
                GlobalSetup.Configuration["ServiceName"]);

            Assert.IsNotNull(config);
            Assert.IsFalse(string.IsNullOrEmpty(config.SqlConnectionString));
            Assert.IsFalse(string.IsNullOrEmpty(config.AzureSearchConnectionString));
        }
    }
}
