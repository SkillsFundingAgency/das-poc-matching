using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace sfa.poc.matching.search.azure.tests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        public static IConfigurationRoot Configuration { get; private set; }

        [OneTimeSetUp]
        public void TestSetup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
        }
    }
}
